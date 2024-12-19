using System.Collections;
using System.Reflection;

using api.Models;

namespace api.Features.Cases.Duplicate;

public static class ResetIdPropertiesInCaseGraphService
{
    private const string ProjectNamespacePrefix = "api.";
    private static readonly string[] ProjectEntityNames = ["Case", "CaseProxy"];

    public static void ResetPrimaryKeysAndForeignKeysInGraph(Case caseItem, Guid duplicateCaseId)
    {
        caseItem.Id = duplicateCaseId;

        SetIdsToEmptyGuids(caseItem, caseItem.Id, []);
    }

    private static void SetIdsToEmptyGuids(object? obj, Guid caseId, HashSet<object> visited)
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
            if (guidProperty.Name == "ProjectId")
            {
                continue;
            }

            if (guidProperty.Name.EndsWith("Id") || guidProperty.Name.EndsWith("Link"))
            {
                guidProperty.SetValue(obj, Guid.Empty);
            }

            if (ProjectEntityNames.Contains(obj.GetType().Name) && guidProperty.Name == "Id")
            {
                guidProperty.SetValue(obj, caseId);
            }
        }

        foreach (var instanceProperty in instanceProperties)
        {
            if (instanceProperty.Name == "Project")
            {
                instanceProperty.SetValue(obj, null);
                continue;
            }

            var childObject = instanceProperty.GetValue(obj);
            SetIdsToEmptyGuids(childObject, caseId, visited);
        }

        foreach (var listProperty in listProperties)
        {
            var childObject = listProperty.GetValue(obj);

            if (childObject is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    SetIdsToEmptyGuids(item, caseId, visited);
                }
            }
        }
    }
}
