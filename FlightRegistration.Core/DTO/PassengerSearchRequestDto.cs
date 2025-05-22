namespace FlightRegistration.Core.DTOs
{
    public class PassengerSearchRequestDto
    {
        public string PassportNumber { get; set; }
        public string FlightNumber { get; set; } // To find bookings for a specific flight
    }
}