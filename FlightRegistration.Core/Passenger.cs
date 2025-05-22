using System.Collections.Generic; // Add this
namespace FlightRegistration.Core.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string PassportNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}