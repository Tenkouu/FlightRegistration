namespace FlightRegistration.Core.DTOs
{
    public class SeatAssignmentRequestDto
    {
        public int BookingId { get; set; }
        public string SeatNumber { get; set; } // e.g., "3A"
        public int FlightId { get; set; } // Important for identifying the correct seat map
    }
}