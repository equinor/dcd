using System.Text.Json;

using api.Models;
using api.Models.Infrastructure;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ChangeLogs;

public static class ProjectChangeLogDtoMapperService
{
    public static List<ProjectChangeLogDto> MapToDtos(List<ChangeLog> changes, Project liveData)
    {
        var result = new List<ProjectChangeLogDto>();

        foreach (var change in changes)
        {
            if (change.EntityState == EntityState.Modified.ToString())
            {
                result.Add(new ProjectChangeLogDto
                {
                    EntityDescription = GetEntityDescription(change.EntityId, change.EntityName, liveData),
                    EntityId = change.EntityId,
                    EntityName = change.EntityName,
                    PropertyName = change.PropertyName,
                    OldValue = change.OldValue,
                    NewValue = change.NewValue,
                    Username = change.Username,
                    TimestampUtc = change.TimestampUtc,
                    EntityState = change.EntityState,
                    Category = CalculateCategory(change.EntityName, change.PropertyName!)
                });

                continue;
            }

            if (change.EntityState == EntityState.Deleted.ToString())
            {
                result.Add(new ProjectChangeLogDto
                {
                    EntityDescription = GetEntityDescription(change.EntityId, change.EntityName, liveData),
                    EntityId = change.EntityId,
                    EntityName = change.EntityName,
                    PropertyName = null,
                    OldValue = null,
                    NewValue = null,
                    Username = change.Username,
                    TimestampUtc = change.TimestampUtc,
                    EntityState = change.EntityState,
                    Category = ChangeLogCategory.None
                });

                continue;
            }

            if (change.EntityState == EntityState.Added.ToString())
            {
                result.AddRange(ExpandToFields(change.EntityName, change.EntityId, change.Username, change.TimestampUtc, change.Payload!, liveData));
            }
        }

        return result;
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private static readonly HashSet<string> PropertyNamesToIgnore = ["Id", "ProjectId", "CreatedBy", "UpdatedBy", "CreatedUtc", "UpdatedUtc"];

    private static readonly Dictionary<string, HashSet<string>> EntitySpecificPropertiesToIgnore = new()
    {
        { "CaseImage", ["Url"] },
        { "ProjectImage", ["Url"] }
    };

    private static List<ProjectChangeLogDto> ExpandToFields(string entityName, Guid entityId, string? username, DateTime timestampUtc, string payload, Project liveData)
    {
        var mapping = JsonSerializer.Deserialize<Dictionary<string, object?>>(payload, JsonSerializerOptions)!;

        var entitySpecificPropertiesToIgnore = EntitySpecificPropertiesToIgnore.TryGetValue(entityName, out var value) ? value : [];

        return mapping.Where(x => !PropertyNamesToIgnore.Contains(x.Key))
            .Where(x => !entitySpecificPropertiesToIgnore.Contains(x.Key))
            .Select(change => new ProjectChangeLogDto
            {
                EntityDescription = GetEntityDescription(entityId, entityName, liveData),
                EntityId = entityId,
                EntityName = entityName,
                PropertyName = change.Key,
                OldValue = null,
                NewValue = change.Value?.ToString(),
                Username = username,
                TimestampUtc = timestampUtc,
                EntityState = EntityState.Added.ToString(),
                Category = CalculateCategory(entityName, change.Key)
            })
            .ToList();
    }

    private static string? GetEntityDescription(Guid entityId, string entityName, Project liveData)
    {
        return entityName switch
        {
            nameof(ProjectMember) => liveData.ProjectMembers.SingleOrDefault(x => x.Id == entityId)?.AzureAdUserId.ToString(),
            nameof(Well) => liveData.Wells.SingleOrDefault(x => x.Id == entityId)?.Name,
            _ => null
        };
    }

    private static ChangeLogCategory CalculateCategory(string entityName, string propertyName)
    {
        switch (entityName)
        {
            case nameof(ProjectMember):
                return ChangeLogCategory.AccessManagementTab;

            case nameof(Well):
            case nameof(DevelopmentOperationalWellCosts):
            case nameof(ExplorationOperationalWellCosts):
                return ChangeLogCategory.WellCostTab;
        }

        switch (entityName, propertyName)
        {
            case (nameof(Project), nameof(Project.AverageDevelopmentDrillingDays)):
            case (nameof(Project), nameof(Project.Co2EmissionFromFuelGas)):
            case (nameof(Project), nameof(Project.Co2EmissionsFromFlaredGas)):
            case (nameof(Project), nameof(Project.Co2RemovedFromGas)):
            case (nameof(Project), nameof(Project.Co2Vented)):
            case (nameof(Project), nameof(Project.DailyEmissionFromDrillingRig)):
            case (nameof(Project), nameof(Project.FlaredGasPerProducedVolume)):
                return ChangeLogCategory.Co2Tab;

            case (nameof(Project), nameof(Project.PhysicalUnit)):
            case (nameof(Project), nameof(Project.Currency)):
            case (nameof(Project), nameof(Project.Classification)):
            case (nameof(Project), nameof(Project.OilPriceUsd)):
            case (nameof(Project), nameof(Project.NglPriceUsd)):
            case (nameof(Project), nameof(Project.GasPriceNok)):
            case (nameof(Project), nameof(Project.DiscountRate)):
            case (nameof(Project), nameof(Project.ExchangeRateUsdToNok)):
            case (nameof(Project), nameof(Project.NpvYear)):
                return ChangeLogCategory.SettingsTab;

            case (nameof(Project), nameof(Project.Country)):
            case (nameof(Project), nameof(Project.Description)):
            case (nameof(Project), nameof(Project.Name)):
            case (nameof(Project), nameof(Project.ProjectCategory)):
            case (nameof(Project), nameof(Project.ProjectPhase)):
            case (nameof(ProjectImage), nameof(ProjectImage.Description)):
                return ChangeLogCategory.ProjectOverviewTab;

            case (nameof(Project), nameof(Project.SharepointSiteUrl)):
                return ChangeLogCategory.ProspTab;

            default:
                return ChangeLogCategory.None;
        }
    }
}
