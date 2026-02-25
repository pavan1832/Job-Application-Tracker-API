using JobTrackerAPI.Extensions;
using JobTrackerAPI.Models.DTOs.JobApplication;
using JobTrackerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackerAPI.Controllers;

/// <summary>
/// Manages job applications for the authenticated user.
/// All operations are scoped to the currently logged-in user.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationService _applicationService;

    public JobApplicationsController(IJobApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    /// <summary>
    /// Get all job applications for the current user, with optional filtering and pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<JobApplicationResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] JobApplicationQueryParams queryParams)
    {
        var userId = User.GetUserId();
        var result = await _applicationService.GetAllAsync(userId, queryParams);
        return Ok(result);
    }

    /// <summary>
    /// Get a single job application by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(JobApplicationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.GetUserId();
        var application = await _applicationService.GetByIdAsync(id, userId);
        return Ok(application);
    }

    /// <summary>
    /// Create a new job application.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(JobApplicationResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateJobApplicationDto dto)
    {
        var userId = User.GetUserId();
        var application = await _applicationService.CreateAsync(userId, dto);
        return CreatedAtAction(nameof(GetById), new { id = application.Id }, application);
    }

    /// <summary>
    /// Partially update a job application. Only provided fields will be changed.
    /// </summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(JobApplicationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateJobApplicationDto dto)
    {
        var userId = User.GetUserId();
        var application = await _applicationService.UpdateAsync(id, userId, dto);
        return Ok(application);
    }

    /// <summary>
    /// Delete a job application.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.GetUserId();
        await _applicationService.DeleteAsync(id, userId);
        return NoContent();
    }
}
