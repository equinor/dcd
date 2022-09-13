using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class CommonLibraryProjectDtoAdapter
{
    public static CommonLibraryProjectDto? Convert(dynamic project)
    {
        var projectDto = new CommonLibraryProjectDto();
        if (!Guid.TryParse(project.GUID.ToString(), out Guid guid))
        {
            return null;
        }

        projectDto.ProjectState = project.ProjectState;
        projectDto.Id = guid;
        projectDto.Name = project.Name;
        projectDto.Description = project.Description;
        projectDto.Country = project.Country;
        projectDto.ProjectCategory = ConvertCategory(project.ProjectCategory.ToString());
        projectDto.ProjectPhase = ConvertPhase(project.Phase.ToString());

        return projectDto;
    }

    internal static ProjectCategory ConvertCategory(string category)
    {
        return category switch
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
            _ => throw new Exception(String.Format("Category {0} does not exist in DCD.", category)),
        };
    }

    internal static ProjectPhase ConvertPhase(string phase)
    {
        return phase switch
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
            _ => throw new Exception(String.Format("Phase {0} does not exist in DCD.", phase)),
        };
    }
}
