using FlightRegistration.Core.DTOs;
using FlightRegistration.Services.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq; // For Enumerable.Any()
using System.Threading.Tasks;

namespace FlightRegistration.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInController : ControllerBase
    {
        private readonly ICheckInService _checkInService;
        // private readonly ILogger<CheckInController> _logger; // Optional for logging

        public CheckInController(ICheckInService checkInService /*, ILogger<CheckInController> logger */)
        {
            _checkInService = checkInService;
            // _logger = logger;
        }

        // POST: api/checkin/search-passenger
        [HttpPost("search-passenger")]
        public async Task<IActionResult> SearchPassengerBookings([FromBody] PassengerSearchRequestDto searchRequest)
        {
            if (searchRequest == null || string.IsNullOrWhiteSpace(searchRequest.PassportNumber) || string.IsNullOrWhiteSpace(searchRequest.FlightNumber))
            {
                // _logger.LogWarning("Search passenger request was invalid.");
                return BadRequest("Passport number and flight number are required.");
            }

            // _logger.LogInformation($"Searching for passenger bookings: Passport={searchRequest.PassportNumber}, Flight={searchRequest.FlightNumber}");
            var bookings = await _checkInService.SearchPassengerBookingsAsync(searchRequest);

            if (bookings == null || !bookings.Any())
            {
                // _logger.LogInformation("No bookings found for the given criteria.");
                return NotFound("No bookings found for the specified passenger and flight.");
            }

            return Ok(bookings);
        }

        // POST: api/checkin/assign-seat
        [HttpPost("assign-seat")]
        public async Task<IActionResult> AssignSeat([FromBody] SeatAssignmentRequestDto seatAssignmentRequest)
        {
            if (seatAssignmentRequest == null)
            {
                // _logger.LogWarning("Assign seat request was null.");
                return BadRequest("Seat assignment request is required.");
            }

            // _logger.LogInformation($"Attempting to assign seat for Booking ID: {seatAssignmentRequest.BookingId}, Seat ID: {seatAssignmentRequest.SeatId}");
            var (success, message, boardingPass) = await _checkInService.AssignSeatAsync(seatAssignmentRequest);

            if (!success)
            {
                // _logger.LogWarning($"Seat assignment failed for Booking ID: {seatAssignmentRequest.BookingId}. Message: {message}");
                // Determine appropriate status code based on message, or add more specific error types from service
                if (message.Contains("not found")) // Basic check
                {
                    return NotFound(new { Message = message });
                }
                return BadRequest(new { Message = message }); // For other failures like "already assigned", "status invalid", "already reserved"
            }

            // _logger.LogInformation($"Seat assigned successfully for Booking ID: {seatAssignmentRequest.BookingId}.");
            return Ok(new { Message = message, BoardingPass = boardingPass });
        }
    }
}