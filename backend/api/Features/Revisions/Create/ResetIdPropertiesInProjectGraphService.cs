using System.Collections;
using System.Reflection;

using api.Models;

namespace api.Features.Revisions.Create;

public static class ResetIdPropertiesInProjectGraphService
{
    private const string ProjectNamespacePrefix = "api.";
    private const string ProjectEntityName = "Project";
    private const string CaseEntityName = "Case";
    private static readonly string[] PropertiesToIgnore = ["CommonLibraryId", "FusionProjectId"];

    public static void ResetPrimaryKeysAndForeignKeysInGraph(Project project, Dictionary<Guid, Guid> caseIdMapping)
    {
        project.Id = Guid.NewGuid();

        SetIdsToEmptyGuids(project, project.Id, [], caseIdMapping);
    }

    private static void SetIdsToEmptyGuids(object? obj, Guid projectId, HashSet<object> visited, Dictionary<Guid, Guid> caseIdMapping)
    {
        if (obj == null || !visited.Add(obj))
        {
            return;
        }

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var guidProperties = properties
            .Where(x => x.PropertyType == typeof(Guid))
            .Where(x => !PropertiesToIgnore.Contains(x.Name))
            .ToList();

        var listProperties = properties
            .Where(x => x.PropertyType.IsGenericType)
            .Where(x => x.PropertyType.GetGenericArguments().Any(y => y.Namespace?.StartsWith(ProjectNamespacePrefix) == true))
            .ToList();

        var instanceProperties = properties
            .Where(x => x.PropertyType.Namespace?.StartsWith(ProjectNamespacePrefix) == true)
            .ToList();

        foreach (var guidProperty in guidProperties)
        {
            if (CaseEntityName == obj.GetType().Name && guidProperty.Name == "Id")
            {
                var caseId = (Guid)guidProperty.GetValue(obj)!;
                guidProperty.SetValue(obj, caseIdMapping[caseId]);
                continue;
            }

            if (guidProperty.Name == "CaseId")
            {
                var caseId = (Guid)guidProperty.GetValue(obj)!;
                guidProperty.SetValue(obj, caseIdMapping[caseId]);
                continue;
            }

            if (guidProperty.Name == "ProjectId")
            {
                guidProperty.SetValue(obj, projectId);
                continue;
            }

            if (ProjectEntityName == obj.GetType().Name && guidProperty.Name == "Id")
            {
                continue;
            }

            if (guidProperty.Name.EndsWith("Id"))
            {
                guidProperty.SetValue(obj, Guid.Empty);
            }
        }

        foreach (var instanceProperty in instanceProperties)
        {
            var childObject = instanceProperty.GetValue(obj);
            SetIdsToEmptyGuids(childObject, projectId, visited, caseIdMapping);
        }

        foreach (var listProperty in listProperties)
        {
            var childObject = listProperty.GetValue(obj);

            if (childObject is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    SetIdsToEmptyGuids(item, projectId, visited, caseIdMapping);
                }
            }
        }
    }
}
