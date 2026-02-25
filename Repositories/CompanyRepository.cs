using JobTrackerAPI.Data;
using JobTrackerAPI.Models.Entities;
using JobTrackerAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobTrackerAPI.Repositories;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Company>> SearchAsync(string? searchTerm)
    {
        var query = _context.Companies.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(c =>
                c.Name.ToLower().Contains(searchTerm) ||
                (c.Industry != null && c.Industry.ToLower().Contains(searchTerm)) ||
                (c.Location != null && c.Location.ToLower().Contains(searchTerm)));
        }

        return await query.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Company?> GetWithApplicationsAsync(int companyId) =>
        await _context.Companies
            .Include(c => c.JobApplications)
            .FirstOrDefaultAsync(c => c.Id == companyId);
}
