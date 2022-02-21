using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ProjectService
    {
        private readonly DcdDbContext _context;
        private readonly WellProjectService _wellProjectService;
        private readonly DrainageStrategyService _drainageStrategyService;
        private readonly SurfService _surfService;
        private readonly SubstructureService _substructureService;
        private readonly TopsideService _topsideService;
        private readonly TransportService _transportService;
        private readonly ExplorationService _explorationService;

        private readonly CaseService _caseService;

        public ProjectService(DcdDbContext context)
        {
            _context = context;
            _wellProjectService = new WellProjectService(_context, this);
            _drainageStrategyService = new DrainageStrategyService(_context, this);
            _surfService = new SurfService(_context, this);
            _substructureService = new SubstructureService(_context, this);
            _topsideService = new TopsideService(_context, this);
            _caseService = new CaseService(_context, this);
            _explorationService = new ExplorationService(_context, this);
            _transportService = new TransportService(_context, this);
        }

        public ProjectDto CreateProject(Project project)
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
                projects.Add(project);
                _context.AddRange(projects);
            }
            else
            {
                _context.Projects.Add(project);
            }
            _context.SaveChanges();
            return GetProjectDto(project.Id);
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

        public IEnumerable<ProjectDto> GetAllDtos()
        {
            if (GetAll() != null)
            {
                var projects = GetAll();
                var projectDtos = new List<ProjectDto>();
                foreach (Project project in projects)
                {
                    var projectDto = ProjectDtoAdapter.Convert(project);
                    projectDtos.Add(projectDto);
                }

                return projectDtos;
            }
            else
            {
                return new List<ProjectDto>();
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

        public ProjectDto GetProjectDto(Guid projectId)
        {
            var project = GetProject(projectId);
            var projectDto = ProjectDtoAdapter.Convert(project);
            return projectDto;
        }

        private Project AddAssetsToProject(Project project)
        {
            project.WellProjects = _wellProjectService.GetWellProjects(project.Id).ToList();
            project.DrainageStrategies = _drainageStrategyService.GetDrainageStrategies(project.Id).ToList();
            project.Surfs = _surfService.GetSurfs(project.Id).ToList();
            project.Substructures = _substructureService.GetSubstructures(project.Id).ToList();
            project.Topsides = _topsideService.GetTopsides(project.Id).ToList();
            project.Transports = _transportService.GetTransports(project.Id).ToList();
            project.Explorations = _explorationService.GetExplorations(project.Id).ToList();
            return project;
        }
    }
}
