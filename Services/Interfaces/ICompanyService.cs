using JobTrackerAPI.Models.DTOs.Company;

namespace JobTrackerAPI.Services.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<CompanyResponseDto>> GetAllAsync(string? searchTerm);
    Task<CompanyResponseDto> GetByIdAsync(int id);
    Task<CompanyResponseDto> CreateAsync(CreateCompanyDto dto);
    Task<CompanyResponseDto> UpdateAsync(int id, UpdateCompanyDto dto);
    Task DeleteAsync(int id);
}
