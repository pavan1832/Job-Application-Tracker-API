using JobTrackerAPI.Data;
using JobTrackerAPI.Models.DTOs.JobApplication;
using JobTrackerAPI.Models.Entities;
using JobTrackerAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobTrackerAPI.Repositories;

public class JobApplicationRepository : Repository<JobApplication>, IJobApplicationRepository
{
    public JobApplicationRepository(ApplicationDbContext context) : base(context) { }

    /// <summary>
    /// Returns a paginated, filtered, and sorted list of job applications for a specific user.
    /// </summary>
    public async Task<PagedResult<JobApplication>> GetPagedByUserAsync(int userId, JobApplicationQueryParams queryParams)
    {
        var query = _context.JobApplications
            .Where(j => j.UserId == userId)
            .AsQueryable();

        // Filter by status
        if (queryParams.Status.HasValue)
            query = query.Where(j => j.Status == queryParams.Status.Value);

        // Search by job title or company name
        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
        {
            var term = queryParams.SearchTerm.ToLower();
            query = query.Where(j =>
                j.JobTitle.ToLower().Contains(term) ||
                j.CompanyName.ToLower().Contains(term) ||
                (j.JobLocation != null && j.JobLocation.ToLower().Contains(term)));
        }

        // Sorting
        query = queryParams.SortBy?.ToLower() switch
        {
            "jobtitle"     => queryParams.SortDescending ? query.OrderByDescending(j => j.JobTitle)     : query.OrderBy(j => j.JobTitle),
            "companyname"  => queryParams.SortDescending ? query.OrderByDescending(j => j.CompanyName)  : query.OrderBy(j => j.CompanyName),
            "status"       => queryParams.SortDescending ? query.OrderByDescending(j => j.Status)       : query.OrderBy(j => j.Status),
            _              => queryParams.SortDescending ? query.OrderByDescending(j => j.ApplicationDate) : query.OrderBy(j => j.ApplicationDate)
        };

        var totalCount = await query.CountAsync();

        var items = await query
            .Include(j => j.InterviewRounds)
            .Skip((queryParams.Page - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync();

        return new PagedResult<JobApplication>
        {
            Items = items,
            TotalCount = totalCount,
            Page = queryParams.Page,
            PageSize = queryParams.PageSize
        };
    }

    /// <summary>
    /// Returns a single application with all details, ensuring it belongs to the given user.
    /// </summary>
    public async Task<JobApplication?> GetByIdWithDetailsAsync(int id, int userId) =>
        await _context.JobApplications
            .Include(j => j.Company)
            .Include(j => j.InterviewRounds)
            .FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);

    public async Task<bool> BelongsToUserAsync(int applicationId, int userId) =>
        await _context.JobApplications
            .AnyAsync(j => j.Id == applicationId && j.UserId == userId);
}
