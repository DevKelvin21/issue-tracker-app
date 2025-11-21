using AutoMapper;
using IssueTracker.Application.DTOs;
using IssueTracker.Core.Entities;
using IssueTracker.Core.Enums;

namespace IssueTracker.Application.Mappings;

/// <summary>
/// AutoMapper profile for entity to DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Issue entity to IssueDto
        CreateMap<Issue, IssueDto>();

        // CreateIssueDto to Issue entity
        CreateMap<CreateIssueDto, Issue>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => IssueStatus.Open))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ResolvedAt, opt => opt.Ignore());

        // UpdateIssueDto to Issue entity (for updates)
        CreateMap<UpdateIssueDto, Issue>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ResolvedAt, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
