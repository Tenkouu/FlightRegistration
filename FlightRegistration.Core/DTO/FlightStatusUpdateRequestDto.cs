using FlightRegistration.Core.Models;

namespace FlightRegistration.Core.DTOs
{
    public class FlightStatusUpdateRequestDto
    {
        public int FlightId { get; set; }
        public FlightStatus NewStatus { get; set; } // Using the enum
    }
}