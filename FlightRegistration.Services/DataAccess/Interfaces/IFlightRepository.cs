using FlightRegistration.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightRegistration.Services.DataAccess.Interfaces
{
    public interface IFlightRepository
    {
        Task<Flight?> GetFlightByIdAsync(int flightId);
        Task<IEnumerable<Flight>> GetAllFlightsAsync();
        Task AddFlightAsync(Flight flight);
        Task UpdateFlightAsync(Flight flight);
        // Add other necessary methods like GetFlightByNumberAsync, etc.
    }
}