using System.ComponentModel.DataAnnotations;
using JobTrackerAPI.Models.Entities;

namespace JobTrackerAPI.Models.DTOs.JobApplication;

public class CreateJobApplicationDto
{
    [Required, MaxLength(200)]
    public string JobTitle { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? JobLocation { get; set; }

    [MaxLength(1000)]
    public string? JobUrl { get; set; }

    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

    [MaxLength(5000)]
    public string? Notes { get; set; }

    public int? CompanyId { get; set; }
}

public class UpdateJobApplicationDto
{
    [MaxLength(200)]
    public string? JobTitle { get; set; }

    [MaxLength(200)]
    public string? CompanyName { get; set; }

    [MaxLength(200)]
    public string? JobLocation { get; set; }

    [MaxLength(1000)]
    public string? JobUrl { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public ApplicationStatus? Status { get; set; }

    [MaxLength(5000)]
    public string? Notes { get; set; }

    public int? CompanyId { get; set; }
}

public class JobApplicationResponseDto
{
    public int Id { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string? JobLocation { get; set; }
    public string? JobUrl { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int? CompanyId { get; set; }
    public int InterviewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Query parameters for filtering and paginating job applications.
/// </summary>
public class JobApplicationQueryParams
{
    public ApplicationStatus? Status { get; set; }

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;

    public string? SearchTerm { get; set; }

    public string SortBy { get; set; } = "ApplicationDate";
    public bool SortDescending { get; set; } = true;
}

/// <summary>
/// Paginated result wrapper.
/// </summary>
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}
