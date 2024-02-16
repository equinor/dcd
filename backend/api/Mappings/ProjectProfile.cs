using api.Dtos;
using api.Models;

using Api.Services.FusionIntegration.Models;

using AutoMapper;

namespace api.Mappings;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectDto>();
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();

        CreateMap<FusionProjectMaster, Project>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Description)
            )
            .ForMember(
                dest => dest.CommonLibraryName,
                opt => opt.MapFrom(src => src.Description)
            )
            .ForMember(
                dest => dest.FusionProjectId,
                opt => opt.MapFrom(src => src.Identity)
            )
            .ForMember(
                dest => dest.Currency,
                opt => opt.MapFrom(src => Currency.NOK)
            )
            .ForMember(
                dest => dest.PhysicalUnit,
                opt => opt.MapFrom(src => PhysUnit.SI)
            )
            .ForMember(
                dest => dest.ProjectCategory,
                opt => opt.MapFrom(src => GetProjectCategory(src.ProjectCategory))
            )
            .ForMember(
                dest => dest.ProjectPhase,
                opt => opt.MapFrom(src => GetProjectPhase(src.Phase))
            );
    }

    private static ProjectCategory GetProjectCategory(string? projectCategory)
    {
        return projectCategory switch
        {
            "" => ProjectCategory.Null,
            "BROWNFIELD" => ProjectCategory.Brownfield,
            "CESSATION" => ProjectCategory.Cessation,
            "DRILLING_UPGRADE" => ProjectCategory.DrillingUpgrade,
            "ONSHORE" => ProjectCategory.Onshore,
            "PIPELINE" => ProjectCategory.Pipeline,
            "PLATFORM_FPSO" => ProjectCategory.PlatformFpso,
            "SUBSEA" => ProjectCategory.Subsea,
            "SOLAR" => ProjectCategory.Solar,
            "CO2 STORAGE" => ProjectCategory.Co2Storage,
            "EFUEL" => ProjectCategory.Efuel,
            "NUCLEAR" => ProjectCategory.Nuclear,
            "CO2 CAPTURE" => ProjectCategory.Co2Capture,
            "FPSO" => ProjectCategory.Fpso,
            "HYDROGEN" => ProjectCategory.Hydrogen,
            "HSE" => ProjectCategory.Hse,
            "OFFSHORE_WIND" => ProjectCategory.OffshoreWind,
            "PLATFORM" => ProjectCategory.Platform,
            "POWER_FROM_SHORE" => ProjectCategory.PowerFromShore,
            "TIE-IN" => ProjectCategory.TieIn,
            "RENEWABLE_OTHER" => ProjectCategory.RenewableOther,
            "CCS" => ProjectCategory.Ccs,
            _ => ProjectCategory.Null
        };
    }

    private static ProjectPhase GetProjectPhase(string? projectPhase)
    {
        return projectPhase switch
        {
            "" => ProjectPhase.Null,
            "Bid preparations" => ProjectPhase.BidPreparations,
            "Business identification" => ProjectPhase.BusinessIdentification,
            "Business planning" => ProjectPhase.BusinessPlanning,
            "Concept planning" => ProjectPhase.ConceptPlanning,
            "Concession / Negotiations" => ProjectPhase.ConcessionNegotiations,
            "Definition" => ProjectPhase.Definition,
            "Execution" => ProjectPhase.Execution,
            "Operation" => ProjectPhase.Operation,
            "Screening business opportunities" => ProjectPhase.ScreeningBusinessOpportunities,
            _ => ProjectPhase.Null
        };
    }
}
