using JobTrackerAPI.Models.Entities;

namespace JobTrackerAPI.Repositories.Interfaces;

public interface IInterviewRoundRepository : IRepository<InterviewRound>
{
    Task<IEnumerable<InterviewRound>> GetByApplicationIdAsync(int applicationId);
    Task<InterviewRound?> GetByIdAndApplicationAsync(int id, int applicationId);
}
