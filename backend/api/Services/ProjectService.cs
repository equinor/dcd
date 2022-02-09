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
        private readonly TopsideService _topsideFaciltyService;
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
            _topsideFaciltyService = new TopsideService(_context, this);
            _caseService = new CaseService(_context, this);
            _explorationService = new ExplorationService(_context, this);
            _transportService = new TransportService(_context, this);
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

        public IEnumerable<ProjectDto> GetAllDtos()
        {
            if (GetAll() != null)
            {
                var projects = GetAll();
                var projectDtos = new List<ProjectDto>();
                foreach (Project project in projects)
                {
                    var projectDto = ProjectDtoAdapter.Convert(project);
                    AddCapexToCases(projectDto);
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
            AddCapexToCases(projectDto);
            return projectDto;
        }

        private Project AddAssetsToProject(Project project)
        {
            project.WellProjects = _wellProjectService.GetWellProjects(project.Id).ToList();
            project.DrainageStrategies = _drainageStrategyService.GetDrainageStrategies(project.Id).ToList();
            project.Surfs = _surfService.GetSurfs(project.Id).ToList();
            project.Substructures = _substructureService.GetSubstructures(project.Id).ToList();
            project.Topsides = _topsideFaciltyService.GetTopsides(project.Id).ToList();
            project.Transports = _transportService.GetTransports(project.Id).ToList();
            project.Explorations = _explorationService.GetExplorations(project.Id).ToList();
            return project;
        }

        private void AddCapexToCases(ProjectDto projectDto)
        {
            foreach (CaseDto c in projectDto.Cases)
            {
                c.Capex = 0;
                if (c.WellProjectLink != Guid.Empty)
                {
                    var wellProject = _wellProjectService.GetWellProject(c.WellProjectLink);
                    c.Capex += sumValues(wellProject.CostProfile);
                }
                if (c.SubstructureLink != Guid.Empty)
                {
                    var substructure = _substructureService.GetSubstructure(c.SubstructureLink);
                    c.Capex += sumValues(substructure.CostProfile);
                }
                if (c.SurfLink != Guid.Empty)
                {
                    var surf = _surfService.GetSurf(c.SurfLink);
                    c.Capex += sumValues(surf.CostProfile);
                }
                if (c.TopsideLink != Guid.Empty)
                {
                    var topside = _topsideFaciltyService.GetTopside(c.TopsideLink);
                    c.Capex += sumValues(topside.CostProfile);
                }
                if (c.TransportLink != Guid.Empty)
                {
                    var transport = _transportService.GetTransport(c.TransportLink);
                    c.Capex += sumValues(transport.CostProfile);
                }
            }
        }

        private double sumValues(TimeSeriesCost<double> timeSeries)
        {
            double sum = 0;
            foreach (YearValue<double> yearValue in timeSeries.YearValues)
            {
                sum += yearValue.Value;
            }
            return sum;
        }
    }
}
