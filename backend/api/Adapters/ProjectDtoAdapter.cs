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
            projectDto.CommonLibraryName = project.CommonLibraryName;
            projectDto.Description = project.Description;
            projectDto.Country = project.Country;
            projectDto.CreateDate = project.CreateDate;
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
    }
}
