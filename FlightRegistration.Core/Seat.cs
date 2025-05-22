namespace FlightRegistration.Core.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public string SeatNumber { get; set; }
        public bool IsReserved { get; set; }
        // public int? ReservedByBookingId { get; set; } // <-- REMOVE THIS LINE

        public virtual Flight Flight { get; set; }
        // This navigation property allows you to find the Booking that reserved this seat.
        // EF Core will populate this based on a Booking whose AssignedSeatId points to this Seat.
        public virtual Booking? ReservedByBooking { get; set; }
    }
}