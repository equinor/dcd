using System.Linq;

using api.Context;
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

        public Project CreateWellProject(WellProject wellProject)
        {
            var project = _projectService.GetProject(wellProject.ProjectId);
            wellProject.Project = project;
            _context.WellProjects!.Add(wellProject);
            _context.SaveChanges();
            return _projectService.GetProject(project.Id);
        }

        public Project DeleteWellProject(Guid wellProjectId)
        {
            var wellProject = GetWellProject(wellProjectId);
            _context.WellProjects!.Remove(wellProject);
            _context.SaveChanges();
            return _projectService.GetProject(wellProject.ProjectId);
        }

        public Project UpdateWellProject(Guid wellProjectId, WellProject updatedWellProject)
        {
            var wellProject = GetWellProject(wellProjectId);
            CopyData(wellProject, updatedWellProject);
            _context.WellProjects!.Update(wellProject);
            _context.SaveChanges();
            return _projectService.GetProject(wellProject.ProjectId);
        }

        private WellProject GetWellProject(Guid wellProjectId)
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
