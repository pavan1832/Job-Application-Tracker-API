using System.ComponentModel.DataAnnotations;

namespace JobTrackerAPI.Models.DTOs.Company;

public class CreateCompanyDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Website { get; set; }

    [MaxLength(100)]
    public string? Industry { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }
}

public class UpdateCompanyDto
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(500)]
    public string? Website { get; set; }

    [MaxLength(100)]
    public string? Industry { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }
}

public class CompanyResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Website { get; set; }
    public string? Industry { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public int ApplicationCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
