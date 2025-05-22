using FlightRegistration.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic; // For IEnumerable

namespace FlightRegistration.Services.DataAccess.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByPassengerAndFlightAsync(int passengerId, int flightId);
        Task<Booking?> GetBookingWithDetailsAsync(int bookingId); // To include Flight and Passenger info
        // Add other methods if needed
    }
}