using Microsoft.AspNetCore.Mvc;

namespace JobTrackerAPI.Controllers;

/// <summary>
/// Health check endpoint for monitoring and readiness probes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Returns the health status of the API.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() =>
        Ok(new { status = "Healthy", timestamp = DateTime.UtcNow, version = "1.0.0" });
}
