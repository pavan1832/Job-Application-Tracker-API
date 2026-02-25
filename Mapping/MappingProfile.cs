using AutoMapper;
using JobTrackerAPI.Models.DTOs.Company;
using JobTrackerAPI.Models.DTOs.Interview;
using JobTrackerAPI.Models.DTOs.JobApplication;
using JobTrackerAPI.Models.Entities;

namespace JobTrackerAPI.Mapping;

/// <summary>
/// Configures all AutoMapper mappings between entities and DTOs.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ── Company ───────────────────────────────────────────────────────
        CreateMap<Company, CompanyResponseDto>()
            .ForMember(dest => dest.ApplicationCount,
                       opt => opt.MapFrom(src => src.JobApplications.Count));

        CreateMap<CreateCompanyDto, Company>();
        CreateMap<UpdateCompanyDto, Company>()
            .ForAllMembers(opt => opt.Condition((_, _, srcMember) => srcMember != null));

        // ── Job Application ───────────────────────────────────────────────
        CreateMap<JobApplication, JobApplicationResponseDto>()
            .ForMember(dest => dest.Status,
                       opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.InterviewCount,
                       opt => opt.MapFrom(src => src.InterviewRounds.Count));

        CreateMap<CreateJobApplicationDto, JobApplication>();
        CreateMap<UpdateJobApplicationDto, JobApplication>()
            .ForAllMembers(opt => opt.Condition((_, _, srcMember) => srcMember != null));

        // ── Interview Round ───────────────────────────────────────────────
        CreateMap<InterviewRound, InterviewRoundResponseDto>()
            .ForMember(dest => dest.InterviewType,
                       opt => opt.MapFrom(src => src.InterviewType.ToString()))
            .ForMember(dest => dest.Result,
                       opt => opt.MapFrom(src => src.Result.ToString()))
            .ForMember(dest => dest.JobTitle,
                       opt => opt.MapFrom(src => src.JobApplication != null
                           ? src.JobApplication.JobTitle
                           : string.Empty));

        CreateMap<CreateInterviewRoundDto, InterviewRound>();
        CreateMap<UpdateInterviewRoundDto, InterviewRound>()
            .ForAllMembers(opt => opt.Condition((_, _, srcMember) => srcMember != null));
    }
}
