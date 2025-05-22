using FlightRegistration.Core.Models;
using FlightRegistration.Services.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FlightRegistration.Services.DataAccess.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly AppDbContext _context;

        public PassengerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Passenger?> GetByPassportNumberAsync(string passportNumber)
        {
            return await _context.Passengers
                                 .FirstOrDefaultAsync(p => p.PassportNumber == passportNumber);
        }
    }
}