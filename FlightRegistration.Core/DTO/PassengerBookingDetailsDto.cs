﻿using FlightRegistration.Core.Models;

namespace FlightRegistration.Core.DTOs
{
    public class PassengerBookingDetailsDto
    {
        public int BookingId { get; set; }
        public string PassengerName { get; set; }
        public string PassportNumber { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public string? CurrentSeatNumber { get; set; } // Null if not yet assigned
        public int FlightId { get; set; } // Useful for fetching seat map later
        public FlightStatus FlightStatus { get; set; }
    }
}