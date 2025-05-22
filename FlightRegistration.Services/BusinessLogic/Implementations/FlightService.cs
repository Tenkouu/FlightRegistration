// File: FlightRegistration.Services/BusinessLogic/Implementations/FlightService.cs
using FlightRegistration.Core.DTOs;
using FlightRegistration.Core.Models;
using FlightRegistration.Services.BusinessLogic.Interfaces;
using FlightRegistration.Services.DataAccess.Interfaces;
using FlightRegistration.Services.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightRegistration.Core.Interfaces; // <--- ENSURE THIS USING IS CORRECT
using System; // For Console.WriteLine

namespace FlightRegistration.Services.BusinessLogic.Implementations
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly AppDbContext _context;
        private readonly IAgentNotifier _agentNotifier; // Dependency on the interface

        public FlightService(
            IFlightRepository flightRepository,
            AppDbContext context,
            IAgentNotifier agentNotifier) // Injected here
        {
            _flightRepository = flightRepository;
            _context = context;
            _agentNotifier = agentNotifier; // Assigned here
        }

        // ... (GetAllFlightsAsync and GetFlightDetailsByIdAsync methods from before) ...
        public async Task<IEnumerable<FlightDetailsDto>> GetAllFlightsAsync()
        {
            var flights = await _flightRepository.GetAllFlightsAsync();
            return flights.Select(f => new FlightDetailsDto
            {
                Id = f.Id,
                FlightNumber = f.FlightNumber,
                DepartureCity = f.DepartureCity,
                ArrivalCity = f.ArrivalCity,
                DepartureTime = f.DepartureTime,
                ArrivalTime = f.ArrivalTime,
                Status = f.Status,
                Seats = f.Seats.Select(s => new SeatDto
                {
                    Id = s.Id,
                    SeatNumber = s.SeatNumber,
                    IsReserved = s.IsReserved,
                }).ToList()
            }).ToList();
        }

        public async Task<FlightDetailsDto?> GetFlightDetailsByIdAsync(int flightId)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(flightId);
            if (flight == null) return null;

            return new FlightDetailsDto
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber,
                DepartureCity = flight.DepartureCity,
                ArrivalCity = flight.ArrivalCity,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Status = flight.Status,
                Seats = flight.Seats.Select(s => new SeatDto
                {
                    Id = s.Id,
                    SeatNumber = s.SeatNumber,
                    IsReserved = s.IsReserved,
                }).ToList()
            };
        }

        // ... (UpdateFlightStatusAsync method from before, ensure it uses _agentNotifier) ...
        public async Task<bool> UpdateFlightStatusAsync(int flightId, FlightStatus newStatus)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(flightId);
            if (flight == null)
            {
                return false;
            }

            flight.Status = newStatus;

            try
            {
                await _context.SaveChangesAsync();
                await _agentNotifier.NotifyFlightStatusChangedAsync(flight.Id, flight.Status, flight.FlightNumber);
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error in UpdateFlightStatusAsync: {ex.Message}"); // Log ex
                return false;
            }
        }
    }
}