namespace JobTrackerAPI.Models.Entities;

/// <summary>
/// Represents a single job application submitted by a user.
/// </summary>
public class JobApplication
{
    public int Id { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty; // Denormalized for quick display
    public string? JobLocation { get; set; }
    public string? JobUrl { get; set; }
    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public int UserId { get; set; }
    public int? CompanyId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Company? Company { get; set; }
    public ICollection<InterviewRound> InterviewRounds { get; set; } = new List<InterviewRound>();
}
