using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class ProjectDtoAdapter
    {
        public static ProjectDto Convert(Project project)
        {
            var projectDto = new ProjectDto();
            projectDto.ProjectId = project.Id;
            projectDto.Name = project.Name;
            projectDto.Description = project.Description;
            projectDto.Country = project.Country;
            projectDto.ProjectCategory = project.ProjectCategory;
            projectDto.ProjectPhase = project.ProjectPhase;
            projectDto.Cases = new List<CaseDto>();
            foreach (Case c in project.Cases)
            {
                projectDto.Cases.Add(CaseDtoAdapter.Convert(c));
            }
            projectDto.Explorations = new List<ExplorationDto>();
            foreach (Exploration e in project.Explorations)
            {
                projectDto.Explorations.Add(ExplorationDtoAdapter.Convert(e));
            }
            projectDto.DrainageStrategies = new List<DrainageStrategyDto>();
            foreach (DrainageStrategy d in project.DrainageStrategies)
            {
                projectDto.DrainageStrategies.Add(DrainageStrategyDtoAdapter.Convert(d));
            }
            projectDto.WellProjects = new List<WellProjectDto>();
            foreach (WellProject w in project.WellProjects)
            {
                projectDto.WellProjects.Add(WellProjectDtoAdapter.Convert(w));
            }
            projectDto.Substructures = new List<SubstructureDto>();
            foreach (Substructure s in project.Substructures)
            {
                projectDto.Substructures.Add(SubstructureDtoAdapter.Convert(s));
            }
            projectDto.Surfs = new List<SurfDto>();
            foreach (Surf s in project.Surfs)
            {
                projectDto.Surfs.Add(SurfDtoAdapter.Convert(s));
            }
            projectDto.Topsides = new List<TopsideDto>();
            foreach (Topside t in project.Topsides)
            {
                projectDto.Topsides.Add(TopsideDtoAdapter.Convert(t));
            }
            projectDto.Transports = new List<TransportDto>();
            foreach (Transport t in project.Transports)
            {
                projectDto.Transports.Add(TransportDtoAdapter.Convert(t));
            }

            return projectDto;
        }

        public static ProjectDto Convert(dynamic project)
        {
            var projectDto = new ProjectDto();
            projectDto.Name = project.Name;
            projectDto.Description = project.Description;
            projectDto.Country = project.Country;
            projectDto.ProjectCategory = ConvertCategory(project.ProjectCategory.ToString());
            projectDto.ProjectPhase = ConvertPhase(project.Phase.ToString());
            projectDto.Cases = new List<CaseDto>();
            projectDto.Explorations = new List<ExplorationDto>();
            projectDto.DrainageStrategies = new List<DrainageStrategyDto>();
            projectDto.WellProjects = new List<WellProjectDto>();
            projectDto.Substructures = new List<SubstructureDto>();
            projectDto.Surfs = new List<SurfDto>();
            projectDto.Topsides = new List<TopsideDto>();
            projectDto.Transports = new List<TransportDto>();

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
