using FlightRegistration.Core.DTOs; // For FlightDetailsDto and FlightStatusUpdateRequestDto
using FlightRegistration.Core.Models; // For FlightStatus enum
using FlightRegistration.Services.BusinessLogic.Interfaces; // For IFlightService
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightRegistration.Server.Controllers
{
    [Route("api/[controller]")] // Sets the base route to "api/flights"
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;
        // You might add ILogger later for logging
        // private readonly ILogger<FlightsController> _logger;

        public FlightsController(IFlightService flightService /*, ILogger<FlightsController> logger */)
        {
            _flightService = flightService;
            // _logger = logger;
        }

        // GET: api/flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightDetailsDto>>> GetAllFlights()
        {
            // _logger.LogInformation("Getting all flights");
            var flights = await _flightService.GetAllFlightsAsync();
            return Ok(flights);
        }

        // GET: api/flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightDetailsDto>> GetFlightById(int id)
        {
            // _logger.LogInformation($"Getting flight with ID: {id}");
            var flight = await _flightService.GetFlightDetailsByIdAsync(id);

            if (flight == null)
            {
                // _logger.LogWarning($"Flight with ID: {id} not found.");
                return NotFound(); // Returns HTTP 404
            }

            return Ok(flight); // Returns HTTP 200 with the flight data
        }

        // PUT: api/flights/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateFlightStatus(int id, [FromBody] FlightStatusUpdateRequestDto statusUpdateDto)
        {
            if (id != statusUpdateDto.FlightId)
            {
                // _logger.LogWarning("Mismatched flight ID in route and body for status update.");
                return BadRequest("Flight ID in route must match Flight ID in request body.");
            }

            // _logger.LogInformation($"Updating status for flight ID: {id} to {statusUpdateDto.NewStatus}");
            var success = await _flightService.UpdateFlightStatusAsync(id, statusUpdateDto.NewStatus);

            if (!success)
            {
                // This could be because the flight wasn't found, or a DB update failed.
                // The service should ideally differentiate, or you can log more details in the service.
                // _logger.LogError($"Failed to update status for flight ID: {id}. Flight might not exist or DB error.");
                return NotFound($"Flight with ID {id} not found or update failed."); // Or a more generic 500 if it's a DB error
            }

            return NoContent(); // Returns HTTP 204 (Success, no content to return)
                                // Alternatively, you could return Ok(updatedFlightDto) if your service returned it.
        }

        // You might add POST for creating new flights, DELETE for removing flights, etc.,
        // if those are part of the requirements.
    }
}