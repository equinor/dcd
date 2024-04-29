using System.Diagnostics;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Mappings;
using api.Models;

using Api.Services.FusionIntegration;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace api.Services;

public class ProjectService : IProjectService
{
    private readonly DcdDbContext _context;
    private readonly IFusionService? _fusionService;
    private readonly ILogger<ProjectService> _logger;
    private readonly IMapper _mapper;

    public ProjectService(
        DcdDbContext context,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        FusionService? fusionService = null
        )
    {
        _context = context;
        _logger = loggerFactory.CreateLogger<ProjectService>();
        _fusionService = fusionService;
        _mapper = mapper;
    }

    public async Task<ProjectDto> UpdateProject(Guid projectId, UpdateProjectDto projectDto)
    {
        var existingProject = await GetProject(projectId);

        _mapper.Map(projectDto, existingProject);

        _context.Projects!.Update(existingProject);
        await _context.SaveChangesAsync();
        return await GetProjectDto(existingProject.Id);
    }

    public async Task<ProjectDto> UpdateProjectFromProjectMaster(ProjectDto projectDto)
    {
        var existingProject = await GetProject(projectDto.Id);

        _mapper.Map(projectDto, existingProject);

        _context.Projects!.Update(existingProject);
        await _context.SaveChangesAsync();
        return await GetProjectDto(existingProject.Id);
    }

