using System.Diagnostics;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Api.Services.FusionIntegration;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace api.Services;

public class ProjectService : IProjectService
{
    private readonly DcdDbContext _context;
    private readonly IFusionService? _fusionService;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(DcdDbContext context, ILoggerFactory loggerFactory,
        FusionService? fusionService = null)
    {
        _context = context;
        _logger = loggerFactory.CreateLogger<ProjectService>();
        _fusionService = fusionService;
    }

    public ProjectDto UpdateProject(ProjectDto projectDto)
    {
        var existingProject = GetProject(projectDto.ProjectId);
        ProjectAdapter.ConvertExisting(existingProject, projectDto);
        _context.Projects!.Update(existingProject);
        _context.SaveChanges();
        return GetProjectDto(existingProject.Id);
    }

    public ProjectDto UpdateProjectFromProjectMaster(ProjectDto projectDto)
    {
        var existingProject = GetProject(projectDto.ProjectId);
        ProjectAdapter.ConvertExistingFromProjectMaster(existingProject, projectDto);
        _context.Projects!.Update(existingProject);
        _context.SaveChanges();
        return GetProjectDto(existingProject.Id);
    }

    public ProjectDto CreateProject(Project project)
    {
        project.CreateDate = DateTimeOffset.UtcNow;
        project.Cases = new List<Case>();
        project.ReferenceCaseId = Guid.Empty;
        project.DrainageStrategies = new List<DrainageStrategy>();
        project.Substructures = new List<Substructure>();
        project.Surfs = new List<Surf>();
        project.Topsides = new List<Topside>();
        project.Transports = new List<Transport>();
        project.WellProjects = new List<WellProject>();
        project.Explorations = new List<Exploration>();

        project.ExplorationOperationalWellCosts = new ExplorationOperationalWellCosts();
        project.DevelopmentOperationalWellCosts = new DevelopmentOperationalWellCosts();

        project.CO2EmissionFromFuelGas = 2.34;
        project.FlaredGasPerProducedVolume = 1.321;
        project.CO2EmissionsFromFlaredGas = 3.74;
        project.CO2Vented = 1.96;
        project.DailyEmissionFromDrillingRig = 100;
        project.AverageDevelopmentDrillingDays = 50;

        Activity.Current?.AddBaggage(nameof(project), JsonConvert.SerializeObject(project, Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));

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
        Activity.Current?.AddBaggage(nameof(_context.Projects), JsonConvert.SerializeObject(_context.Projects,
            Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));
        if (_context.Projects != null)
        {
            var projects = _context.Projects
                .Include(c => c.Cases);

            foreach (var project in projects)
            {
                AddAssetsToProject(project);
            }

            return projects;
        }

        return new List<Project>();
    }

    public IEnumerable<ProjectDto> GetAllDtos()
    {
        if (GetAll() != null)
        {
            var projects = GetAll();
            var projectDtos = new List<ProjectDto>();
            foreach (var project in projects)
            {
                var projectDto = ProjectDtoAdapter.Convert(project);
                projectDtos.Add(projectDto);
            }

            Activity.Current?.AddBaggage(nameof(projectDtos), JsonConvert.SerializeObject(projectDtos, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                }));

            return projectDtos;
        }

