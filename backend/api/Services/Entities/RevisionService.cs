using System.Collections;
using System.Diagnostics;
using System.Reflection;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Mappings;
using api.Models;
using api.Repositories;
using api.Services.FusionIntegration;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

namespace api.Services;

public class RevisionService : IRevisionService
{
    private readonly ILogger<RevisionService> _logger;
    private readonly IRevisionRepository _revisionRepository;
    private readonly IProjectAccessService _projectAccessService;


    public RevisionService(
        ILoggerFactory loggerFactory,
        IRevisionRepository revisionRepository,
        IProjectAccessService projectAccessService
    )
    {
        _logger = loggerFactory.CreateLogger<RevisionService>();
        _revisionRepository = revisionRepository;
        _projectAccessService = projectAccessService;
    }

    public async Task<string> CreateRevision(Guid projectId)
    {
        var project = await _revisionRepository.GetProjectAndAssetsNoTracking(projectId);

        if (project != null)
        {
            var caseItem = project.Cases?.FirstOrDefault();
            Console.WriteLine("CaseItem: " + caseItem?.Name);
        }

        if (project == null)
        {
            throw new NotFoundInDBException($"Project with id {projectId} not found.");
        }

        SetProjectAndRelatedEntitiesToEmptyGuids(project);
        if (project == null)
        {
            throw new NotFoundInDBException($"Project with id {projectId} not found.");
        }
        project.OriginalProjectId = projectId;
        project.IsRevision = true;

        var revision = await _revisionRepository.AddRevision(project);

        return JsonConvert.SerializeObject(revision, Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });

        // return revision;
    }

    private void SetProjectAndRelatedEntitiesToEmptyGuids(Project project)
    {
        project.Id = Guid.Empty;

        project.DevelopmentOperationalWellCosts!.Id = Guid.Empty;
        project.ExplorationOperationalWellCosts!.Id = Guid.Empty;

        var drainageStrategies = new List<DrainageStrategy>();
        var topsides = new List<Topside>();
        var substructures = new List<Substructure>();
        var surf = new List<Surf>();
        var transport = new List<Transport>();
        var wellProjects = new List<WellProject>();
        var exploration = new List<Exploration>();


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
                surf.Add(caseItem.Surf);
            }

            if (caseItem.Transport != null)
            {
                caseItem.Transport.Id = Guid.Empty;
                SetIdsToEmptyGuids(caseItem.Transport);
                transport.Add(caseItem.Transport);
            }

            if (caseItem.WellProject != null)
            {
                caseItem.WellProject.Id = Guid.Empty;
                SetIdsToEmptyGuids(caseItem.WellProject);
                wellProjects.Add(caseItem.WellProject);
            }

            if (caseItem.Exploration != null)
            {
                caseItem.Exploration.Id = Guid.Empty;
                SetIdsToEmptyGuids(caseItem.Exploration);
                exploration.Add(caseItem.Exploration);
            }


        }

        project.DrainageStrategies = drainageStrategies;
        project.Topsides = topsides;
        project.Substructures = substructures;
        project.Surfs = surf;
        project.Transports = transport;
        project.WellProjects = wellProjects;
        project.Explorations = exploration;


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

        Console.WriteLine("Type: " + type.Name);

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;

            if (propertyType == typeof(Project))
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
