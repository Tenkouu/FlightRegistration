namespace FlightRegistration.Core.DTOs
{
    public class SeatAssignmentRequestDto
    {
        public int BookingId { get; set; }
        public int SeatId { get; set; } // Changed from SeatNumber to SeatId for more direct assignment
        // public int FlightId { get; set; } // FlightId can be derived from BookingId on the server
    }
}