        return new List<ProjectDto>();
    }

    public Project GetProjectWithoutAssets(Guid projectId)
    {
        if (_context.Projects != null)
        {
            if (projectId == Guid.Empty)
            {
                throw new NotFoundInDBException($"Project {projectId} not found");
            }

            var project = _context.Projects!
                .Include(p => p.Cases)
                .Include(p => p.Wells)
                .Include(p => p.ExplorationOperationalWellCosts)
                .Include(p => p.DevelopmentOperationalWellCosts)
                .FirstOrDefault(p => p.Id.Equals(projectId));

            if (project == null)
            {
                var projectByFusionId = _context.Projects
                    .Include(p => p.Cases)
                    .Include(p => p.Wells)
                    .Include(p => p.ExplorationOperationalWellCosts)
                    .Include(p => p.DevelopmentOperationalWellCosts)
                    .FirstOrDefault(p => p.FusionProjectId.Equals(projectId));
                project = projectByFusionId ?? throw new NotFoundInDBException($"Project {projectId} not found");
            }

            return project;
        }

        _logger.LogError(new NotFoundInDBException("The database contains no projects"), "no projects");
        throw new NotFoundInDBException("The database contains no projects");
    }

    public Project GetProjectWithoutAssetsNoTracking(Guid projectId)
    {
        if (_context.Projects != null)
        {
            if (projectId == Guid.Empty)
            {
                throw new NotFoundInDBException($"Project {projectId} not found");
            }

            var project = _context.Projects
                .AsNoTracking()
                .Include(p => p.Cases)
                .Include(p => p.Wells)
                .Include(p => p.ExplorationOperationalWellCosts)
                .Include(p => p.DevelopmentOperationalWellCosts)
                .FirstOrDefault(p => p.Id.Equals(projectId));

            if (project == null)
            {
                var projectByFusionId = _context.Projects
                    .AsNoTracking()
                    .Include(p => p.Cases)
                    .Include(p => p.Wells)
                    .Include(p => p.ExplorationOperationalWellCosts)
                    .Include(p => p.DevelopmentOperationalWellCosts)
                    .FirstOrDefault(p => p.FusionProjectId.Equals(projectId));
                project = projectByFusionId ?? throw new NotFoundInDBException($"Project {projectId} not found");
            }

            return project;
        }

        _logger.LogError(new NotFoundInDBException("The database contains no projects"), "no projects");
        throw new NotFoundInDBException("The database contains no projects");
    }

    public Project GetProject(Guid projectId)
    {
        if (_context.Projects != null)
        {
            if (projectId == Guid.Empty)
            {
                throw new NotFoundInDBException($"Project {projectId} not found");
            }

            var project = _context.Projects!
                .Include(p => p.Cases)!.ThenInclude(c => c.TotalFeasibilityAndConceptStudies)
                .Include(p => p.Cases)!.ThenInclude(c => c.TotalFeasibilityAndConceptStudiesOverride)
                .Include(p => p.Cases)!.ThenInclude(c => c.TotalFEEDStudies)
                .Include(p => p.Cases)!.ThenInclude(c => c.TotalFEEDStudiesOverride)
                .Include(p => p.Wells)
                .Include(p => p.ExplorationOperationalWellCosts)
                .Include(p => p.DevelopmentOperationalWellCosts)
                .FirstOrDefault(p => p.Id.Equals(projectId));

            if (project?.Cases?.Count > 0)
            {
                project.Cases = project.Cases.OrderBy(c => c.CreateTime).ToList();
            }

            if (project == null)
            {
                var projectByFusionId = _context.Projects
                    .Include(c => c.Cases)
                    .FirstOrDefault(p => p.FusionProjectId.Equals(projectId));
                if (projectByFusionId == null)
                {
                    throw new NotFoundInDBException(string.Format("Project {0} not found", projectId));
                }

                project = projectByFusionId;
            }

            Activity.Current?.AddBaggage(nameof(projectId), JsonConvert.SerializeObject(projectId, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                }));
            AddAssetsToProject(project);
            _logger.LogInformation("Add assets to project: {projectId}", projectId.ToString());
            return project;
        }

        _logger.LogError(new NotFoundInDBException("The database contains no projects"), "no projects");
        throw new NotFoundInDBException("The database contains no projects");
    }

    public ProjectDto GetProjectDto(Guid projectId)
    {
        var project = GetProject(projectId);
        var projectDto = ProjectDtoAdapter.Convert(project);

        Activity.Current?.AddBaggage(nameof(projectDto), JsonConvert.SerializeObject(projectDto, Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));
        return projectDto;
    }

    public void UpdateProjectFromProjectMaster()
    {
        var projectDtos = GetAllDtos();
        var numberOfDeviations = 0;
        var totalNumberOfProjects = projectDtos.Count();
        foreach (var project in projectDtos)
        {
            var projectMaster = GetProjectDtoFromProjectMaster(project.ProjectId);
            if (!project.Equals(projectMaster))
            {
                _logger.LogWarning("Project {projectName} ({projectId}) differs from ProjectMaster", project.Name,
                    project.ProjectId);
                numberOfDeviations++;
                UpdateProjectFromProjectMaster(projectMaster);
            }
            else
            {
                _logger.LogInformation("Project {projectName} ({projectId}) is identical to ProjectMaster",
                    project.Name, project.ProjectId);
            }
        }

        _logger.LogInformation("Number of projects which differs from ProjectMaster: {count} / {total}",
            numberOfDeviations, totalNumberOfProjects);
    }

    private ProjectDto GetProjectDtoFromProjectMaster(Guid projectGuid)
    {
        if (_fusionService != null)
        {
            var projectMaster = _fusionService.ProjectMasterAsync(projectGuid).GetAwaiter().GetResult();
            var category = CommonLibraryProjectDtoAdapter.ConvertCategory(projectMaster.ProjectCategory ?? "");
            var phase = CommonLibraryProjectDtoAdapter.ConvertPhase(projectMaster.Phase ?? "");
            ProjectDto projectDto = new()
            {
                Name = projectMaster.Description ?? "",
                CommonLibraryName = projectMaster.Description ?? "",
                FusionProjectId = projectMaster.Identity,
                Country = projectMaster.Country ?? "",
                Currency = Currency.NOK,
                PhysUnit = PhysUnit.SI,
                ProjectId = projectMaster.Identity,
                ProjectCategory = category,
                ProjectPhase = phase,
            };
            return projectDto;
        }

        _logger.LogCritical("FusionService is null!");
        throw new NullReferenceException();
    }

    public ProjectDto SetReferenceCase(ProjectDto projectDto)
    {
        if (projectDto.ProjectId == Guid.Empty)
        {
            throw new NotFoundInDBException($"Project {projectDto.ProjectId} not found");
        }

        var project = GetProject(projectDto.ProjectId);
        project.ReferenceCaseId = projectDto.ReferenceCaseId;

        _context.Projects?.Update(project);
        _context.SaveChanges();
        return ProjectDtoAdapter.Convert(project);
    }

    private Project AddAssetsToProject(Project project)
    {
        project.WellProjects = GetWellProjects(project.Id).ToList();
        project.DrainageStrategies = GetDrainageStrategies(project.Id).ToList();
        project.Surfs = GetSurfs(project.Id).ToList();
        project.Substructures = GetSubstructures(project.Id).ToList();
        project.Topsides = GetTopsides(project.Id).ToList();
        project.Transports = GetTransports(project.Id).ToList();
        project.Explorations = GetExplorations(project.Id).ToList();
        project.Wells = GetWells(project.Id).ToList();
        return project;
    }

    public IEnumerable<Well> GetWells(Guid projectId)
    {
        if (_context.Wells != null)
        {
            return _context.Wells
                .Where(d => d.ProjectId.Equals(projectId));
        }
        else
        {
            return new List<Well>();
        }
    }

    public IEnumerable<Exploration> GetExplorations(Guid projectId)
    {
        if (_context.Explorations != null)
        {
            return _context.Explorations
                .Include(c => c.ExplorationWellCostProfile)
                .Include(c => c.AppraisalWellCostProfile)
                .Include(c => c.SidetrackCostProfile)
                .Include(c => c.GAndGAdminCost)
                .Include(c => c.SeismicAcquisitionAndProcessing)
                .Include(c => c.CountryOfficeCost)
                .Include(c => c.ExplorationWells!).ThenInclude(ew => ew.DrillingSchedule)
                .Where(d => d.Project.Id.Equals(projectId));
        }
        else
        {
            return new List<Exploration>();
        }
    }
    public IEnumerable<Transport> GetTransports(Guid projectId)
    {
        if (_context.Transports != null)
        {
            return _context.Transports
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .Where(c => c.Project.Id.Equals(projectId));
        }
        else
        {
            return new List<Transport>();
        }
    }

    public IEnumerable<Topside> GetTopsides(Guid projectId)
    {
        if (_context.Topsides != null)
        {
            return _context.Topsides
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .Where(c => c.Project.Id.Equals(projectId));
        }
        else
        {
            return new List<Topside>();
        }
    }

    public IEnumerable<Surf> GetSurfs(Guid projectId)
    {
        if (_context.Surfs != null)
        {
            return _context.Surfs
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .Where(c => c.Project.Id.Equals(projectId));
        }
        else
        {
            return new List<Surf>();
        }
    }

    public IEnumerable<DrainageStrategy> GetDrainageStrategies(Guid projectId)
    {
        if (_context.DrainageStrategies != null)
        {
            return _context.DrainageStrategies
                .Include(c => c.ProductionProfileOil)
                .Include(c => c.ProductionProfileGas)
                .Include(c => c.ProductionProfileWater)
                .Include(c => c.ProductionProfileWaterInjection)
                .Include(c => c.FuelFlaringAndLosses)
                .Include(c => c.FuelFlaringAndLossesOverride)
                .Include(c => c.NetSalesGas)
                .Include(c => c.NetSalesGasOverride)
                .Include(c => c.Co2Emissions)
                .Include(c => c.Co2EmissionsOverride)
                .Include(c => c.ProductionProfileNGL)
                .Include(c => c.ImportedElectricity)
                .Include(c => c.ImportedElectricityOverride)
                .Where(d => d.Project.Id.Equals(projectId));
        }
        else
        {
            return new List<DrainageStrategy>();
        }
    }

    public IEnumerable<WellProject> GetWellProjects(Guid projectId)
    {
        if (_context.WellProjects != null)
        {
            return _context.WellProjects
                .Include(c => c.OilProducerCostProfile)
                .Include(c => c.OilProducerCostProfileOverride)
                .Include(c => c.GasProducerCostProfile)
                .Include(c => c.GasProducerCostProfileOverride)
                .Include(c => c.WaterInjectorCostProfile)
                .Include(c => c.WaterInjectorCostProfileOverride)
                .Include(c => c.GasInjectorCostProfile)
                .Include(c => c.GasInjectorCostProfileOverride)
                .Include(c => c.WellProjectWells!).ThenInclude(wpw => wpw.DrillingSchedule)
                .Where(d => d.Project.Id.Equals(projectId));
        }
        else
        {
            return new List<WellProject>();
        }
    }

    public IEnumerable<Substructure> GetSubstructures(Guid projectId)
    {
        if (_context.Substructures != null)
        {
            return _context.Substructures
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .Where(c => c.Project.Id.Equals(projectId));
        }
        else
        {
            return new List<Substructure>();
        }
    }
}
