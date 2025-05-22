using FlightRegistration.Core.DTOs;
using FlightRegistration.Core.Models;
using FlightRegistration.Services.BusinessLogic.Interfaces;
using FlightRegistration.Services.DataAccess;
using FlightRegistration.Services.DataAccess.Interfaces; // To use IFlightRepository
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightRegistration.Services.BusinessLogic.Implementations
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly AppDbContext _context; // For SaveChangesAsync

        // We'll also need repositories for Seats, Bookings, Passengers later for more complex logic
        // For now, FlightRepository is enough for basic flight operations.

        public FlightService(IFlightRepository flightRepository, AppDbContext context)
        {
            _flightRepository = flightRepository;
            _context = context;
        }

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
                Seats = f.Seats.Select(s => new SeatDto // Assuming Seats are loaded by GetAllFlightsAsync
                {
                    Id = s.Id,
                    SeatNumber = s.SeatNumber,
                    IsReserved = s.IsReserved,
                    // ReservedForPassengerName might require loading Booking -> Passenger
                }).ToList()
            }).ToList();
        }

        public async Task<FlightDetailsDto?> GetFlightDetailsByIdAsync(int flightId)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(flightId); // Ensure GetFlightByIdAsync loads Seats
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

        public async Task<bool> UpdateFlightStatusAsync(int flightId, FlightStatus newStatus)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(flightId);
            if (flight == null)
            {
                return false; // Flight not found
            }

            flight.Status = newStatus;
            // _flightRepository.UpdateFlightAsync(flight); // The repository marks it as modified
            // No need to call UpdateFlightAsync if GetFlightByIdAsync returns a tracked entity
            // and you modify its properties. EF Core tracks changes.
            try
            {
                await _context.SaveChangesAsync(); // Save changes to the database
                return true;
            }
            catch (System.Exception ex)
            {
                // Log the exception (ex)
                return false;
            }
        }
    }
}