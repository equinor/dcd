using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ProjectService : IProjectService
    {
        private readonly DcdDbContext _context;
        private readonly WellProjectService _wellProjectService;
        private readonly DrainageStrategyService _drainageStrategyService;
        private readonly FacilityService _facilityService;

        public ProjectService(DcdDbContext context)
        {
            _context = context;
            _wellProjectService = new WellProjectService(_context);
            _drainageStrategyService = new DrainageStrategyService(_context, this);
            _facilityService = new FacilityService(_context);
        }

        public IEnumerable<Project> GetAll()
        {
            if (_context.Projects != null)
            {

                var projects = _context.Projects!
                    .Include(c => c.Cases!)
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
                var project = _context.Projects!
                    .Include(c => c.Cases!)
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
            project.WellProjects = _wellProjectService.GetWellProjects(project.Id).ToList();
            project.DrainageStrategies = _drainageStrategyService.GetDrainageStrategies(project.Id).ToList();
            project.Surfs = _facilityService.GetSurfsForProject(project.Id).ToList();
            project.Substructures = _facilityService.GetSubstructuresForProject(project.Id).ToList();
            project.Topsides = _facilityService.GetTopsidesForProject(project.Id).ToList();
            project.Transports = _facilityService.GetTransportsForProject(project.Id).ToList();
            return project;
        }
        public void AddDrainageStrategy(Project project, DrainageStrategy drainageStrategy) {
            
            project.DrainageStrategies!.Add(drainageStrategy);
        }
    }
}
