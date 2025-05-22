namespace FlightRegistration.Core.DTOs
{
    public class SeatDto
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public bool IsReserved { get; set; }
        public string ReservedForPassengerName { get; set; } // Could be useful for UI
    }
}