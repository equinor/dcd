using System.Collections;
using System.Reflection;

using api.Models;

namespace api.Features.Revision.Create;

public static class ResetIdPropertiesInProjectGraphService
{
    private const string ProjectNamespacePrefix = "api.";
    private static readonly string[] ProjectEntityNames = ["Project", "ProjectProxy"];

    public static void ResetPrimaryKeysAndForeignKeysInGraph(Project project)
    {
        project.Id = Guid.NewGuid();

        SetIdsToEmptyGuids(project, project.Id, []);
    }

    private static void SetIdsToEmptyGuids(object? obj, Guid projectId, HashSet<object> visited)
    {
        if (obj == null || !visited.Add(obj))
        {
            return;
        }

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var guidProperties = properties
            .Where(x => x.PropertyType == typeof(Guid))
            .ToList();

        var listProperties = properties
            .Where(x => x.PropertyType.IsGenericType &&
                        x.PropertyType.GetGenericArguments().Any(y => y.Namespace?.StartsWith(ProjectNamespacePrefix) == true))
            .ToList();

        var instanceProperties = properties
            .Where(x => x.PropertyType.Namespace?.StartsWith(ProjectNamespacePrefix) == true)
            .ToList();

        foreach (var guidProperty in guidProperties)
        {
            if (guidProperty.Name.EndsWith("Id") || guidProperty.Name.EndsWith("Link"))
            {
                guidProperty.SetValue(obj, Guid.Empty);
            }

            if (ProjectEntityNames.Contains(obj.GetType().Name) && guidProperty.Name == "Id")
            {
                guidProperty.SetValue(obj, projectId);
            }

            if (guidProperty.Name == "ProjectId")
            {
                guidProperty.SetValue(obj, projectId);
            }
        }

        foreach (var instanceProperty in instanceProperties)
        {
            var childObject = instanceProperty.GetValue(obj);
            SetIdsToEmptyGuids(childObject, projectId, visited);
        }

        foreach (var listProperty in listProperties)
        {
            var childObject = listProperty.GetValue(obj);

            if (childObject is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    SetIdsToEmptyGuids(item, projectId, visited);
                }
            }
        }
    }
}
