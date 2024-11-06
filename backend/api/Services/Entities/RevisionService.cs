using System.Collections;
using System.Diagnostics;
using System.Reflection;

using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace api.Services;

public class RevisionService(
    DcdDbContext context,
    IRevisionRepository revisionRepository,
    IMapper mapper,
    IProjectRepository projectRepository)
    : IRevisionService
{
    // TODO: Rewrite when CaseWithAssetsDto is no longer needed
    public async Task<Project> GetProjectWithCasesAndAssets(Guid projectId)
    {
        Project project = await context.Projects
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
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .FirstOrDefaultAsync(p => (p.Id.Equals(projectId) || p.FusionProjectId.Equals(projectId)) && p.IsRevision)
            ?? throw new NotFoundInDBException($"Project with id {projectId} not found.");

        if (project.Cases?.Count > 0)
        {
            project.Cases = project.Cases.OrderBy(c => c.CreateTime).ToList();
        }

        await AddAssetsToProject(project);
        return project;
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

    public async Task<ProjectWithAssetsDto> GetRevision(Guid projectId)
    {
        DateTimeOffset projectLastUpdated;
        var project = await GetProjectWithCasesAndAssets(projectId);
        if (project.Cases?.Count > 0)
        {
            projectLastUpdated = new[] { project.ModifyTime }.Concat(project.Cases.Select(c => c.ModifyTime)).Max();
        }
        else
        {
            projectLastUpdated = project.ModifyTime;
        }

        var destination = mapper.Map<Project, ProjectWithAssetsDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

        var projectDto = destination;

        if (projectDto == null)
        {
            throw new NotFoundInDBException(string.Format("Project {0} not found", projectId));
        }

        projectDto.ModifyTime = projectLastUpdated;

        Activity.Current?.AddBaggage(nameof(projectDto), JsonConvert.SerializeObject(projectDto, Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));
        return projectDto;
    }

    public async Task<ProjectWithAssetsDto> CreateRevision(Guid projectId, CreateRevisionDto createRevisionDto)
    {
        var project = await revisionRepository.GetProjectAndAssetsNoTracking(projectId)
            ?? throw new NotFoundInDBException($"Project with id {projectId} not found.");

        SetProjectAndRelatedEntitiesToEmptyGuids(project, projectId, createRevisionDto);
        await UpdateProjectWithRevisionChanges(projectId, createRevisionDto);

        var revisionDetails = new RevisionDetails
        {
            OriginalProjectId = projectId,
            RevisionName = createRevisionDto.Name,
            Mdqc = createRevisionDto.Mdqc,
            Arena = createRevisionDto.Arena,
            RevisionDate = DateTimeOffset.UtcNow,
            Revision = project,
        };

        project.RevisionDetails = revisionDetails;

        var revision = await revisionRepository.AddRevision(project);

        var revisionDto = mapper.Map<Project, ProjectWithAssetsDto>(revision, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

        return revisionDto;
    }

    private async Task<Project> UpdateProjectWithRevisionChanges(Guid projectId, CreateRevisionDto createRevisionDto)
    {
        var existingProject = await projectRepository.GetProject(projectId)
            ?? throw new NotFoundInDBException($"Project {projectId} not found");

        existingProject.InternalProjectPhase = createRevisionDto.InternalProjectPhase;
        existingProject.Classification = createRevisionDto.Classification;

        return existingProject;
    }

    private void SetProjectAndRelatedEntitiesToEmptyGuids(Project project, Guid originalProjectId, CreateRevisionDto createRevisionDto)
    {
        project.Id = Guid.Empty;

        project.IsRevision = true;
        project.OriginalProjectId = originalProjectId;

        project.InternalProjectPhase = createRevisionDto.InternalProjectPhase;
        project.Classification = createRevisionDto.Classification;

        if (project.DevelopmentOperationalWellCosts != null)
        {
            project.DevelopmentOperationalWellCosts.Id = Guid.Empty;
        }
        if (project.ExplorationOperationalWellCosts != null)
        {
            project.ExplorationOperationalWellCosts.Id = Guid.Empty;
        }

        var drainageStrategies = new List<DrainageStrategy>();
        var topsides = new List<Topside>();
        var substructures = new List<Substructure>();
        var surfs = new List<Surf>();
        var transports = new List<Transport>();
        var wellProjects = new List<WellProject>();
        var explorations = new List<Exploration>();
        var wells = new List<Well>();


        if (project.Cases == null)
        {
            return;
        }

        foreach (var caseItem in project.Cases)
        {
            caseItem.Id = Guid.Empty;
            caseItem.ProjectId = Guid.Empty;

            if (caseItem.CessationOffshoreFacilitiesCost != null)
            {
                caseItem.CessationOffshoreFacilitiesCost.Id = Guid.Empty;
                SetIdsToEmptyGuids(caseItem.CessationOffshoreFacilitiesCost);
            }

            if (caseItem.WellInterventionCostProfile != null)
            {
                caseItem.WellInterventionCostProfile.Id = Guid.Empty;
                SetIdsToEmptyGuids(caseItem.WellInterventionCostProfile);
            }

            if (caseItem.DrainageStrategy != null)
            {
                caseItem.DrainageStrategy.Id = Guid.Empty;
                SetIdsToEmptyGuids(caseItem.DrainageStrategy);
                drainageStrategies.Add(caseItem.DrainageStrategy);
            }

            if (caseItem.Topside != null)
            {
                caseItem.Topside.Id = Guid.Empty;
                SetIdsToEmptyGuids(caseItem.Topside);
                topsides.Add(caseItem.Topside);
            }

            if (caseItem.Substructure != null)
            {
                caseItem.Substructure.Id = Guid.Empty;
                SetIdsToEmptyGuids(caseItem.Substructure);
                substructures.Add(caseItem.Substructure);
            }

            if (caseItem.Surf != null)
            {
                caseItem.Surf.Id = Guid.Empty;
                SetIdsToEmptyGuids(caseItem.Surf);
                surfs.Add(caseItem.Surf);
            }

            if (caseItem.Transport != null)
            {
                caseItem.Transport.Id = Guid.Empty;
                SetIdsToEmptyGuids(caseItem.Transport);
                transports.Add(caseItem.Transport);
            }

            if (caseItem.WellProject != null)
            {
                caseItem.WellProject.Id = Guid.Empty;
                var wpw = caseItem.WellProject.WellProjectWells?.Select(wp => wp.Well).ToList();
                if (wpw != null)
                {
                    foreach (var well in wpw)
                    {
                        well.Id = Guid.Empty;
                        wells.Add(well);
                    }
                }
                SetIdsToEmptyGuids(caseItem.WellProject);
                wellProjects.Add(caseItem.WellProject);
            }

            if (caseItem.Exploration != null)
            {
                caseItem.Exploration.Id = Guid.Empty;
                var ew = caseItem.Exploration.ExplorationWells?.Select(ew => ew.Well).ToList();
                if (ew != null)
                {
                    foreach (var well in ew)
                    {
                        well.Id = Guid.Empty;
                        wells.Add(well);
                    }
                }
                SetIdsToEmptyGuids(caseItem.Exploration);
                explorations.Add(caseItem.Exploration);
            }
        }

        project.DrainageStrategies = drainageStrategies;
        project.Topsides = topsides;
        project.Substructures = substructures;
        project.Surfs = surfs;
        project.Transports = transports;
        project.WellProjects = wellProjects;
        project.Explorations = explorations;
        project.Wells = wells;
    }

    private static void SetIdsToEmptyGuids(object? obj)
    {
        SetIdsToEmptyGuids(obj, new HashSet<object>());
    }

    /// <summary>
    /// Recursively sets all properties ending with "Id" of type <see cref="Guid"/> to <see cref="Guid.Empty"/>
    /// in the given object and its child objects.
    /// </summary>
    /// <param name="obj">The object whose properties are to be modified.</param>
    /// <param name="visited">A set of already visited objects to avoid infinite recursion.</param>
    private static void SetIdsToEmptyGuids(object? obj, HashSet<object> visited)
    {
        if (obj == null || visited.Contains(obj))
        {
            return;
        }

        visited.Add(obj);

        var type = obj.GetType();

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;

            if (propertyType == typeof(Project) || propertyType == typeof(Well))
            {
                continue;
            }

            if (propertyType == typeof(Guid) && (property.Name.EndsWith("Id") || property.Name.EndsWith("Link")))
            {
                property.SetValue(obj, Guid.Empty);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string))
            {
                var childObject = property.GetValue(obj);
                if (childObject is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        SetIdsToEmptyGuids(item, visited);
                    }
                }
            }
            else if (propertyType.IsClass && propertyType != typeof(string))
            {
                var childObject = property.GetValue(obj);
                SetIdsToEmptyGuids(childObject, visited);
            }
        }
    }
}
