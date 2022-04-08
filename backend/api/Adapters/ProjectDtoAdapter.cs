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
            projectDto.CommonLibraryId = project.CommonLibraryId;
            projectDto.CommonLibraryName = project.CommonLibraryName;
            projectDto.Description = project.Description;
            projectDto.Country = project.Country;
            projectDto.CreateDate = project.CreateDate;
            projectDto.ProjectCategory = project.ProjectCategory;
            projectDto.ProjectPhase = project.ProjectPhase;
            projectDto.Cases = new List<CaseDto>();
            if (project.Cases != null)
            {
                foreach (Case c in project.Cases)
                {
                    projectDto.Cases.Add(CaseDtoAdapter.Convert(c));
                }
            }
            if (project.Explorations != null)
            {
                projectDto.Explorations = new List<ExplorationDto>();
                foreach (Exploration e in project.Explorations)
                {
                    projectDto.Explorations.Add(ExplorationDtoAdapter.Convert(e));
                }
            }
            if (project.DrainageStrategies != null)
            {
                projectDto.DrainageStrategies = new List<DrainageStrategyDto>();
                foreach (DrainageStrategy d in project.DrainageStrategies)
                {
                    projectDto.DrainageStrategies.Add(DrainageStrategyDtoAdapter.Convert(d));
                }
            }
            if (project.WellProjects != null)
            {
                projectDto.WellProjects = new List<WellProjectDto>();
                foreach (WellProject w in project.WellProjects)
                {
                    projectDto.WellProjects.Add(WellProjectDtoAdapter.Convert(w));
                }
            }
            if (project.Substructures != null)
            {
                projectDto.Substructures = new List<SubstructureDto>();
                foreach (Substructure s in project.Substructures)
                {

                    projectDto.Substructures.Add(SubstructureDtoAdapter.Convert(s));
                }
            }
            if (project.Surfs != null)
            {
                projectDto.Surfs = new List<SurfDto>();
                foreach (Surf s in project.Surfs)
                {
                    projectDto.Surfs.Add(SurfDtoAdapter.Convert(s));
                }
            }
            if (project.Topsides != null)
            {
                projectDto.Topsides = new List<TopsideDto>();
                foreach (Topside t in project.Topsides)
                {
                    projectDto.Topsides.Add(TopsideDtoAdapter.Convert(t));
                }
            }
            if (project.Transports != null)
            {
                projectDto.Transports = new List<TransportDto>();
                foreach (Transport t in project.Transports)
                {
                    projectDto.Transports.Add(TransportDtoAdapter.Convert(t));
                }
            }
            if (projectDto.Cases != null)
            {
                AddCapexToCases(projectDto);
            }
            return projectDto;
        }

        public static void AddCapexToCases(ProjectDto p)
        {
            foreach (CaseDto c in p.Cases!)
            {
                c.Capex = 0;
                if (c.WellProjectLink != Guid.Empty)
                {
                    var wellProject = p.WellProjects!.First(l => l.Id == c.WellProjectLink);
                    if (wellProject.CostProfile != null)
                    {
                        c.Capex += wellProject.CostProfile?.Sum ?? 0;
                    }
                }
                if (c.SubstructureLink != Guid.Empty)
                {
                    c.Capex += p.Substructures!.First(l => l.Id == c.SubstructureLink)?.CostProfile?.Sum ?? 0;
                }
                if (c.SurfLink != Guid.Empty)
                {
                    c.Capex += p.Surfs!.First(l => l.Id == c.SurfLink)?.CostProfile?.Sum ?? 0;
                }
                if (c.TopsideLink != Guid.Empty)
                {
                    c.Capex += p.Topsides!.First(l => l.Id == c.TopsideLink)?.CostProfile?.Sum ?? 0;
                }
                if (c.TransportLink != Guid.Empty)
                {
                    c.Capex += p.Transports!.First(l => l.Id == c.TransportLink)?.CostProfile?.Sum ?? 0;
                }
            }
        }
    }
}
