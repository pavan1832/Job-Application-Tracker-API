using JobTrackerAPI.Models.DTOs.JobApplication;
using JobTrackerAPI.Models.Entities;

namespace JobTrackerAPI.Repositories.Interfaces;

public interface IJobApplicationRepository : IRepository<JobApplication>
{
    Task<PagedResult<JobApplication>> GetPagedByUserAsync(int userId, JobApplicationQueryParams queryParams);
    Task<JobApplication?> GetByIdWithDetailsAsync(int id, int userId);
    Task<bool> BelongsToUserAsync(int applicationId, int userId);
}
