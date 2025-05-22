// File: FlightRegistration.Server/Services/AgentNotifier.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FlightRegistration.Core.Interfaces;
using FlightRegistration.Core.Models;
using FlightRegistration.Server.Hubs; // <--- ADD THIS for FlightDisplayHub
using Microsoft.AspNetCore.SignalR;  // <--- ADD THIS for IHubContext

namespace FlightRegistration.Server.Services
{
    public class AgentNotifier : IAgentNotifier
    {
        private readonly List<Socket> _connectedClients = new List<Socket>();
        private readonly object _lock = new object();
        private readonly IHubContext<FlightDisplayHub> _flightDisplayHubContext; // <--- ADD THIS

        // Modify constructor to inject IHubContext
        public AgentNotifier(IHubContext<FlightDisplayHub> flightDisplayHubContext) // <--- MODIFIED
        {
            _flightDisplayHubContext = flightDisplayHubContext; // <--- ADD THIS
        }

        // ... (AddClient, RemoveClient, BroadcastMessageAsync methods remain the same) ...
        public void AddClient(Socket clientSocket)
        {
            lock (_lock)
            {
                if (!_connectedClients.Contains(clientSocket))
                {
                    _connectedClients.Add(clientSocket);
                    Console.WriteLine($"Socket Client connected: {clientSocket.RemoteEndPoint}. Total clients: {_connectedClients.Count}");
                }
            }
        }

        public void RemoveClient(Socket clientSocket)
        {
            lock (_lock)
            {
                _connectedClients.Remove(clientSocket);
                Console.WriteLine($"Socket Client disconnected: {clientSocket.RemoteEndPoint?.ToString() ?? "N/A"}. Total clients: {_connectedClients.Count}");
            }
        }

        private async Task BroadcastMessageAsync(object messagePayload)
        {
            var messageJson = JsonSerializer.Serialize(messagePayload, new JsonSerializerOptions { WriteIndented = false });
            var messageBytes = Encoding.UTF8.GetBytes(messageJson + Environment.NewLine);

            List<Socket> clientsToRemove = new List<Socket>();
            List<Socket> currentClients;

            lock (_lock)
            {
                currentClients = new List<Socket>(_connectedClients);
            }

            foreach (var client in currentClients)
            {
                if (client.Connected)
                {
                    try
                    {
                        await client.SendAsync(new ArraySegment<byte>(messageBytes), SocketFlags.None);
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine($"SocketException sending message to client {client.RemoteEndPoint}: {ex.Message}. Marking for removal.");
                        clientsToRemove.Add(client);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        Console.WriteLine($"ObjectDisposedException sending message to client {client.RemoteEndPoint}: {ex.Message}. Marking for removal.");
                        clientsToRemove.Add(client);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"General error sending message to client {client.RemoteEndPoint}: {ex.Message}");
                    }
                }
                else
                {
                    clientsToRemove.Add(client);
                }
            }

            if (clientsToRemove.Any())
            {
                lock (_lock)
                {
                    foreach (var clientToRemove in clientsToRemove)
                    {
                        if (_connectedClients.Remove(clientToRemove))
                        {
                            Console.WriteLine($"Removed disconnected/problematic client: {clientToRemove.RemoteEndPoint?.ToString() ?? "N/A"}. Total clients: {_connectedClients.Count}");
                        }
                        try
                        {
                            clientToRemove.Dispose();
                        }
                        catch { }
                    }
                }
            }
        }


        // --- Implementation of IAgentNotifier ---
        public async Task NotifySeatReservedAsync(int flightId, int seatId, string seatNumber, string agentIdWhoReserved)
        {
            var agentMessage = new
            {
                Type = "SeatReservedUpdate",
                Payload = new { FlightId = flightId, SeatId = seatId, SeatNumber = seatNumber, IsReserved = true, ReservedByAgentId = agentIdWhoReserved }
            };
            Console.WriteLine($"Broadcasting to Agent Sockets: {JsonSerializer.Serialize(agentMessage)}");
            await BroadcastMessageAsync(agentMessage);

            // For the public display, we might not send individual seat reservations,
            // or we might send a summarized update. For now, let's skip sending this to FlightDisplayHub
            // unless a specific requirement arises for it.
        }

        public async Task NotifySeatReservationFailedAsync(int flightId, int seatId, string seatNumber, string attemptingAgentId, string reason)
        {
            var agentMessage = new
            {
                Type = "SeatReservationFailed",
                Payload = new { FlightId = flightId, SeatId = seatId, SeatNumber = seatNumber, AttemptingAgentId = attemptingAgentId, Reason = reason }
            };
            Console.WriteLine($"Broadcasting to Agent Sockets: {JsonSerializer.Serialize(agentMessage)}");
            await BroadcastMessageAsync(agentMessage);
        }

        public async Task NotifyFlightStatusChangedAsync(int flightId, FlightStatus newStatus, string flightNumber)
        {
            // Message for Agent Sockets
            var agentMessage = new
            {
                Type = "FlightStatusUpdate",
                Payload = new { FlightId = flightId, FlightNumber = flightNumber, NewStatus = newStatus.ToString(), NewStatusId = (int)newStatus }
            };
            Console.WriteLine($"Broadcasting to Agent Sockets: {JsonSerializer.Serialize(agentMessage)}");
            await BroadcastMessageAsync(agentMessage);

            // Message for Flight Display Hub (SignalR)
            // The hub has a method `SendFlightStatusUpdate(int flightId, string flightNumber, FlightStatus newStatus)`
            // We can call it directly.
            Console.WriteLine($"Sending to FlightDisplayHub: Flight {flightNumber} status to {newStatus}");
            await _flightDisplayHubContext.Clients.All.SendAsync("ReceiveFlightStatusUpdate", flightId, flightNumber, newStatus.ToString(), (int)newStatus);
        }
    }
}