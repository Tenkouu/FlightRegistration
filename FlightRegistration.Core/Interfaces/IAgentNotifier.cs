// File: FlightRegistration.Core/Interfaces/IAgentNotifier.cs
using System.Threading.Tasks;
using FlightRegistration.Core.Models; // For FlightStatus

namespace FlightRegistration.Core.Interfaces
{
    public interface IAgentNotifier
    {
        Task NotifySeatReservedAsync(int flightId, int seatId, string seatNumber, string agentIdWhoReserved);
        Task NotifySeatReservationFailedAsync(int flightId, int seatId, string seatNumber, string attemptingAgentId, string reason);
        Task NotifyFlightStatusChangedAsync(int flightId, FlightStatus newStatus, string flightNumber);
    }
}