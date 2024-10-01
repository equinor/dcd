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

        SetIdsToEmptyGuids(project);
        if (project == null)
        {
            throw new NotFoundInDBException($"Project with id {projectId} not found.");
        }
        project.OriginalProjectId = projectId;

        var revision = await _revisionRepository.AddRevision(project);

        return JsonConvert.SerializeObject(revision, Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });

        // return revision;
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
        if (obj == null || visited.Contains(obj)) { return; }

        visited.Add(obj);

        var type = obj.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);


        Console.WriteLine("---------------------------------------------------------");
        Console.WriteLine("Type: " + type.Name);
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(Project) || property.PropertyType ==  typeof(ICollection<Project>))
            {
                continue;
            }
            if (property.PropertyType == typeof(ICollection<Case>)) {
                Console.WriteLine("Property is ICollection<Case>");
                Console.WriteLine("Result: " + property.PropertyType.IsClass);
                Console.WriteLine("Result: " + property.PropertyType.IsCollectible);
                Console.WriteLine("Result: " + property.PropertyType.IsEnum);
                Console.WriteLine("Result: " + property.PropertyType.IsInterface);
            }
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                Console.WriteLine("Property is IEnumerable");
                var childObject = property.GetValue(obj);
                if (childObject is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        SetIdsToEmptyGuids(item, visited);
                    }
                }
            }
            Console.WriteLine("Property: " + property.Name);
            if (property.PropertyType == typeof(Guid) && property.Name.EndsWith("Id"))
            {
                Console.WriteLine("Setting property to empty guid");
                property.SetValue(obj, Guid.Empty);
            }
            else if ((property.PropertyType.IsClass || property.PropertyType.IsCollectible) && property.PropertyType != typeof(string))
            {
                var childObject = property.GetValue(obj);
                Console.WriteLine("childObject is enumerable: " + (childObject is IEnumerable));
                if (childObject is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        SetIdsToEmptyGuids(item, visited);
                    }
                }
                else
                {
                    SetIdsToEmptyGuids(childObject, visited);
                }
            }
        }
    }
}
