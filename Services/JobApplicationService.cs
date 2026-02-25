using AutoMapper;
using JobTrackerAPI.Models.DTOs.JobApplication;
using JobTrackerAPI.Models.Entities;
using JobTrackerAPI.Repositories.Interfaces;
using JobTrackerAPI.Services.Interfaces;

namespace JobTrackerAPI.Services;

public class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _applicationRepository;
    private readonly IMapper _mapper;

    public JobApplicationService(IJobApplicationRepository applicationRepository, IMapper mapper)
    {
        _applicationRepository = applicationRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<JobApplicationResponseDto>> GetAllAsync(int userId, JobApplicationQueryParams queryParams)
    {
        var pagedResult = await _applicationRepository.GetPagedByUserAsync(userId, queryParams);

        return new PagedResult<JobApplicationResponseDto>
        {
            Items = _mapper.Map<IEnumerable<JobApplicationResponseDto>>(pagedResult.Items),
            TotalCount = pagedResult.TotalCount,
            Page = pagedResult.Page,
            PageSize = pagedResult.PageSize
        };
    }

    public async Task<JobApplicationResponseDto> GetByIdAsync(int id, int userId)
    {
        var application = await _applicationRepository.GetByIdWithDetailsAsync(id, userId)
            ?? throw new KeyNotFoundException($"Job application with ID {id} was not found.");

        return _mapper.Map<JobApplicationResponseDto>(application);
    }

    public async Task<JobApplicationResponseDto> CreateAsync(int userId, CreateJobApplicationDto dto)
    {
        var application = _mapper.Map<JobApplication>(dto);
        application.UserId = userId;
        application.CreatedAt = DateTime.UtcNow;
        application.UpdatedAt = DateTime.UtcNow;

        var created = await _applicationRepository.AddAsync(application);
        return _mapper.Map<JobApplicationResponseDto>(created);
    }

    public async Task<JobApplicationResponseDto> UpdateAsync(int id, int userId, UpdateJobApplicationDto dto)
    {
        var application = await _applicationRepository.GetByIdWithDetailsAsync(id, userId)
            ?? throw new KeyNotFoundException($"Job application with ID {id} was not found.");

        // Patch only provided fields
        if (dto.JobTitle is not null) application.JobTitle = dto.JobTitle;
        if (dto.CompanyName is not null) application.CompanyName = dto.CompanyName;
        if (dto.JobLocation is not null) application.JobLocation = dto.JobLocation;
        if (dto.JobUrl is not null) application.JobUrl = dto.JobUrl;
        if (dto.ApplicationDate.HasValue) application.ApplicationDate = dto.ApplicationDate.Value;
        if (dto.Status.HasValue) application.Status = dto.Status.Value;
        if (dto.Notes is not null) application.Notes = dto.Notes;
        if (dto.CompanyId.HasValue) application.CompanyId = dto.CompanyId;
        application.UpdatedAt = DateTime.UtcNow;

        var updated = await _applicationRepository.UpdateAsync(application);
        return _mapper.Map<JobApplicationResponseDto>(updated);
    }

    public async Task DeleteAsync(int id, int userId)
    {
        var application = await _applicationRepository.GetByIdWithDetailsAsync(id, userId)
            ?? throw new KeyNotFoundException($"Job application with ID {id} was not found.");

        await _applicationRepository.DeleteAsync(application);
    }
}