    public async Task<ProjectDto> CreateProject(Project project)
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
            _logger.LogInformation($"Empty projects: {nameof(project)}");
            var projects = new List<Project>
            {
                project
            };
            await _context.AddRangeAsync(projects);
        }
        else
        {
            _context.Projects.Add(project);
        }

        await _context.SaveChangesAsync();
        return await GetProjectDto(project.Id);
    }

    public async Task<IEnumerable<Project>> GetAll()
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
                await AddAssetsToProject(project);
            }

            return projects;
        }

        return new List<Project>();
    }

    public async Task<IEnumerable<ProjectDto>> GetAllDtos()
    {
        var projects = await GetAll();
        if (projects != null)
        {
            var projectDtos = new List<ProjectDto>();
            foreach (var project in projects)
            {
                var projectDto = _mapper.Map<ProjectDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());
                if (projectDto != null)
                {
                    projectDtos.Add(projectDto);
                }
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

    public async Task<Project> GetProjectWithoutAssets(Guid projectId)
    {
        if (_context.Projects != null)
        {
            if (projectId == Guid.Empty)
            {
                throw new NotFoundInDBException($"Project {projectId} not found");
            }

            var project = await _context.Projects!
                .Include(p => p.Cases)
                .Include(p => p.Wells)
                .Include(p => p.ExplorationOperationalWellCosts)
                .Include(p => p.DevelopmentOperationalWellCosts)
                .FirstOrDefaultAsync(p => p.Id.Equals(projectId));

            if (project == null)
            {
                var projectByFusionId = await _context.Projects
                    .Include(p => p.Cases)
                    .Include(p => p.Wells)
                    .Include(p => p.ExplorationOperationalWellCosts)
                    .Include(p => p.DevelopmentOperationalWellCosts)
                    .FirstOrDefaultAsync(p => p.FusionProjectId.Equals(projectId));
                project = projectByFusionId ?? throw new NotFoundInDBException($"Project {projectId} not found");
            }

            return project;
        }

        _logger.LogError(new NotFoundInDBException("The database contains no projects"), "no projects");
        throw new NotFoundInDBException("The database contains no projects");
    }

    public async Task<Project> GetProjectWithoutAssetsNoTracking(Guid projectId)
    {
        if (_context.Projects != null)
        {
            if (projectId == Guid.Empty)
            {
                throw new NotFoundInDBException($"Project {projectId} not found");
            }

            var project = await _context.Projects
                .AsNoTracking()
                .Include(p => p.Cases)
                .Include(p => p.Wells)
                .Include(p => p.ExplorationOperationalWellCosts)
                .Include(p => p.DevelopmentOperationalWellCosts)
                .FirstOrDefaultAsync(p => p.Id.Equals(projectId));

            if (project == null)
            {
                var projectByFusionId = await _context.Projects
                    .AsNoTracking()
                    .Include(p => p.Cases)
                    .Include(p => p.Wells)
                    .Include(p => p.ExplorationOperationalWellCosts)
                    .Include(p => p.DevelopmentOperationalWellCosts)
                    .FirstOrDefaultAsync(p => p.FusionProjectId.Equals(projectId));
                project = projectByFusionId ?? throw new NotFoundInDBException($"Project {projectId} not found");
            }

            return project;
        }

        _logger.LogError(new NotFoundInDBException("The database contains no projects"), "no projects");
        throw new NotFoundInDBException("The database contains no projects");
    }

    public async Task<Project> GetProject(Guid projectId)
    {
        if (_context.Projects != null)
        {
            if (projectId == Guid.Empty)
            {
                throw new NotFoundInDBException($"Project {projectId} not found");
            }

            var project = await _context.Projects!
                .Include(p => p.Cases)!.ThenInclude(c => c.TotalFeasibilityAndConceptStudies)
                .Include(p => p.Cases)!.ThenInclude(c => c.TotalFeasibilityAndConceptStudiesOverride)
                .Include(p => p.Cases)!.ThenInclude(c => c.TotalFEEDStudies)
                .Include(p => p.Cases)!.ThenInclude(c => c.TotalFEEDStudiesOverride)
                .Include(p => p.Cases)!.ThenInclude(c => c.TotalOtherStudies)
                .Include(p => p.Cases)!.ThenInclude(c => c.WellInterventionCostProfile)
                .Include(p => p.Cases)!.ThenInclude(c => c.WellInterventionCostProfileOverride)
                .Include(p => p.Cases)!.ThenInclude(c => c.OffshoreFacilitiesOperationsCostProfile)
                .Include(p => p.Cases)!.ThenInclude(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
                .Include(p => p.Cases)!.ThenInclude(c => c.HistoricCostCostProfile)
                .Include(p => p.Cases)!.ThenInclude(c => c.OnshoreRelatedOPEXCostProfile)
                .Include(p => p.Cases)!.ThenInclude(c => c.AdditionalOPEXCostProfile)
                .Include(p => p.Cases)!.ThenInclude(c => c.CessationWellsCost)
                .Include(p => p.Cases)!.ThenInclude(c => c.CessationWellsCostOverride)
                .Include(p => p.Cases)!.ThenInclude(c => c.CessationOffshoreFacilitiesCost)
                .Include(p => p.Cases)!.ThenInclude(c => c.CessationOffshoreFacilitiesCostOverride)
                .Include(p => p.Cases)!.ThenInclude(c => c.CessationOnshoreFacilitiesCostProfile)
                .Include(p => p.Wells)
                .Include(p => p.ExplorationOperationalWellCosts)
                .Include(p => p.DevelopmentOperationalWellCosts)
                .FirstOrDefaultAsync(p => p.Id.Equals(projectId));

            if (project?.Cases?.Count > 0)
            {
                project.Cases = project.Cases.OrderBy(c => c.CreateTime).ToList();
            }

            if (project == null)
            {
                var projectByFusionId = await _context.Projects!
                    .Include(p => p.Cases)!.ThenInclude(c => c.TotalFeasibilityAndConceptStudies)
                    .Include(p => p.Cases)!.ThenInclude(c => c.TotalFeasibilityAndConceptStudiesOverride)
                    .Include(p => p.Cases)!.ThenInclude(c => c.TotalFEEDStudies)
                    .Include(p => p.Cases)!.ThenInclude(c => c.TotalFEEDStudiesOverride)
                    .Include(p => p.Cases)!.ThenInclude(c => c.TotalOtherStudies)
                    .Include(p => p.Cases)!.ThenInclude(c => c.WellInterventionCostProfile)
                    .Include(p => p.Cases)!.ThenInclude(c => c.WellInterventionCostProfileOverride)
                    .Include(p => p.Cases)!.ThenInclude(c => c.OffshoreFacilitiesOperationsCostProfile)
                    .Include(p => p.Cases)!.ThenInclude(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
                    .Include(p => p.Cases)!.ThenInclude(c => c.HistoricCostCostProfile)
                    .Include(p => p.Cases)!.ThenInclude(c => c.OnshoreRelatedOPEXCostProfile)
                    .Include(p => p.Cases)!.ThenInclude(c => c.AdditionalOPEXCostProfile)
                    .Include(p => p.Cases)!.ThenInclude(c => c.CessationWellsCost)
                    .Include(p => p.Cases)!.ThenInclude(c => c.CessationWellsCostOverride)
                    .Include(p => p.Cases)!.ThenInclude(c => c.CessationOffshoreFacilitiesCost)
                    .Include(p => p.Cases)!.ThenInclude(c => c.CessationOffshoreFacilitiesCostOverride)
                    .Include(p => p.Cases)!.ThenInclude(c => c.CessationOnshoreFacilitiesCostProfile)
                    .Include(p => p.Wells)
                    .Include(p => p.ExplorationOperationalWellCosts)
                    .Include(p => p.DevelopmentOperationalWellCosts)
                    .FirstOrDefaultAsync(p => p.FusionProjectId.Equals(projectId));

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
            await AddAssetsToProject(project);
            return project;
        }

        _logger.LogError(new NotFoundInDBException("The database contains no projects"), "no projects");
        throw new NotFoundInDBException("The database contains no projects");
    }

    public async Task<ProjectDto> GetProjectDto(Guid projectId)
    {
        var project = await GetProject(projectId);

        var destination = _mapper.Map<Project, ProjectDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

        var projectDto = destination;

        if (projectDto == null)
        {
            throw new NotFoundInDBException(string.Format("Project {0} not found", projectId));
        }

        Activity.Current?.AddBaggage(nameof(projectDto), JsonConvert.SerializeObject(projectDto, Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));
        return projectDto;
    }

    public async Task UpdateProjectFromProjectMaster()
    {
        var projectDtos = await GetAllDtos();
        var numberOfDeviations = 0;
        var totalNumberOfProjects = projectDtos.Count();
        foreach (var project in projectDtos)
        {
            var projectMaster = await GetProjectDtoFromProjectMaster(project.Id);
            if (!project.Equals(projectMaster))
            {
                _logger.LogWarning("Project {projectName} ({projectId}) differs from ProjectMaster", project.Name,
                    project.Id);
                numberOfDeviations++;
                await UpdateProjectFromProjectMaster(projectMaster);
            }
            else
            {
                _logger.LogInformation("Project {projectName} ({projectId}) is identical to ProjectMaster",
                    project.Name, project.Id);
            }
        }

        _logger.LogInformation("Number of projects which differs from ProjectMaster: {count} / {total}",
            numberOfDeviations, totalNumberOfProjects);
    }

    private async Task<ProjectDto> GetProjectDtoFromProjectMaster(Guid projectGuid)
    {
        if (_fusionService != null)
        {
            var projectMaster = await _fusionService.ProjectMasterAsync(projectGuid);
            // var category = CommonLibraryProjectDtoAdapter.ConvertCategory(projectMaster.ProjectCategory ?? "");
            // var phase = CommonLibraryProjectDtoAdapter.ConvertPhase(projectMaster.Phase ?? "");
            ProjectDto projectDto = new()
            {
                Name = projectMaster.Description ?? "",
                CommonLibraryName = projectMaster.Description ?? "",
                FusionProjectId = projectMaster.Identity,
                Country = projectMaster.Country ?? "",
                Currency = Currency.NOK,
                PhysicalUnit = PhysUnit.SI,
                Id = projectMaster.Identity,
                // ProjectCategory = category,
                // ProjectPhase = phase,
            };
            return projectDto;
        }

        _logger.LogCritical("FusionService is null!");
        throw new NullReferenceException();
    }

    private async Task<Project> AddAssetsToProject(Project project)
    {
        project.WellProjects = (await GetWellProjects(project.Id)).ToList();
        project.DrainageStrategies = (await GetDrainageStrategies(project.Id)).ToList();
        project.Surfs = (await GetSurfs(project.Id)).ToList();
        project.Substructures = (await GetSubstructures(project.Id)).ToList();
        project.Topsides = (await GetTopsides(project.Id)).ToList();
        project.Transports = (await GetTransports(project.Id)).ToList();
        project.Explorations = (await GetExplorations(project.Id)).ToList();
        project.Wells = (await GetWells(project.Id)).ToList();

        return project;
    }

    public async Task<IEnumerable<Well>> GetWells(Guid projectId)
    {
        if (_context.Wells != null)
        {
            return await _context.Wells
                .Where(d => d.ProjectId.Equals(projectId)).ToListAsync();
        }
        else
        {
            return new List<Well>();
        }
    }

    public async Task<IEnumerable<Exploration>> GetExplorations(Guid projectId)
    {
        if (_context.Explorations != null)
        {
            return await _context.Explorations
                .Include(c => c.ExplorationWellCostProfile)
                .Include(c => c.AppraisalWellCostProfile)
                .Include(c => c.SidetrackCostProfile)
                .Include(c => c.GAndGAdminCost)
                .Include(c => c.SeismicAcquisitionAndProcessing)
                .Include(c => c.CountryOfficeCost)
                .Include(c => c.ExplorationWells!).ThenInclude(ew => ew.DrillingSchedule)
                .Where(d => d.Project.Id.Equals(projectId)).ToListAsync();
        }
        else
        {
            return new List<Exploration>();
        }
    }

    public async Task<IEnumerable<Transport>> GetTransports(Guid projectId)
    {
        if (_context.Transports != null)
        {
            return await _context.Transports
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .Where(c => c.Project.Id.Equals(projectId)).ToListAsync();
        }
        else
        {
            return new List<Transport>();
        }
    }

    public async Task<IEnumerable<Topside>> GetTopsides(Guid projectId)
    {
        if (_context.Topsides != null)
        {
            return await _context.Topsides
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .Where(c => c.Project.Id.Equals(projectId)).ToListAsync();
        }
        else
        {
            return new List<Topside>();
        }
    }

    public async Task<IEnumerable<Surf>> GetSurfs(Guid projectId)
    {
        if (_context.Surfs != null)
        {
            return await _context.Surfs
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .Where(c => c.Project.Id.Equals(projectId)).ToListAsync();
        }
        else
        {
            return new List<Surf>();
        }
    }

    public async Task<IEnumerable<DrainageStrategy>> GetDrainageStrategies(Guid projectId)
    {
        if (_context.DrainageStrategies != null)
        {
            return await _context.DrainageStrategies
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
                .Where(d => d.Project.Id.Equals(projectId)).ToListAsync();
        }
        else
        {
            return new List<DrainageStrategy>();
        }
    }

    public async Task<IEnumerable<WellProject>> GetWellProjects(Guid projectId)
    {
        if (_context.WellProjects != null)
        {
            return await _context.WellProjects
                .Include(c => c.OilProducerCostProfile)
                .Include(c => c.OilProducerCostProfileOverride)
                .Include(c => c.GasProducerCostProfile)
                .Include(c => c.GasProducerCostProfileOverride)
                .Include(c => c.WaterInjectorCostProfile)
                .Include(c => c.WaterInjectorCostProfileOverride)
                .Include(c => c.GasInjectorCostProfile)
                .Include(c => c.GasInjectorCostProfileOverride)
                .Include(c => c.WellProjectWells!).ThenInclude(wpw => wpw.DrillingSchedule)
                .Where(d => d.Project.Id.Equals(projectId)).ToListAsync();
        }
        else
        {
            return new List<WellProject>();
        }
    }

    public async Task<IEnumerable<Substructure>> GetSubstructures(Guid projectId)
    {
        if (_context.Substructures != null)
        {
            return await _context.Substructures
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .Where(c => c.Project.Id.Equals(projectId)).ToListAsync();
        }
        else
        {
            return new List<Substructure>();
        }
    }
}
