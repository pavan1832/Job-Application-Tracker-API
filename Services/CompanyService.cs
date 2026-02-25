using AutoMapper;
using JobTrackerAPI.Models.DTOs.Company;
using JobTrackerAPI.Models.Entities;
using JobTrackerAPI.Repositories.Interfaces;
using JobTrackerAPI.Services.Interfaces;

namespace JobTrackerAPI.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync(string? searchTerm)
    {
        var companies = await _companyRepository.SearchAsync(searchTerm);
        return _mapper.Map<IEnumerable<CompanyResponseDto>>(companies);
    }

    public async Task<CompanyResponseDto> GetByIdAsync(int id)
    {
        var company = await _companyRepository.GetWithApplicationsAsync(id)
            ?? throw new KeyNotFoundException($"Company with ID {id} was not found.");

        return _mapper.Map<CompanyResponseDto>(company);
    }

    public async Task<CompanyResponseDto> CreateAsync(CreateCompanyDto dto)
    {
        var company = _mapper.Map<Company>(dto);
        company.CreatedAt = DateTime.UtcNow;
        company.UpdatedAt = DateTime.UtcNow;

        var created = await _companyRepository.AddAsync(company);
        return _mapper.Map<CompanyResponseDto>(created);
    }

    public async Task<CompanyResponseDto> UpdateAsync(int id, UpdateCompanyDto dto)
    {
        var company = await _companyRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Company with ID {id} was not found.");

        // Only apply properties that were explicitly provided
        if (dto.Name is not null) company.Name = dto.Name;
        if (dto.Website is not null) company.Website = dto.Website;
        if (dto.Industry is not null) company.Industry = dto.Industry;
        if (dto.Location is not null) company.Location = dto.Location;
        if (dto.Notes is not null) company.Notes = dto.Notes;
        company.UpdatedAt = DateTime.UtcNow;

        var updated = await _companyRepository.UpdateAsync(company);
        return _mapper.Map<CompanyResponseDto>(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Company with ID {id} was not found.");

        await _companyRepository.DeleteAsync(company);
    }
}
