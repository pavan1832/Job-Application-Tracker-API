using JobTrackerAPI.Extensions;
using JobTrackerAPI.Models.DTOs.Interview;
using JobTrackerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackerAPI.Controllers;

/// <summary>
/// Manages interview rounds nested under a specific job application.
/// Route: /api/jobapplications/{applicationId}/interviews
/// </summary>
[ApiController]
[Route("api/jobapplications/{applicationId:int}/interviews")]
[Authorize]
[Produces("application/json")]
public class InterviewRoundsController : ControllerBase
{
    private readonly IInterviewRoundService _interviewService;

    public InterviewRoundsController(IInterviewRoundService interviewService)
    {
        _interviewService = interviewService;
    }

    /// <summary>
    /// Get all interview rounds for a specific job application.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InterviewRoundResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(int applicationId)
    {
        var userId = User.GetUserId();
        var rounds = await _interviewService.GetByApplicationAsync(applicationId, userId);
        return Ok(rounds);
    }

    /// <summary>
    /// Get a single interview round by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InterviewRoundResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int applicationId, int id)
    {
        var userId = User.GetUserId();
        var round = await _interviewService.GetByIdAsync(id, applicationId, userId);
        return Ok(round);
    }

    /// <summary>
    /// Add a new interview round to a job application.
    /// Automatically updates the application status to 'Interviewing' if it was 'Applied'.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(InterviewRoundResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(int applicationId, [FromBody] CreateInterviewRoundDto dto)
    {
        var userId = User.GetUserId();
        var round = await _interviewService.CreateAsync(applicationId, userId, dto);
        return CreatedAtAction(nameof(GetById), new { applicationId, id = round.Id }, round);
    }

    /// <summary>
    /// Update an existing interview round.
    /// </summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(InterviewRoundResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int applicationId, int id, [FromBody] UpdateInterviewRoundDto dto)
    {
        var userId = User.GetUserId();
        var round = await _interviewService.UpdateAsync(id, applicationId, userId, dto);
        return Ok(round);
    }

    /// <summary>
    /// Delete an interview round.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int applicationId, int id)
    {
        var userId = User.GetUserId();
        await _interviewService.DeleteAsync(id, applicationId, userId);
        return NoContent();
    }
}
