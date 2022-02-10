using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class WellProjectService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public WellProjectService(DcdDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public IEnumerable<WellProject> GetWellProjects(Guid projectId)
        {
            if (_context.WellProjects != null)
            {
                return _context.WellProjects
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DrillingSchedule)
                            .ThenInclude(c => c.YearValues)
                    .Where(d => d.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<WellProject>();
            }
        }

        public ProjectDto CreateWellProject(WellProject wellProject, Guid sourceCaseId)
        {
            var project = _projectService.GetProject(wellProject.ProjectId);
            wellProject.Project = project;
            _context.WellProjects!.Add(wellProject);
            _context.SaveChanges();
            SetCaseLink(wellProject, sourceCaseId, project);
            return _projectService.GetProjectDto(project.Id);
        }

        private void SetCaseLink(WellProject wellProject, Guid sourceCaseId, Project project)
        {
            var case_ = project.Cases.FirstOrDefault(o => o.Id == sourceCaseId);
            if (case_ == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
            }
            case_.WellProjectLink = wellProject.Id;
            _context.SaveChanges();
        }

        public ProjectDto DeleteWellProject(Guid wellProjectId)
        {
            var wellProject = GetWellProject(wellProjectId);
            _context.WellProjects!.Remove(wellProject);
            DeleteCaseLinks(wellProjectId);
            _context.SaveChanges();
            return _projectService.GetProjectDto(wellProject.ProjectId);
        }

        private void DeleteCaseLinks(Guid wellProjectId)
        {
            foreach (Case c in _context.Cases!)
            {
                if (c.WellProjectLink == wellProjectId)
                {
                    c.WellProjectLink = Guid.Empty;
                }
            }
        }

        public ProjectDto UpdateWellProject(Guid wellProjectId, WellProject updatedWellProject)
        {
            var wellProject = GetWellProject(wellProjectId);
            CopyData(wellProject, updatedWellProject);
            _context.WellProjects!.Update(wellProject);
            _context.SaveChanges();
            return _projectService.GetProjectDto(wellProject.ProjectId);
        }

        public WellProject GetWellProject(Guid wellProjectId)
        {
            var wellProject = _context.WellProjects!
                .Include(c => c.CostProfile)
                    .ThenInclude(c => c.YearValues)
                .Include(c => c.DrillingSchedule)
                    .ThenInclude(c => c.YearValues)
                .FirstOrDefault(o => o.Id == wellProjectId);
            if (wellProject == null)
            {
                throw new ArgumentException(string.Format("Well project {0} not found.", wellProjectId));
            }
            return wellProject;
        }

        private static void CopyData(WellProject wellProject, WellProject updatedWellProject)
        {
            wellProject.Name = updatedWellProject.Name;
            wellProject.ProducerCount = updatedWellProject.ProducerCount;
            wellProject.GasInjectorCount = updatedWellProject.GasInjectorCount;
            wellProject.WaterInjectorCount = updatedWellProject.WaterInjectorCount;
            wellProject.ArtificialLift = updatedWellProject.ArtificialLift;
            wellProject.RigMobDemob = updatedWellProject.RigMobDemob;
            wellProject.AnnualWellInterventionCost = updatedWellProject.AnnualWellInterventionCost;
            wellProject.PluggingAndAbandonment = updatedWellProject.PluggingAndAbandonment;
            wellProject.CostProfile.Currency = updatedWellProject.CostProfile.Currency;
            wellProject.CostProfile.EPAVersion = updatedWellProject.CostProfile.EPAVersion;
            wellProject.CostProfile.YearValues = updatedWellProject.CostProfile.YearValues;
            wellProject.DrillingSchedule.YearValues = updatedWellProject.DrillingSchedule.YearValues;
        }
    }
}
