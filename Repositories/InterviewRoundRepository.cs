using JobTrackerAPI.Data;
using JobTrackerAPI.Models.Entities;
using JobTrackerAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobTrackerAPI.Repositories;

public class InterviewRoundRepository : Repository<InterviewRound>, IInterviewRoundRepository
{
    public InterviewRoundRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<InterviewRound>> GetByApplicationIdAsync(int applicationId) =>
        await _context.InterviewRounds
            .Where(i => i.JobApplicationId == applicationId)
            .OrderBy(i => i.InterviewDate)
            .ToListAsync();

    public async Task<InterviewRound?> GetByIdAndApplicationAsync(int id, int applicationId) =>
        await _context.InterviewRounds
            .FirstOrDefaultAsync(i => i.Id == id && i.JobApplicationId == applicationId);
}
