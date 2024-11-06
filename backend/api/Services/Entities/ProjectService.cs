using System.Diagnostics;

using api.Context;
using api.Dtos;
using api.Dtos.Project.Revision;
using api.Exceptions;
using api.Helpers;
using api.Models;
using api.Repositories;
using api.Services.FusionIntegration;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace api.Services;

public class ProjectService(
    DcdDbContext context,
    ILogger<ProjectService> logger,
    IMapper mapper,
    IProjectRepository projectRepository,
    IMapperService mapperService,
    IFusionService fusionService)
    : IProjectService
{
    public async Task<ProjectWithCasesDto> UpdateProject(Guid projectId, UpdateProjectDto projectDto)
    {
        var existingProject = await projectRepository.GetProjectWithCases(projectId)
            ?? throw new NotFoundInDBException($"Project {projectId} not found");

        existingProject.ModifyTime = DateTimeOffset.UtcNow;

        mapperService.MapToEntity(projectDto, existingProject, projectId);

        try
        {
            await projectRepository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Failed to update project {projectId}", projectId);
            throw;
        }

        var dto = mapperService.MapToDto<Project, ProjectWithCasesDto>(existingProject, projectId);
        return dto;
    }

    public async Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts(
        Guid projectId,
        Guid explorationOperationalWellCostsId,
        UpdateExplorationOperationalWellCostsDto dto
    )
    {
        var existingExplorationOperationalWellCosts = await projectRepository.GetExplorationOperationalWellCosts(explorationOperationalWellCostsId)
            ?? throw new NotFoundInDBException($"ExplorationOperationalWellCosts {explorationOperationalWellCostsId} not found");

        mapperService.MapToEntity(dto, existingExplorationOperationalWellCosts, explorationOperationalWellCostsId);

        try
        {
            await projectRepository.UpdateModifyTime(projectId);
            await projectRepository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Failed to update exploration operational well costs {explorationOperationalWellCostsId}", explorationOperationalWellCostsId);
            throw;
        }

        var returnDto = mapperService.MapToDto<ExplorationOperationalWellCosts, ExplorationOperationalWellCostsDto>(existingExplorationOperationalWellCosts, explorationOperationalWellCostsId);
        return returnDto;
    }

    public async Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts(
        Guid projectId,
        Guid developmentOperationalWellCostsId,
        UpdateDevelopmentOperationalWellCostsDto dto
    )
    {
        var existingDevelopmentOperationalWellCosts = await projectRepository.GetDevelopmentOperationalWellCosts(developmentOperationalWellCostsId)
            ?? throw new NotFoundInDBException($"DevelopmentOperationalWellCosts {developmentOperationalWellCostsId} not found");

        mapperService.MapToEntity(dto, existingDevelopmentOperationalWellCosts, developmentOperationalWellCostsId);

        try
        {
            await projectRepository.UpdateModifyTime(projectId);
            await projectRepository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Failed to update development operational well costs {developmentOperationalWellCostsId}", developmentOperationalWellCostsId);
            throw;
        }

        var returnDto = mapperService.MapToDto<DevelopmentOperationalWellCosts, DevelopmentOperationalWellCostsDto>(existingDevelopmentOperationalWellCosts, developmentOperationalWellCostsId);
        return returnDto;
    }

    public async Task<ProjectWithAssetsDto> UpdateProjectFromProjectMaster(ProjectWithAssetsDto projectDto)
    {
        var existingProject = await GetProjectWithCasesAndAssets(projectDto.Id);

        mapper.Map(projectDto, existingProject);

        context.Projects!.Update(existingProject);
        await context.SaveChangesAsync();
        return await GetProjectDto(existingProject.Id);
    }

    public async Task<ProjectWithAssetsDto> CreateProject(Guid contextId)
    {
        var projectMaster = await fusionService.GetProjectMasterFromFusionContextId(contextId);

        if (projectMaster == null)
        {
            throw new KeyNotFoundException($"Project with context ID {contextId} not found in the external API.");
        }

        // Check if a project with the same external ID already exists
        var existingProject = await projectRepository.GetProjectByExternalId(projectMaster.Identity);

        if (existingProject != null)
        {
            throw new ProjectAlreadyExistsException($"Project with externalId {projectMaster.Identity} already exists");
        }

        var project = new Project();

        mapperService.MapToEntity(projectMaster, project, Guid.Empty);

        project.CreateDate = DateTimeOffset.UtcNow;
        project.Cases = new List<Case>();
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
        project.FlaredGasPerProducedVolume = 1.13;
        project.CO2EmissionsFromFlaredGas = 3.74;
        project.CO2Vented = 1.96;
        project.DailyEmissionFromDrillingRig = 100;
        project.AverageDevelopmentDrillingDays = 50;

        context.Projects.Add(project);

        await context.SaveChangesAsync();
        return await GetProjectDto(project.Id);
    }

    public async Task<IEnumerable<Project>> GetAll()
    {
        Activity.Current?.AddBaggage(nameof(context.Projects), JsonConvert.SerializeObject(context.Projects,
            Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));
        if (context.Projects != null)
        {
            var projects = context.Projects
                .Include(c => c.Cases);

            foreach (var project in projects)
            {
                await AddAssetsToProject(project);
            }

            return projects;
        }

        return new List<Project>();
    }

    public async Task<IEnumerable<ProjectWithAssetsDto>> GetAllDtos()
    {
        var projects = await GetAll();
        if (projects != null)
        {
            var projectDtos = new List<ProjectWithAssetsDto>();
            foreach (var project in projects)
            {
                var projectDto = mapper.Map<ProjectWithAssetsDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());
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

        return new List<ProjectWithAssetsDto>();
    }

    public async Task<Project> GetProjectWithoutAssets(Guid projectId)
    {
        if (context.Projects != null)
        {
            if (projectId == Guid.Empty)
            {
                throw new NotFoundInDBException($"Project {projectId} not found");
            }

            var project = await context.Projects!
                .Include(p => p.Cases)
                .Include(p => p.Wells)
                .Include(p => p.ExplorationOperationalWellCosts)
                .Include(p => p.DevelopmentOperationalWellCosts)
                .FirstOrDefaultAsync(p => p.Id.Equals(projectId) || p.FusionProjectId.Equals(projectId));

            if (project == null)
            {
                throw new NotFoundInDBException($"Project {projectId} not found");
            }

            return project;
        }

        logger.LogError(new NotFoundInDBException("The database contains no projects"), "no projects");
        throw new NotFoundInDBException("The database contains no projects");
    }

    public async Task<Project> GetProjectWithoutAssetsNoTracking(Guid projectId)
    {
        if (context.Projects != null)
        {
            if (projectId == Guid.Empty)
            {
                throw new NotFoundInDBException($"Project {projectId} not found");
            }

            var project = await context.Projects
                .AsNoTracking()
                .Include(p => p.Cases)
                .Include(p => p.Wells)
                .Include(p => p.ExplorationOperationalWellCosts)
                .Include(p => p.DevelopmentOperationalWellCosts)
                .FirstOrDefaultAsync(p => p.Id.Equals(projectId) || p.FusionProjectId.Equals(projectId));

            if (project == null)
            {
                throw new NotFoundInDBException($"Project {projectId} not found");
            }

            return project;
        }

        logger.LogError(new NotFoundInDBException("The database contains no projects"), "no projects");
        throw new NotFoundInDBException("The database contains no projects");
    }

    public async Task<Project> GetProject(Guid projectId)
    {
        return await projectRepository.GetProject(projectId)
            ?? throw new NotFoundInDBException($"Project {projectId} not found");
    }

    public async Task<Project> GetProjectWithCasesAndAssets(Guid projectId)
    {

        if (projectId == Guid.Empty)
        {
            throw new NotFoundInDBException($"Project {projectId} not found");
        }

        var project = await context.Projects
            .Include(p => p.Cases)!.ThenInclude(c => c.TotalFeasibilityAndConceptStudies)
            .Include(p => p.Cases)!.ThenInclude(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(p => p.Cases)!.ThenInclude(c => c.TotalFEEDStudies)
            .Include(p => p.Cases)!.ThenInclude(c => c.TotalFEEDStudiesOverride)
            .Include(p => p.Cases)!.ThenInclude(c => c.TotalOtherStudiesCostProfile)
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
            .Include(p => p.Cases)!.ThenInclude(c => c.CalculatedTotalIncomeCostProfile)
            .Include(p => p.Cases)!.ThenInclude(c => c.CalculatedTotalCostCostProfile)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .FirstOrDefaultAsync(p => (p.Id.Equals(projectId) || p.FusionProjectId.Equals(projectId)) && !p.IsRevision);

        if (project?.Cases?.Count > 0)
        {
            project.Cases = project.Cases.OrderBy(c => c.CreateTime).ToList();
        }

        if (project == null)
        {
            throw new NotFoundInDBException($"Project {projectId} not found");
        }

        Activity.Current?.AddBaggage(nameof(projectId), JsonConvert.SerializeObject(projectId, Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));
        await AddAssetsToProject(project);
        return project;
    }

    public async Task<ProjectWithAssetsDto> GetProjectDto(Guid projectId)
    {
        var project = await GetProjectWithCasesAndAssets(projectId);

        DateTimeOffset projectLastUpdated;
        if (project.Cases?.Count > 0)
        {
            projectLastUpdated = new[] { project.ModifyTime }.Concat(project.Cases.Select(c => c.ModifyTime)).Max();
        }
        else
        {
            projectLastUpdated = project.ModifyTime;
        }
        var revisionDetails = context.RevisionDetails.Where(r => r.OriginalProjectId == project.Id).ToList();

        var destination = mapper.Map<Project, ProjectWithAssetsDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

        destination.RevisionsDetailsList = mapper.Map<List<RevisionDetailsDto>>(revisionDetails);

        var projectDto = destination;

        if (projectDto == null)
        {
            throw new NotFoundInDBException($"Project {projectId} not found");
        }

        projectDto.ModifyTime = projectLastUpdated;

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
            if (projectMaster == null)
            {
                logger.LogWarning("ProjectMaster not found for project {projectName} ({projectId})", project.Name,
                    project.Id);
                continue;
            }
            if (!project.Equals(projectMaster))
            {
                logger.LogWarning("Project {projectName} ({projectId}) differs from ProjectMaster", project.Name,
                    project.Id);
                numberOfDeviations++;
                await UpdateProjectFromProjectMaster(projectMaster);
            }
            else
            {
                logger.LogInformation("Project {projectName} ({projectId}) is identical to ProjectMaster",
                    project.Name, project.Id);
            }
        }

        logger.LogInformation("Number of projects which differs from ProjectMaster: {count} / {total}",
            numberOfDeviations, totalNumberOfProjects);
    }

    private async Task<ProjectWithAssetsDto?> GetProjectDtoFromProjectMaster(Guid projectGuid)
    {

        var projectMaster = await fusionService.GetProjectMasterFromFusionContextId(projectGuid);

        if (projectMaster == null)
        {
            return null;
        }

        ProjectCategory category;
        ProjectPhase phase;

        try
        {
            category = CommonLibraryHelper.ConvertCategory(projectMaster.ProjectCategory ?? "");
            phase = CommonLibraryHelper.ConvertPhase(projectMaster.Phase ?? "");
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Invalid category or phase for project with ID {ProjectId}", projectGuid);
            return null;
        }

        ProjectWithAssetsDto projectDto = new()
        {
            Name = projectMaster.Description ?? "",
            CommonLibraryName = projectMaster.Description ?? "",
            FusionProjectId = projectMaster.Identity,
            Country = projectMaster.Country ?? "",
            Currency = Currency.NOK,
            PhysicalUnit = PhysUnit.SI,
            Id = projectMaster.Identity,
            ProjectCategory = category,
            ProjectPhase = phase,
        };
        return projectDto;
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
        return await context.Wells
            .Where(d => d.ProjectId.Equals(projectId)).ToListAsync();

    }

    public async Task<IEnumerable<Exploration>> GetExplorations(Guid projectId)
    {
        return await context.Explorations
            .Include(c => c.ExplorationWellCostProfile)
            .Include(c => c.AppraisalWellCostProfile)
            .Include(c => c.SidetrackCostProfile)
            .Include(c => c.GAndGAdminCost)
            .Include(c => c.GAndGAdminCostOverride)
            .Include(c => c.SeismicAcquisitionAndProcessing)
            .Include(c => c.CountryOfficeCost)
            .Include(c => c.ExplorationWells!).ThenInclude(ew => ew.DrillingSchedule)
            .Where(d => d.Project.Id.Equals(projectId)).ToListAsync();
    }

    public async Task<IEnumerable<Transport>> GetTransports(Guid projectId)
    {
        return await context.Transports
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(c => c.Project.Id.Equals(projectId)).ToListAsync();
    }

    public async Task<IEnumerable<Topside>> GetTopsides(Guid projectId)
    {
        return await context.Topsides
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(c => c.Project.Id.Equals(projectId)).ToListAsync();
    }

    public async Task<IEnumerable<Surf>> GetSurfs(Guid projectId)
    {
        return await context.Surfs
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(c => c.Project.Id.Equals(projectId)).ToListAsync();
    }

    public async Task<IEnumerable<DrainageStrategy>> GetDrainageStrategies(Guid projectId)
    {
        return await context.DrainageStrategies
            .Include(c => c.ProductionProfileOil)
            .Include(c => c.AdditionalProductionProfileOil)
            .Include(c => c.ProductionProfileGas)
            .Include(c => c.AdditionalProductionProfileGas)
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
            .Include(c => c.DeferredOilProduction)
            .Include(c => c.DeferredGasProduction)
            .Where(d => d.Project.Id.Equals(projectId)).ToListAsync();
    }

    public async Task<IEnumerable<WellProject>> GetWellProjects(Guid projectId)
    {
        return await context.WellProjects
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

    public async Task<IEnumerable<Substructure>> GetSubstructures(Guid projectId)
    {
        return await context.Substructures
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(c => c.Project.Id.Equals(projectId)).ToListAsync();
    }
}
