using FlightRegistration.Core.DTOs;
using System.Collections.Generic; // For IEnumerable
using System.Threading.Tasks;

namespace FlightRegistration.Services.BusinessLogic.Interfaces
{
    public interface ICheckInService
    {
        Task<IEnumerable<PassengerBookingDetailsDto>> SearchPassengerBookingsAsync(PassengerSearchRequestDto searchRequest);
        Task<(bool Success, string Message, BoardingPassDto? BoardingPass)> AssignSeatAsync(SeatAssignmentRequestDto seatAssignmentRequest);
        // We might add a method to get available seats for a flight later if needed by the UI directly through this service
        // Task<IEnumerable<SeatDto>> GetAvailableSeatsForFlightAsync(int flightId);
    }
}