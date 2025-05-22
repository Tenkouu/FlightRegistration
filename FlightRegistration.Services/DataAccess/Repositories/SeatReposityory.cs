using FlightRegistration.Core.Models;
using FlightRegistration.Services.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightRegistration.Services.DataAccess.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;

        public SeatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Seat?> GetSeatByIdAsync(int seatId)
        {
            // Also include flight to check if the seat belongs to the correct flight if needed,
            // or to check if the seat is already reserved.
            return await _context.Seats
                                 .Include(s => s.Flight) // Useful for validation
                                 .FirstOrDefaultAsync(s => s.Id == seatId);
        }

        public async Task<IEnumerable<Seat>> GetSeatsByFlightIdAsync(int flightId)
        {
            return await _context.Seats
                                 .Where(s => s.FlightId == flightId)
                                 .ToListAsync();
        }
    }
}