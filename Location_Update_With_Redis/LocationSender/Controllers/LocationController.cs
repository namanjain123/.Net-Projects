using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services.IService;

namespace LocationSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IRedisPublishService _redisPublishService;
        private readonly ILogger<LocationController> _logger; // Inject the ILogger interface

        public LocationController(IRedisPublishService redisPublishService, ILogger<LocationController> logger)
        {
            _redisPublishService = redisPublishService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> LocationChange([FromBody] UserLocation locationUser) 
        {
            try
            {
                _logger.LogInformation($"Received location update for user with ID: {locationUser.Id}");
                string result = await _redisPublishService.PublishMessageAsync(locationUser);
                // Log information about the successful update
                _logger.LogInformation($"Location update for user with ID {locationUser.Id} successful");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating location for user with ID: {locationUser.Id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"{locationUser.Id} service update failed");
            }
        }
    }
}
