// File: FlightRegistration.Services/BusinessLogic/Implementations/CheckInService.cs
using FlightRegistration.Core.DTOs;
using FlightRegistration.Core.Models;
using FlightRegistration.Services.BusinessLogic.Interfaces;
using FlightRegistration.Services.DataAccess;
using FlightRegistration.Services.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightRegistration.Core.Interfaces; // <--- ENSURE THIS USING IS CORRECT

namespace FlightRegistration.Services.BusinessLogic.Implementations
{
    public class CheckInService : ICheckInService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly AppDbContext _context;
        private readonly IAgentNotifier _agentNotifier; // Dependency on the interface

        public CheckInService(
            IPassengerRepository passengerRepository,
            IBookingRepository bookingRepository,
            ISeatRepository seatRepository,
            IFlightRepository flightRepository,
            AppDbContext context,
            IAgentNotifier agentNotifier) // Injected here
        {
            _passengerRepository = passengerRepository;
            _bookingRepository = bookingRepository;
            _seatRepository = seatRepository;
            _flightRepository = flightRepository;
            _context = context;
            _agentNotifier = agentNotifier; // Assigned here
        }

        // ... (rest of SearchPassengerBookingsAsync method from before) ...
        public async Task<IEnumerable<PassengerBookingDetailsDto>> SearchPassengerBookingsAsync(PassengerSearchRequestDto searchRequest)
        {
            var passenger = await _passengerRepository.GetByPassportNumberAsync(searchRequest.PassportNumber);
            if (passenger == null)
            {
                return Enumerable.Empty<PassengerBookingDetailsDto>();
            }

            var flight = await _context.Flights
                                     .FirstOrDefaultAsync(f => f.FlightNumber == searchRequest.FlightNumber);
            if (flight == null)
            {
                return Enumerable.Empty<PassengerBookingDetailsDto>();
            }

            var bookings = await _bookingRepository.GetBookingsByPassengerAndFlightAsync(passenger.Id, flight.Id);

            return bookings.Select(b => new PassengerBookingDetailsDto
            {
                BookingId = b.Id,
                PassengerName = $"{b.Passenger.FirstName} {b.Passenger.LastName}",
                PassportNumber = b.Passenger.PassportNumber,
                FlightNumber = b.Flight.FlightNumber,
                DepartureTime = b.Flight.DepartureTime,
                CurrentSeatNumber = b.AssignedSeat?.SeatNumber,
                FlightId = b.FlightId,
                FlightStatus = b.Flight.Status
            }).ToList();
        }


        // ... (AssignSeatAsync method from before, ensure it uses _agentNotifier) ...
        public async Task<(bool Success, string Message, BoardingPassDto? BoardingPass)> AssignSeatAsync(SeatAssignmentRequestDto seatAssignmentRequest)
        {
            string agentId = "Agent_REST_API"; // Placeholder

            var booking = await _bookingRepository.GetBookingWithDetailsAsync(seatAssignmentRequest.BookingId);
            if (booking == null)
            {
                return (false, "Booking not found.", null);
            }

            if (booking.AssignedSeatId.HasValue)
            {
                return (false, $"Passenger {booking.Passenger.FirstName} {booking.Passenger.LastName} already has seat {booking.AssignedSeat?.SeatNumber} assigned.", null);
            }

            if (booking.Flight.Status != FlightStatus.CheckingIn && booking.Flight.Status != FlightStatus.Scheduled)
            {
                return (false, $"Cannot assign seat. Flight {booking.Flight.FlightNumber} status is {booking.Flight.Status}.", null);
            }

            var seatToAssign = await _seatRepository.GetSeatByIdAsync(seatAssignmentRequest.SeatId);
            if (seatToAssign == null)
            {
                return (false, "Selected seat not found.", null);
            }

            if (seatToAssign.FlightId != booking.FlightId)
            {
                return (false, "Selected seat does not belong to this flight.", null);
            }

            if (seatToAssign.IsReserved)
            {
                return (false, $"Seat {seatToAssign.SeatNumber} is already reserved.", null);
            }

            booking.AssignedSeatId = seatToAssign.Id;
            booking.AssignedSeat = seatToAssign;
            seatToAssign.IsReserved = true;
            seatToAssign.ReservedByBooking = booking;

            try
            {
                await _context.SaveChangesAsync();

                var boardingTime = booking.Flight.DepartureTime.AddMinutes(-45);
                var boardingPass = new BoardingPassDto
                {
                    PassengerName = $"{booking.Passenger.FirstName} {booking.Passenger.LastName}",
                    FlightNumber = booking.Flight.FlightNumber,
                    DepartureCity = booking.Flight.DepartureCity,
                    ArrivalCity = booking.Flight.ArrivalCity,
                    DepartureTime = booking.Flight.DepartureTime,
                    SeatNumber = seatToAssign.SeatNumber,
                    BoardingTime = boardingTime
                };

                await _agentNotifier.NotifySeatReservedAsync(booking.FlightId, seatToAssign.Id, seatToAssign.SeatNumber, agentId);

                return (true, $"Seat {seatToAssign.SeatNumber} assigned successfully to {booking.Passenger.FirstName} {booking.Passenger.LastName}.", boardingPass);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"DbUpdateConcurrencyException: {ex.Message}"); // Log ex
                await _agentNotifier.NotifySeatReservationFailedAsync(booking.FlightId, seatToAssign.Id, seatToAssign.SeatNumber, agentId, "Concurrency conflict - seat taken");
                return (false, "Failed to assign seat due to a conflict (seat may have just been taken). Please try another seat.", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}"); // Log ex
                return (false, $"An error occurred while assigning the seat: {ex.Message}", null);
            }
        }
    }
}