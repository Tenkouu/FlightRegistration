using FlightRegistration.Core.DTOs;
using FlightRegistration.Core.Models;
using FlightRegistration.Services.BusinessLogic.Interfaces;
using FlightRegistration.Services.DataAccess; // For AppDbContext
using FlightRegistration.Services.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore; // For ToListAsync, FirstOrDefaultAsync etc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightRegistration.Services.BusinessLogic.Implementations
{
    public class CheckInService : ICheckInService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IFlightRepository _flightRepository; // To get flight details for search and validation
        private readonly AppDbContext _context; // For SaveChangesAsync and transactions

        public CheckInService(
            IPassengerRepository passengerRepository,
            IBookingRepository bookingRepository,
            ISeatRepository seatRepository,
            IFlightRepository flightRepository,
            AppDbContext context)
        {
            _passengerRepository = passengerRepository;
            _bookingRepository = bookingRepository;
            _seatRepository = seatRepository;
            _flightRepository = flightRepository;
            _context = context;
        }

        public async Task<IEnumerable<PassengerBookingDetailsDto>> SearchPassengerBookingsAsync(PassengerSearchRequestDto searchRequest)
        {
            var passenger = await _passengerRepository.GetByPassportNumberAsync(searchRequest.PassportNumber);
            if (passenger == null)
            {
                return Enumerable.Empty<PassengerBookingDetailsDto>(); // Or throw a specific NotFoundException
            }

            var flight = await _context.Flights // Using context directly for simple query
                                     .FirstOrDefaultAsync(f => f.FlightNumber == searchRequest.FlightNumber);
            if (flight == null)
            {
                return Enumerable.Empty<PassengerBookingDetailsDto>(); // Or throw
            }

            var bookings = await _bookingRepository.GetBookingsByPassengerAndFlightAsync(passenger.Id, flight.Id);

            return bookings.Select(b => new PassengerBookingDetailsDto
            {
                BookingId = b.Id,
                PassengerName = $"{b.Passenger.FirstName} {b.Passenger.LastName}",
                PassportNumber = b.Passenger.PassportNumber,
                FlightNumber = b.Flight.FlightNumber,
                DepartureTime = b.Flight.DepartureTime,
                CurrentSeatNumber = b.AssignedSeat?.SeatNumber, // Safely access SeatNumber
                FlightId = b.FlightId,
                FlightStatus = b.Flight.Status
            }).ToList();
        }

        public async Task<(bool Success, string Message, BoardingPassDto? BoardingPass)> AssignSeatAsync(SeatAssignmentRequestDto seatAssignmentRequest)
        {
            // Use a transaction to ensure atomicity if multiple entities are updated
            // For this specific operation, we update Booking and Seat.
            // EF Core SaveChangesAsync on a single DbContext instance typically runs within a transaction by default
            // if the database provider supports it (SQLite does).
            // For more complex scenarios spanning multiple service calls or contexts, explicit transactions are better.

            var booking = await _bookingRepository.GetBookingWithDetailsAsync(seatAssignmentRequest.BookingId);
            if (booking == null)
            {
                return (false, "Booking not found.", null);
            }

            if (booking.AssignedSeatId.HasValue)
            {
                return (false, $"Passenger {booking.Passenger.FirstName} {booking.Passenger.LastName} already has seat {booking.AssignedSeat?.SeatNumber} assigned.", null);
            }

            // Validate flight status (e.g., can only assign seats if flight is "CheckingIn")
            if (booking.Flight.Status != FlightStatus.CheckingIn && booking.Flight.Status != FlightStatus.Scheduled) // Allow Scheduled for pre-assignment
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

            // CRITICAL SECTION: Check if seat is reserved and assign it
            // This is where concurrency issues can happen if not handled carefully.
            // A database-level lock or optimistic concurrency might be needed for high-traffic systems.
            // For now, we'll do a simple check.
            if (seatToAssign.IsReserved) // Check the flag
            {
                // Double check if it's reserved by another booking (more robust)
                var seatStillReserved = await _context.Seats.AnyAsync(s => s.Id == seatToAssign.Id && s.IsReserved);
                if (seatStillReserved)
                {
                    return (false, $"Seat {seatToAssign.SeatNumber} is already reserved or has just been taken.", null);
                }
            }


            // Assign seat
            booking.AssignedSeatId = seatToAssign.Id;
            booking.AssignedSeat = seatToAssign; // Link the navigation property
            seatToAssign.IsReserved = true;
            // seatToAssign.ReservedByBookingId = booking.Id; // We removed this FK from Seat model
            seatToAssign.ReservedByBooking = booking; // Link inverse navigation

            try
            {
                await _context.SaveChangesAsync(); // This saves changes to Booking and Seat

                // Prepare Boarding Pass Data
                var boardingTime = booking.Flight.DepartureTime.AddMinutes(-45); // Example: boarding 45 mins before departure
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

                // Here, you would ideally trigger a real-time update to other agents/displays
                // For now, we just return success.
                return (true, $"Seat {seatToAssign.SeatNumber} assigned successfully to {booking.Passenger.FirstName} {booking.Passenger.LastName}.", boardingPass);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // This exception can occur if another transaction modified the same seat (IsReserved flag)
                // and saved it between when we read it and when we try to save our changes.
                // This is a form of optimistic concurrency.
                // You'd need to reload the seat and inform the user.
                // For now, a generic message.
                // Log ex
                return (false, "Failed to assign seat due to a conflict. Please try another seat.", null);
            }
            catch (Exception ex)
            {
                // Log ex
                return (false, $"An error occurred while assigning the seat: {ex.Message}", null);
            }
        }
    }
}