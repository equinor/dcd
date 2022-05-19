using System.Diagnostics;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

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
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(DcdDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<ProjectService>();
            _wellProjectService = new WellProjectService(_context, this, loggerFactory);
            _drainageStrategyService = new DrainageStrategyService(_context, this, loggerFactory);
            _surfService = new SurfService(_context, this, loggerFactory);
            _substructureService = new SubstructureService(_context, this, loggerFactory);
            _topsideService = new TopsideService(_context, this, loggerFactory);
            _caseService = new CaseService(_context, this, loggerFactory);
            _explorationService = new ExplorationService(_context, this, loggerFactory);
            _transportService = new TransportService(_context, this, loggerFactory);
        }

        public ProjectDto UpdateProject(ProjectDto projectDto)
        {

            var updatedProject = ProjectAdapter.Convert(projectDto);
            _context.Projects!.Update(updatedProject);
            _context.SaveChanges();
            return GetProjectDto(updatedProject.Id);
        }

        public ProjectDto CreateProject(Project project)
        {
            project.CreateDate = DateTimeOffset.UtcNow.Date;
            project.Cases = new List<Case>();
            project.DrainageStrategies = new List<DrainageStrategy>();
            project.Substructures = new List<Substructure>();
            project.Surfs = new List<Surf>();
            project.Topsides = new List<Topside>();
            project.Transports = new List<Transport>();
            project.WellProjects = new List<WellProject>();
            project.Explorations = new List<Exploration>();

            Activity.Current?.AddBaggage(nameof(project), JsonConvert.SerializeObject(project, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));

            if (_context.Projects != null)
            {
                var existingProjectLibIds = _context.Projects.Select(p => p.CommonLibraryId).ToList();
                if (existingProjectLibIds.Contains(project.CommonLibraryId))
                {
                    // Project already exists, navigate to project
                    _logger.LogInformation(nameof(project));
                    return GetProjectDto(_context.Projects.Where(p => p.CommonLibraryId == project.CommonLibraryId).First().Id);
                }
            }

            if (_context.Projects == null)
            {
                _logger.LogInformation("Empty projects: ", nameof(project));
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
            Activity.Current?.AddBaggage(nameof(_context.Projects), JsonConvert.SerializeObject(_context.Projects, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));
            if (_context.Projects != null)
            {
                var projects = _context.Projects
                    .Include(c => c.Cases);

                foreach (Project project in projects)
                {
                    AddAssetsToProject(project);
                }
                _logger.LogInformation("Get projects");
                return projects;
            }
            else
            {
                _logger.LogInformation("Get projects");
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
                Activity.Current?.AddBaggage(nameof(projectDtos), JsonConvert.SerializeObject(projectDtos, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

                _logger.LogInformation(nameof(projectDtos));
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
                Activity.Current?.AddBaggage(nameof(projectId), JsonConvert.SerializeObject(projectId, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
                AddAssetsToProject(project);
                _logger.LogInformation("Add assets to project", project.ToString());
                return project;
            }
            _logger.LogError(new NotFoundInDBException($"The database contains no projects"), "no projects");
            throw new NotFoundInDBException($"The database contains no projects");
        }

        public ProjectDto GetProjectDto(Guid projectId)
        {


            var project = GetProject(projectId);
            var projectDto = ProjectDtoAdapter.Convert(project);

            Activity.Current?.AddBaggage(nameof(projectDto), JsonConvert.SerializeObject(projectDto, Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));

            _logger.LogInformation(nameof(projectDto));
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
