using FlightRegistration.Core.Models;

namespace FlightRegistration.Core.DTOs
{
    public class FlightDetailsDto
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public FlightStatus Status { get; set; }
        public List<SeatDto> Seats { get; set; } = new List<SeatDto>();
    }
}