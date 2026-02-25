using JobTrackerAPI.Models.Entities;

namespace JobTrackerAPI.Repositories.Interfaces;

public interface ICompanyRepository : IRepository<Company>
{
    Task<IEnumerable<Company>> SearchAsync(string? searchTerm);
    Task<Company?> GetWithApplicationsAsync(int companyId);
}
