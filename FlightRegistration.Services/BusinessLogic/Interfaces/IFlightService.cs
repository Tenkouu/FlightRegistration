using FlightRegistration.Core.DTOs;
using FlightRegistration.Core.Models; // For FlightStatus enum
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightRegistration.Services.BusinessLogic.Interfaces
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightDetailsDto>> GetAllFlightsAsync();
        Task<FlightDetailsDto?> GetFlightDetailsByIdAsync(int flightId);
        Task<bool> UpdateFlightStatusAsync(int flightId, FlightStatus newStatus);
        // Add more methods as needed, e.g., for creating flights if that's a requirement
    }
}