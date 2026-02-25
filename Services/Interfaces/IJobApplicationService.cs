using JobTrackerAPI.Models.DTOs.JobApplication;

namespace JobTrackerAPI.Services.Interfaces;

public interface IJobApplicationService
{
    Task<PagedResult<JobApplicationResponseDto>> GetAllAsync(int userId, JobApplicationQueryParams queryParams);
    Task<JobApplicationResponseDto> GetByIdAsync(int id, int userId);
    Task<JobApplicationResponseDto> CreateAsync(int userId, CreateJobApplicationDto dto);
    Task<JobApplicationResponseDto> UpdateAsync(int id, int userId, UpdateJobApplicationDto dto);
    Task DeleteAsync(int id, int userId);
}
