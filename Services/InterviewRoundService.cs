using AutoMapper;
using JobTrackerAPI.Models.DTOs.Interview;
using JobTrackerAPI.Models.Entities;
using JobTrackerAPI.Repositories.Interfaces;
using JobTrackerAPI.Services.Interfaces;

namespace JobTrackerAPI.Services;

public class InterviewRoundService : IInterviewRoundService
{
    private readonly IInterviewRoundRepository _interviewRepository;
    private readonly IJobApplicationRepository _applicationRepository;
    private readonly IMapper _mapper;

    public InterviewRoundService(
        IInterviewRoundRepository interviewRepository,
        IJobApplicationRepository applicationRepository,
        IMapper mapper)
    {
        _interviewRepository = interviewRepository;
        _applicationRepository = applicationRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InterviewRoundResponseDto>> GetByApplicationAsync(int applicationId, int userId)
    {
        await EnsureApplicationOwnershipAsync(applicationId, userId);
        var rounds = await _interviewRepository.GetByApplicationIdAsync(applicationId);
        return _mapper.Map<IEnumerable<InterviewRoundResponseDto>>(rounds);
    }

    public async Task<InterviewRoundResponseDto> GetByIdAsync(int id, int applicationId, int userId)
    {
        await EnsureApplicationOwnershipAsync(applicationId, userId);

        var round = await _interviewRepository.GetByIdAndApplicationAsync(id, applicationId)
            ?? throw new KeyNotFoundException($"Interview round with ID {id} was not found.");

        return _mapper.Map<InterviewRoundResponseDto>(round);
    }

    public async Task<InterviewRoundResponseDto> CreateAsync(int applicationId, int userId, CreateInterviewRoundDto dto)
    {
        var application = await EnsureApplicationOwnershipAsync(applicationId, userId);

        var round = _mapper.Map<InterviewRound>(dto);
        round.JobApplicationId = applicationId;
        round.Result = InterviewResult.Pending;
        round.CreatedAt = DateTime.UtcNow;
        round.UpdatedAt = DateTime.UtcNow;

        // Automatically move application status to Interviewing
        if (application.Status == ApplicationStatus.Applied)
        {
            application.Status = ApplicationStatus.Interviewing;
            application.UpdatedAt = DateTime.UtcNow;
            await _applicationRepository.UpdateAsync(application);
        }

        var created = await _interviewRepository.AddAsync(round);
        return _mapper.Map<InterviewRoundResponseDto>(created);
    }

    public async Task<InterviewRoundResponseDto> UpdateAsync(int id, int applicationId, int userId, UpdateInterviewRoundDto dto)
    {
        await EnsureApplicationOwnershipAsync(applicationId, userId);

        var round = await _interviewRepository.GetByIdAndApplicationAsync(id, applicationId)
            ?? throw new KeyNotFoundException($"Interview round with ID {id} was not found.");

        if (dto.InterviewDate.HasValue) round.InterviewDate = dto.InterviewDate.Value;
        if (dto.InterviewType.HasValue) round.InterviewType = dto.InterviewType.Value;
        if (dto.Result.HasValue) round.Result = dto.Result.Value;
        if (dto.Interviewer is not null) round.Interviewer = dto.Interviewer;
        if (dto.Feedback is not null) round.Feedback = dto.Feedback;
        if (dto.Notes is not null) round.Notes = dto.Notes;
        round.UpdatedAt = DateTime.UtcNow;

        var updated = await _interviewRepository.UpdateAsync(round);
        return _mapper.Map<InterviewRoundResponseDto>(updated);
    }

    public async Task DeleteAsync(int id, int applicationId, int userId)
    {
        await EnsureApplicationOwnershipAsync(applicationId, userId);

        var round = await _interviewRepository.GetByIdAndApplicationAsync(id, applicationId)
            ?? throw new KeyNotFoundException($"Interview round with ID {id} was not found.");

        await _interviewRepository.DeleteAsync(round);
    }

    /// <summary>
    /// Verifies that the job application exists and belongs to the current user.
    /// Throws appropriate exceptions otherwise.
    /// </summary>
    private async Task<JobApplication> EnsureApplicationOwnershipAsync(int applicationId, int userId)
    {
        // Use a direct fetch — if not found for this user, it's either missing or forbidden
        var application = await _applicationRepository.GetByIdWithDetailsAsync(applicationId, userId)
            ?? throw new KeyNotFoundException($"Job application with ID {applicationId} was not found.");

        return application;
    }
}
