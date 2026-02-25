namespace JobTrackerAPI.Models.Entities;

/// <summary>
/// Represents the current status of a job application.
/// </summary>
public enum ApplicationStatus
{
    Applied = 0,
    Interviewing = 1,
    Offer = 2,
    Rejected = 3,
    Withdrawn = 4,
    Ghosted = 5
}
