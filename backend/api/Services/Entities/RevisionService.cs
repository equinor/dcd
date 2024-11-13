using System.Collections;
using System.Reflection;

using api.Context;
using api.Dtos.Project.Revision;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;


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
            .Include(p => p.Cases)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .FirstOrDefaultAsync(p => (p.Id.Equals(projectId) || p.FusionProjectId.Equals(projectId)) && p.IsRevision)
            ?? throw new NotFoundInDBException($"Project with id {projectId} not found.");

        if (project.Cases?.Count > 0)
        {
            project.Cases = project.Cases.OrderBy(c => c.CreateTime).ToList();
        }

        return project;
    }

    private static DateTimeOffset GetLatestModifyTime(Project project)
    {
        return project.Cases?.Select(c => c.ModifyTime)
            .Append(project.ModifyTime)
            .Max() ?? project.ModifyTime;
    }

    public async Task<RevisionWithCasesDto> GetRevision(Guid projectId)
    {
        var project = await GetProjectWithCasesAndAssets(projectId)
            ?? throw new NotFoundInDBException(string.Format("Project {0} not found", projectId));

        var projectDto = mapper.Map<Project, RevisionWithCasesDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

        projectDto.ModifyTime = GetLatestModifyTime(project);

        return projectDto;
    }

    public async Task<RevisionWithCasesDto> CreateRevision(Guid projectId, CreateRevisionDto createRevisionDto)
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
            Classification = createRevisionDto.Classification
        };

        project.RevisionDetails = revisionDetails;

        var revision = await revisionRepository.AddRevision(project);

        var revisionDto = mapper.Map<Project, RevisionWithCasesDto>(revision, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

        revisionDto.ModifyTime = GetLatestModifyTime(revision);

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

    public async Task<RevisionWithCasesDto> UpdateRevision(Guid projectId, Guid revisionId, UpdateRevisionDto updateRevisionDto)
    {
        var revision = context.Projects
                        .Include(p => p.Cases)
                        .Include(p => p.RevisionDetails)
                        .FirstOrDefault(r => r.Id == revisionId)
                    ?? throw new NotFoundInDBException($"Revision with id {revisionId} not found.");

        if (revision.RevisionDetails == null)
        {
            throw new InvalidOperationException("RevisionDetails cannot be null.");
        }

        revision.RevisionDetails.RevisionName = updateRevisionDto.Name;
        revision.RevisionDetails.Arena = updateRevisionDto.Arena;
        revision.RevisionDetails.Mdqc = updateRevisionDto.Mdqc;

        await context.SaveChangesAsync();

        var revisionDto = mapper.Map<Project, RevisionWithCasesDto>(revision, opts => opts.Items["ConversionUnit"] = revision.PhysicalUnit.ToString());

        revisionDto.ModifyTime = GetLatestModifyTime(revision);

        return revisionDto;
    }

    private static void SetProjectAndRelatedEntitiesToEmptyGuids(Project project, Guid originalProjectId, CreateRevisionDto createRevisionDto)
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
