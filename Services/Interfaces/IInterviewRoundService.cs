using JobTrackerAPI.Models.DTOs.Interview;

namespace JobTrackerAPI.Services.Interfaces;

public interface IInterviewRoundService
{
    Task<IEnumerable<InterviewRoundResponseDto>> GetByApplicationAsync(int applicationId, int userId);
    Task<InterviewRoundResponseDto> GetByIdAsync(int id, int applicationId, int userId);
    Task<InterviewRoundResponseDto> CreateAsync(int applicationId, int userId, CreateInterviewRoundDto dto);
    Task<InterviewRoundResponseDto> UpdateAsync(int id, int applicationId, int userId, UpdateInterviewRoundDto dto);
    Task DeleteAsync(int id, int applicationId, int userId);
}
