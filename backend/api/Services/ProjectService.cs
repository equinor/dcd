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
        private readonly SurfService _surfService;
        private readonly SubstructureService _substructureService;
        private readonly TopsideFacilityService _topsideFaciltyService;
        private readonly TransportService _transportService;

        public ProjectService(DcdDbContext context)
        {
            _context = context;
            _wellProjectService = new WellProjectService(_context);
            _drainageStrategyService = new DrainageStrategyService(_context, this);
            _surfService = new SurfService(_context, this);
            _substructureService = new SubstructureService(_context, this);
            _topsideFaciltyService = new TopsideFacilityService(_context);
            _transportService = new TransportService(_context);
        }

        public Project CreateProject(Project project)
        {
            project.Cases = new List<Case>();
            project.DrainageStrategies = new List<DrainageStrategy>();
            project.Substructures = new List<Substructure>();
            project.Surfs = new List<Surf>();
            project.Topsides = new List<Topside>();
            project.Transports = new List<Transport>();
            project.WellProjects = new List<WellProject>();
            project.Explorations = new List<Exploration>();

            if (_context.Projects == null)
            {
                var projects = new List<Project>();
                projects.AddRange(projects);
                _context.AddRange(projects);
            }
            else
            {
                _context.Projects.AddRange(project);
            }
            _context.SaveChanges();
            return project;
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
                    throw new NotFoundInDBException(string.Format("Project {0} not found", projectId));
                }
                AddAssetsToProject(project);
                return project;
            }
            throw new NotFoundInDBException($"The database contains no projects");
        }
        public void AddExploration(Project project, Exploration exploration)
        {
            project.Explorations.Add(exploration);
        }
        private Project AddAssetsToProject(Project project)
        {
            project.WellProjects = _wellProjectService.GetWellProjects(project.Id).ToList();
            project.DrainageStrategies = _drainageStrategyService.GetDrainageStrategies(project.Id).ToList();
            project.Surfs = _surfService.GetSurfs(project.Id).ToList();
            project.Substructures = _substructureService.GetSubstructures(project.Id).ToList();
            project.Topsides = _topsideFaciltyService.GetTopsides(project.Id).ToList();
            project.Transports = _transportService.GetTransports(project.Id).ToList();
            return project;
        }

        public void AddSurfsToProject(Project project, Surf surf)
        {
            project.Surfs.Add(surf);
        }

        public void AddDrainageStrategy(Project project, DrainageStrategy drainageStrategy)
        {
            project.DrainageStrategies.Add(drainageStrategy);
        }

        public void AddSubstructure(Project project, Substructure substructure)
        {
            project.Substructures.Add(substructure);
        }
    }
}
