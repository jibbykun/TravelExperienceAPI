using Microsoft.AspNetCore.Mvc;
using TravelExperienceAPI.Interfaces;
using TravelExperienceAPI.Models;
using TravelExperienceAPI.Models.Requests;

namespace TravelExperienceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromBody] TripRequest request)
        {
            try
            {
                var result = await _tripService.CreateTripAsync(request);
                return CreatedAtAction(nameof(CreateTrip), new { id = result.Trip.TripId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
