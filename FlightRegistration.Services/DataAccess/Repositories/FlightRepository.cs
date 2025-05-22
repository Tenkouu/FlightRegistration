using FlightRegistration.Core.Models;
using FlightRegistration.Services.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightRegistration.Services.DataAccess.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly AppDbContext _context;

        public FlightRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Flight?> GetFlightByIdAsync(int flightId)
        {
            return await _context.Flights
                                 .Include(f => f.Seats) // Example of eager loading seats
                                 .FirstOrDefaultAsync(f => f.Id == flightId);
        }

        public async Task<IEnumerable<Flight>> GetAllFlightsAsync()
        {
            return await _context.Flights
                                 .Include(f => f.Seats)
                                 .ToListAsync();
        }

        public async Task AddFlightAsync(Flight flight)
        {
            await _context.Flights.AddAsync(flight);
            // Note: SaveChangesAsync is typically called by a Unit of Work or a service layer method
            // that coordinates changes across multiple repositories. For now, we'll assume it's called elsewhere.
        }

        public async Task UpdateFlightAsync(Flight flight)
        {
            _context.Flights.Update(flight);
            // _context.Entry(flight).State = EntityState.Modified; // Another way
        }

        // Don't forget SaveChangesAsync!
        // It's often better to have a generic repository or a Unit of Work pattern
        // to handle SaveChangesAsync centrally. For now, individual repositories
        // won't call it directly after each operation to allow for transactional operations
        // at the service layer. A service method might call multiple repository methods
        // and then call a single SaveChangesAsync.
    }
}