using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; // For ILogger
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlightRegistration.Server.Services; // For AgentNotifier

namespace FlightRegistration.Server.Sockets
{
    public class AgentSocketServer : IHostedService
    {
        private readonly ILogger<AgentSocketServer> _logger;
        private readonly AgentNotifier _agentNotifier; // Use the concrete class to call Add/RemoveClient
        private TcpListener _tcpListener;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private const int Port = 8899; // Choose an available port

        public AgentSocketServer(ILogger<AgentSocketServer> logger, AgentNotifier agentNotifier)
        {
            _logger = logger;
            _agentNotifier = agentNotifier;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Agent Socket Server starting on port {Port}...", Port);
            _tcpListener = new TcpListener(IPAddress.Any, Port);
            _tcpListener.Start();

            // Start a task to listen for incoming connections
            Task.Run(() => ListenForClientsAsync(_cancellationTokenSource.Token), cancellationToken);

            _logger.LogInformation("Agent Socket Server started successfully.");
            return Task.CompletedTask;
        }

        private async Task ListenForClientsAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    _logger.LogInformation("Waiting for a socket client connection...");
                    Socket clientSocket = await _tcpListener.AcceptSocketAsync(token);
                    // Socket clientSocket = await _tcpListener.AcceptSocketAsync(); // Pre .NET 6
                    _logger.LogInformation("Socket Client connected: {RemoteEndPoint}", clientSocket.RemoteEndPoint);

                    _agentNotifier.AddClient(clientSocket);

                    // Handle each client in a separate task
                    _ = Task.Run(() => HandleClientAsync(clientSocket, token), token);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Listening for clients cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ListenForClientsAsync");
            }
            finally
            {
                _tcpListener?.Stop();
            }
        }

        private async Task HandleClientAsync(Socket clientSocket, CancellationToken token)
        {
            _logger.LogInformation("Handling client: {RemoteEndPoint}", clientSocket.RemoteEndPoint);
            var buffer = new byte[1024]; // Buffer for incoming data

            try
            {
                while (!token.IsCancellationRequested && clientSocket.Connected)
                {
                    int bytesRead = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                    if (bytesRead == 0)
                    {
                        // Client disconnected gracefully
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    _logger.LogInformation("Received from {RemoteEndPoint}: {Message}", clientSocket.RemoteEndPoint, message.Trim());

                    // Process the message (e.g., parse JSON, handle commands)
                    // For now, we just log. Later, this will involve parsing JSON and acting on commands
                    // like "TryLockSeat".
                    // Example: if (message.Contains("TryLockSeat")) { /* handle lock */ }

                    // Echo back for testing (optional)
                    // var echoMessage = Encoding.UTF8.GetBytes($"Server Echo: {message.Trim()}\n");
                    // await clientSocket.SendAsync(new ArraySegment<byte>(echoMessage), SocketFlags.None);
                }
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset || ex.SocketErrorCode == SocketError.TimedOut)
            {
                _logger.LogWarning("Client {RemoteEndPoint} disconnected abruptly (ConnectionReset/Timeout).", clientSocket.RemoteEndPoint);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Client handling cancelled for {RemoteEndPoint}.", clientSocket.RemoteEndPoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling client {RemoteEndPoint}", clientSocket.RemoteEndPoint);
            }
            finally
            {
                _logger.LogInformation("Client {RemoteEndPoint} processing finished.", clientSocket.RemoteEndPoint);
                _agentNotifier.RemoveClient(clientSocket);
                clientSocket.Close();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Agent Socket Server stopping.");
            _cancellationTokenSource.Cancel();
            _tcpListener?.Stop(); // Stop listening
            // _agentNotifier.ClearAllClients(); // Optionally clear clients and close their sockets
            _logger.LogInformation("Agent Socket Server stopped.");
            return Task.CompletedTask;
        }
    }
}