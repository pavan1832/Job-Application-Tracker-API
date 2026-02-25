using JobTrackerAPI.Models.DTOs.Company;
using JobTrackerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackerAPI.Controllers;

/// <summary>
/// CRUD operations for companies. Admin users can create/update/delete; any user can read.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    /// <summary>
    /// Get all companies, with optional search filtering.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CompanyResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? searchTerm)
    {
        var companies = await _companyService.GetAllAsync(searchTerm);
        return Ok(companies);
    }

    /// <summary>
    /// Get a single company by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CompanyResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var company = await _companyService.GetByIdAsync(id);
        return Ok(company);
    }

    /// <summary>
    /// Create a new company. Admin only.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CompanyResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCompanyDto dto)
    {
        var company = await _companyService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
    }

    /// <summary>
    /// Update an existing company. Admin only.
    /// </summary>
    [HttpPatch("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CompanyResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyDto dto)
    {
        var company = await _companyService.UpdateAsync(id, dto);
        return Ok(company);
    }

    /// <summary>
    /// Delete a company. Admin only.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _companyService.DeleteAsync(id);
        return NoContent();
    }
}
