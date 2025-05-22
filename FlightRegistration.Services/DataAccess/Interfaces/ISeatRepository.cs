using FlightRegistration.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic; // For IEnumerable

namespace FlightRegistration.Services.DataAccess.Interfaces
{
    public interface ISeatRepository
    {
        Task<Seat?> GetSeatByIdAsync(int seatId);
        Task<IEnumerable<Seat>> GetSeatsByFlightIdAsync(int flightId);
        // Add other methods if needed
    }
}