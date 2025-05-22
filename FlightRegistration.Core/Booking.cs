namespace FlightRegistration.Core.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public DateTime BookingTime { get; set; }
        public int? AssignedSeatId { get; set; }

        public virtual Flight Flight { get; set; }
        public virtual Passenger Passenger { get; set; }
        public virtual Seat? AssignedSeat { get; set; } // A booking might not have an assigned seat yet
    }
}