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
        private readonly SubstructureService _substructureService;
        private readonly TopsideFacilityService _topsideFaciltyService;
        private readonly TransportService _transportService;

        public ProjectService(DcdDbContext context)
        {
            _context = context;
            _wellProjectService = new WellProjectService(_context);
            _drainageStrategyService = new DrainageStrategyService(_context, this);
            _facilityService = new FacilityService(_context);
            _substructureService = new SubstructureService(_context);
            _topsideFaciltyService = new TopsideFacilityService(_context);
            _transportService = new TransportService(_context);
        }

        public IEnumerable<Project> GetAll()
        {
            if (_context.Projects != null)
            {
                var projects = _context.Projects
                    .Include(c => c.Cases);

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
            project.Substructures = _substructureService.GetSubstructuresForProject(project.Id).ToList();
            project.Topsides = _topsideFaciltyService.GetTopsidesForProject(project.Id).ToList();
            project.Transports = _transportService.GetTransportsForProject(project.Id).ToList();
            return project;
        }
    }
}
