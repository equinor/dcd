using System.Collections;
using System.Reflection;

using api.Models;

namespace api.Features.Cases.Duplicate;

public static class ResetIdPropertiesInCaseGraphService
{
    private const string ProjectNamespacePrefix = "api.";
    private const string ProjectEntityName = "Case";

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
            .Where(x => x.PropertyType.IsGenericType)
            .Where(x => x.PropertyType.GetGenericArguments().Any(y => y.Namespace?.StartsWith(ProjectNamespacePrefix) == true))
            .ToList();

        var instanceProperties = properties
            .Where(x => x.PropertyType.Namespace?.StartsWith(ProjectNamespacePrefix) == true)
            .ToList();

        foreach (var guidProperty in guidProperties)
        {
            if (guidProperty.Name is "ProjectId" or "WellId")
            {
                continue;
            }

            if (guidProperty.Name.EndsWith("Id"))
            {
                guidProperty.SetValue(obj, Guid.Empty);
            }

            if (guidProperty.Name == "CaseId")
            {
                guidProperty.SetValue(obj, caseId);
            }

            if (ProjectEntityName == obj.GetType().Name && guidProperty.Name == "Id")
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
