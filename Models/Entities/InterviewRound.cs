namespace JobTrackerAPI.Models.Entities;

/// <summary>
/// Represents the type of interview round.
/// </summary>
public enum InterviewType
{
    HR = 0,
    Technical = 1,
    Managerial = 2,
    Cultural = 3,
    Final = 4,
    Other = 5
}

/// <summary>
/// Represents the result of an interview round.
/// </summary>
public enum InterviewResult
{
    Pending = 0,
    Passed = 1,
    Failed = 2,
    Cancelled = 3
}

/// <summary>
/// Represents a single interview round linked to a job application.
/// </summary>
public class InterviewRound
{
    public int Id { get; set; }
    public DateTime InterviewDate { get; set; }
    public InterviewType InterviewType { get; set; } = InterviewType.HR;
    public InterviewResult Result { get; set; } = InterviewResult.Pending;
    public string? Interviewer { get; set; }
    public string? Feedback { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key
    public int JobApplicationId { get; set; }

    // Navigation
    public JobApplication JobApplication { get; set; } = null!;
}
