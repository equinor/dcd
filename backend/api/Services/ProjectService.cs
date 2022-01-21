using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ProjectService
    {
        private readonly DcdDbContext _context;

        public ProjectService(DcdDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Project> GetAll()
        {
            if (_context.Projects != null)
            {

                var projects = _context.Projects
                    .Include(c => c.Cases)
                        .ThenInclude(c => c.CessationCost)
                            .ThenInclude(c => c.YearValues);

                foreach (Project project in projects)
                {
                    AddAssetsToProject(project);
                }
                return projects;
            }
            else
            {
                return new List<Project>();
            }
        }

        public Project GetProject(Guid projectId)
        {
            if (_context.Projects != null)
            {
                var project = _context.Projects
                    .Include(c => c.Cases)
                        .ThenInclude(c => c.CessationCost)
                            .ThenInclude(c => c.YearValues)
                    .FirstOrDefault(p => p.Id.Equals(projectId));

                if (project == null)
                {
                    throw new NotFoundInDBException(string.Format("Project %s not found", projectId));
                }
                AddAssetsToProject(project);
                return project;
            }
            throw new NotFoundInDBException($"The database contains no projects");
        }
        private Project AddAssetsToProject(Project project)
        {
            WellProjectService wellProjectService = new WellProjectService(_context);
            var wellProjects = wellProjectService.GetWellProjects(project.Id).ToList();
            project.WellProjects = wellProjects;
            DrainageStrategyService drainageStrategyService = new DrainageStrategyService(_context);
            var drainageStrategies = drainageStrategyService.GetDrainageStrategies(project.Id).ToList();
            project.DrainageStrategies = drainageStrategies;
            return project;
        }
    }
}
