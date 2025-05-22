// In FlightRegistration.WinFormsClient/AgentSocketClient.cs
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms; // For Invoke if updating UI directly from here
using FlightRegistration.Core.Models; // For FlightStatus if used in deserialized messages

// Define classes for expected message structures from the server
// These should match what AgentNotifier sends.
public class SocketMessageBase<T>
{
    public string Type { get; set; }
    public T Payload { get; set; }
}

public class SeatReservedUpdatePayload
{
    public int FlightId { get; set; }
    public int SeatId { get; set; }
    public string SeatNumber { get; set; }
    public bool IsReserved { get; set; } // Should always be true for this message type
    public string ReservedByAgentId { get; set; }
}

public class FlightStatusUpdatePayload
{
    public int FlightId { get; set; }
    public string FlightNumber { get; set; }
    public string NewStatus { get; set; } // String representation of the enum
    public int NewStatusId { get; set; } // Integer value of the enum
}

// You might add more payload classes for other message types like SeatReservationFailedPayload

namespace FlightRegistration.WinFormsClient
{
    public class AgentSocketClient
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _receiveTask;

        // Events to notify the UI (or other parts of the client app)
        public event Action<SeatReservedUpdatePayload> OnSeatReservedUpdateReceived;
        public event Action<FlightStatusUpdatePayload> OnFlightStatusUpdateReceived;
        public event Action<string> OnLogMessage; // For general logging to UI
        public event Action OnDisconnected;

        private readonly string _serverIp;
        private readonly int _serverPort;

        public bool IsConnected => _tcpClient?.Connected ?? false;

        public AgentSocketClient(string serverIp = "127.0.0.1", int serverPort = 8899)
        {
            _serverIp = serverIp;
            _serverPort = serverPort;
        }

        public async Task ConnectAsync()
        {
            if (IsConnected) return;

            try
            {
                _tcpClient = new TcpClient();
                OnLogMessage?.Invoke($"Attempting to connect to socket server at {_serverIp}:{_serverPort}...");
                await _tcpClient.ConnectAsync(_serverIp, _serverPort);
                _stream = _tcpClient.GetStream();
                _cancellationTokenSource = new CancellationTokenSource();

                _receiveTask = Task.Run(() => ReceiveMessagesAsync(_cancellationTokenSource.Token));
                OnLogMessage?.Invoke("Connected to socket server.");
            }
            catch (Exception ex)
            {
                OnLogMessage?.Invoke($"Failed to connect to socket server: {ex.Message}");
                // Optionally rethrow or handle further
            }
        }

        private async Task ReceiveMessagesAsync(CancellationToken token)
        {
            var buffer = new byte[4096]; // Buffer for incoming data
            StringBuilder messageBuilder = new StringBuilder(); // To handle fragmented messages

            try
            {
                while (!token.IsCancellationRequested && _tcpClient.Connected)
                {
                    if (_stream == null || !_stream.CanRead)
                    {
                        await Task.Delay(100, token); // Wait a bit if stream is not ready
                        continue;
                    }

                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (bytesRead == 0)
                    {
                        // Server disconnected gracefully
                        OnLogMessage?.Invoke("Server closed the connection.");
                        break;
                    }

                    messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                    // Process complete messages (assuming newline delimiter from server)
                    string allMessages = messageBuilder.ToString();
                    int newlineIndex;
                    while ((newlineIndex = allMessages.IndexOf(Environment.NewLine)) >= 0)
                    {
                        string singleMessageJson = allMessages.Substring(0, newlineIndex);
                        allMessages = allMessages.Substring(newlineIndex + Environment.NewLine.Length);

                        ProcessMessage(singleMessageJson);
                    }
                    messageBuilder.Clear().Append(allMessages); // Keep any remaining part
                }
            }
            catch (OperationCanceledException)
            {
                OnLogMessage?.Invoke("Message receiving cancelled.");
            }
            catch (System.IO.IOException ex) // Often indicates connection closed by server
            {
                OnLogMessage?.Invoke($"IO Exception during receive: {ex.Message}. Connection likely closed.");
            }
            catch (Exception ex)
            {
                OnLogMessage?.Invoke($"Error receiving messages: {ex.Message}");
            }
            finally
            {
                OnLogMessage?.Invoke("Disconnected from socket server.");
                OnDisconnected?.Invoke();
                CloseConnection();
            }
        }

        private void ProcessMessage(string jsonMessage)
        {
            OnLogMessage?.Invoke($"Socket RAW MSG: {jsonMessage}");
            try
            {
                // First, try to deserialize into a base type to get the "Type" property
                var baseMessage = JsonSerializer.Deserialize<SocketMessageBase<object>>(jsonMessage);

                if (baseMessage == null || string.IsNullOrEmpty(baseMessage.Type))
                {
                    OnLogMessage?.Invoke($"Warning: Received message with no type: {jsonMessage}");
                    return;
                }

                switch (baseMessage.Type)
                {
                    case "SeatReservedUpdate":
                        var seatUpdate = JsonSerializer.Deserialize<SocketMessageBase<SeatReservedUpdatePayload>>(jsonMessage);
                        if (seatUpdate?.Payload != null)
                        {
                            OnLogMessage?.Invoke($"Seat Reserved: Flight {seatUpdate.Payload.FlightId}, Seat {seatUpdate.Payload.SeatNumber} by {seatUpdate.Payload.ReservedByAgentId}");
                            OnSeatReservedUpdateReceived?.Invoke(seatUpdate.Payload);
                        }
                        break;
                    case "FlightStatusUpdate":
                        var statusUpdate = JsonSerializer.Deserialize<SocketMessageBase<FlightStatusUpdatePayload>>(jsonMessage);
                        if (statusUpdate?.Payload != null)
                        {
                            OnLogMessage?.Invoke($"Flight Status Update: Flight {statusUpdate.Payload.FlightNumber} to {statusUpdate.Payload.NewStatus}");
                            OnFlightStatusUpdateReceived?.Invoke(statusUpdate.Payload);
                        }
                        break;
                    // Add cases for other message types like "SeatReservationFailed"
                    default:
                        OnLogMessage?.Invoke($"Received unknown message type: {baseMessage.Type}");
                        break;
                }
            }
            catch (JsonException jsonEx)
            {
                OnLogMessage?.Invoke($"Error deserializing JSON message: {jsonEx.Message}. Message: {jsonMessage}");
            }
            catch (Exception ex)
            {
                OnLogMessage?.Invoke($"Error processing message: {ex.Message}. Message: {jsonMessage}");
            }
        }

        public async Task SendMessageAsync(string message) // Basic send, for future use
        {
            if (!IsConnected || _stream == null)
            {
                OnLogMessage?.Invoke("Cannot send message, not connected.");
                return;
            }
            try
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message + Environment.NewLine); // Add newline
                await _stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                OnLogMessage?.Invoke($"Sent to server: {message}");
            }
            catch (Exception ex)
            {
                OnLogMessage?.Invoke($"Error sending message: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            OnLogMessage?.Invoke("Disconnecting from socket server...");
            CloseConnection();
        }

        private void CloseConnection()
        {
            _cancellationTokenSource?.Cancel();
            _receiveTask?.Wait(TimeSpan.FromSeconds(2)); // Give receive task a moment to finish
            _stream?.Close();
            _stream?.Dispose();
            _tcpClient?.Close();
            _tcpClient?.Dispose();
            _stream = null;
            _tcpClient = null;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}