namespace FlightRegistration.Core.DTOs
{
    public class BoardingPassDto
    {
        public string PassengerName { get; set; }
        public string FlightNumber { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public DateTime DepartureTime { get; set; }
        public string SeatNumber { get; set; }
        public DateTime BoardingTime { get; set; }
    }
}