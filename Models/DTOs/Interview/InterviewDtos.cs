using System.ComponentModel.DataAnnotations;
using JobTrackerAPI.Models.Entities;

namespace JobTrackerAPI.Models.DTOs.Interview;

public class CreateInterviewRoundDto
{
    [Required]
    public DateTime InterviewDate { get; set; }

    [Required]
    public InterviewType InterviewType { get; set; } = InterviewType.HR;

    [MaxLength(200)]
    public string? Interviewer { get; set; }

    [MaxLength(3000)]
    public string? Feedback { get; set; }

    [MaxLength(3000)]
    public string? Notes { get; set; }
}

public class UpdateInterviewRoundDto
{
    public DateTime? InterviewDate { get; set; }
    public InterviewType? InterviewType { get; set; }
    public InterviewResult? Result { get; set; }

    [MaxLength(200)]
    public string? Interviewer { get; set; }

    [MaxLength(3000)]
    public string? Feedback { get; set; }

    [MaxLength(3000)]
    public string? Notes { get; set; }
}

public class InterviewRoundResponseDto
{
    public int Id { get; set; }
    public int JobApplicationId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public DateTime InterviewDate { get; set; }
    public string InterviewType { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public string? Interviewer { get; set; }
    public string? Feedback { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
