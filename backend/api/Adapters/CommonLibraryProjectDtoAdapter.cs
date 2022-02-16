using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class CommonLibraryProjectDtoAdapter
    {
        public static CommonLibraryProjectDto Convert(dynamic project)
        {
            var projectDto = new CommonLibraryProjectDto();
            projectDto.Name = project.Name;
            projectDto.Description = project.Description;
            projectDto.Country = project.Country;
            projectDto.ProjectCategory = ConvertCategory(project.ProjectCategory.ToString());
            projectDto.ProjectPhase = ConvertPhase(project.Phase.ToString());

            return projectDto;
        }

        private static ProjectCategory ConvertCategory(string category)
        {
            switch (category)
            {
                case "":
                    return ProjectCategory.Null;
                case "BROWNFIELD":
                    return ProjectCategory.Brownfield;
                case "CESSATION":
                    return ProjectCategory.Cessation;
                case "DRILLING_UPGRADE":
                    return ProjectCategory.DrillingUpgrade;
                case "ONSHORE":
                    return ProjectCategory.Onshore;
                case "PIPELINE":
                    return ProjectCategory.Pipeline;
                case "PLATFORM_FPSO":
                    return ProjectCategory.PlatformFpso;
                case "SUBSEA":
                    return ProjectCategory.Subsea;
                case "SOLAR":
                    return ProjectCategory.Solar;
                case "CO2 STORAGE":
                    return ProjectCategory.Co2Storage;
                case "EFUEL":
                    return ProjectCategory.Efuel;
                case "NUCLEAR":
                    return ProjectCategory.Nuclear;
                case "CO2 CAPTURE":
                    return ProjectCategory.Co2Capture;
                case "FPSO":
                    return ProjectCategory.Fpso;
                case "HYDROGEN":
                    return ProjectCategory.Hydrogen;
                case "HSE":
                    return ProjectCategory.Hse;
                case "OFFSHORE_WIND":
                    return ProjectCategory.OffshoreWind;
                case "PLATFORM":
                    return ProjectCategory.Platform;
                case "POWER_FROM_SHORE":
                    return ProjectCategory.PowerFromShore;
                case "TIE-IN":
                    return ProjectCategory.TieIn;
                case "RENEWABLE_OTHER":
                    return ProjectCategory.RenewableOther;
                case "CCS":
                    return ProjectCategory.Ccs;
                default:
                    throw new Exception(String.Format("Category {0} does not exist in DCD.", category));
            }
        }

        private static ProjectPhase ConvertPhase(string phase)
        {
            switch (phase)
            {
                case "":
                    return ProjectPhase.Null;
                case "Bid preparations":
                    return ProjectPhase.BidPreparations;
                case "Business identification":
                    return ProjectPhase.BusinessIdentification;
                case "Business planning":
                    return ProjectPhase.BusinessPlanning;
                case "Concept planning":
                    return ProjectPhase.ConceptPlanning;
                case "Concession / Negotiations":
                    return ProjectPhase.ConcessionNegotiations;
                case "Definition":
                    return ProjectPhase.Definition;
                case "Execution":
                    return ProjectPhase.Execution;
                case "Operation":
                    return ProjectPhase.Operation;
                case "Screening business opportunities":
                    return ProjectPhase.ScreeningBusinessOpportunities;
                default:
                    throw new Exception(String.Format("Phase {0} does not exist in DCD.", phase));
            }
        }
    }
}
