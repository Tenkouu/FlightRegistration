using FlightRegistration.Core.Models;
using System.Threading.Tasks;

namespace FlightRegistration.Services.DataAccess.Interfaces
{
    public interface IPassengerRepository
    {
        Task<Passenger?> GetByPassportNumberAsync(string passportNumber);
        // Add other methods if needed
    }
}