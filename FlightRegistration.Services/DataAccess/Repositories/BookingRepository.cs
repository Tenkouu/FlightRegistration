using FlightRegistration.Core.Models;
using FlightRegistration.Services.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightRegistration.Services.DataAccess.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings.FindAsync(bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByPassengerAndFlightAsync(int passengerId, int flightId)
        {
            return await _context.Bookings
                                 .Where(b => b.PassengerId == passengerId && b.FlightId == flightId)
                                 .Include(b => b.Flight)
                                 .Include(b => b.Passenger)
                                 .Include(b => b.AssignedSeat) // Include assigned seat details
                                 .ToListAsync();
        }
        public async Task<Booking?> GetBookingWithDetailsAsync(int bookingId)
        {
            return await _context.Bookings
                                 .Include(b => b.Flight)
                                 .Include(b => b.Passenger)
                                 .Include(b => b.AssignedSeat)
                                 .FirstOrDefaultAsync(b => b.Id == bookingId);
        }
    }
}