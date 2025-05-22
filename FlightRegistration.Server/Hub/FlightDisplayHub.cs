// File: FlightRegistration.Server/Hubs/FlightDisplayHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using FlightRegistration.Core.Models; // For FlightStatus enum
using FlightRegistration.Core.DTOs;  // For FlightDetailsDto or a simplified version

namespace FlightRegistration.Server.Hubs
{
    public class FlightDisplayHub : Hub
    {
        // This method can be called by the server (e.g., from AgentNotifier or a service)
        // to send updates to ALL connected clients of this hub.
        public async Task SendFlightUpdate(FlightDetailsDto flightUpdate)
        {
            // "ReceiveFlightUpdate" is the method name that clients will listen for.
            await Clients.All.SendAsync("ReceiveFlightUpdate", flightUpdate);
        }

        public async Task SendFlightStatusUpdate(int flightId, string flightNumber, FlightStatus newStatus)
        {
            // "ReceiveFlightStatusUpdate" is another method clients can listen for.
            // This is more specific than sending the whole FlightDetailsDto if only status changed.
            await Clients.All.SendAsync("ReceiveFlightStatusUpdate", flightId, flightNumber, newStatus.ToString(), (int)newStatus);
        }


        // Optional: Override OnConnectedAsync and OnDisconnectedAsync for logging or group management
        public override async Task OnConnectedAsync()
        {
            // You could add users to groups here if you had different types of displays
            // e.g., await Groups.AddToGroupAsync(Context.ConnectionId, "DepartureDisplays");
            System.Diagnostics.Debug.WriteLine($"SignalR Client Connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            System.Diagnostics.Debug.WriteLine($"SignalR Client Disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}