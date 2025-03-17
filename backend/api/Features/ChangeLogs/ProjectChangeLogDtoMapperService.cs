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
                result.Add(MapModified(change, liveData));

                continue;
            }

            if (change.EntityState == EntityState.Deleted.ToString())
            {
                result.Add(MapDeleted(change, liveData));

                continue;
            }

            if (change.EntityState == EntityState.Added.ToString())
            {
                result.AddRange(MapAdded(change, liveData));
            }
        }

        return result;
    }

    private static ProjectChangeLogDto MapModified(ChangeLog change, Project liveData)
    {
        return new ProjectChangeLogDto
        {
            EntityDescription = GetEntityDescription(change.EntityName, liveData, change.EntityId),
            EntityId = change.EntityId,
            EntityName = change.EntityName,
            PropertyName = change.PropertyName,
            OldValue = change.OldValue,
            NewValue = change.NewValue,
            Username = change.Username,
            TimestampUtc = change.TimestampUtc,
            EntityState = change.EntityState,
            Category = CalculateCategory(change.EntityName, change.PropertyName!)
        };
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    private static ProjectChangeLogDto MapAdded(ChangeLog change, Project liveData)
    {
        var mapping = JsonSerializer.Deserialize<Dictionary<string, object?>>(change.Payload!, JsonSerializerOptions)!;

        return new ProjectChangeLogDto
        {
            EntityDescription = GetEntityDescription(mapping, change.EntityName, liveData, change.EntityId),
            EntityId = change.EntityId,
            EntityName = change.EntityName,
            PropertyName = null,
            OldValue = null,
            NewValue = null,
            Username = change.Username,
            TimestampUtc = change.TimestampUtc,
            EntityState = EntityState.Added.ToString(),
            Category = CalculateCategoryForDeletedEntity(change.EntityName)
        };
    }

    private static ProjectChangeLogDto MapDeleted(ChangeLog change, Project liveData)
    {
        var mapping = JsonSerializer.Deserialize<Dictionary<string, object?>>(change.Payload!, JsonSerializerOptions)!;

        return new ProjectChangeLogDto
        {
            EntityDescription = GetEntityDescription(mapping, change.EntityName, liveData, change.EntityId),
            EntityId = change.EntityId,
            EntityName = change.EntityName,
            PropertyName = null,
            OldValue = null,
            NewValue = null,
            Username = change.Username,
            TimestampUtc = change.TimestampUtc,
            EntityState = EntityState.Deleted.ToString(),
            Category = CalculateCategoryForDeletedEntity(change.EntityName)
        };
    }

    private static string? GetEntityDescription(string entityName, Project liveData, Guid entityId)
    {
        return GetEntityDescription(new Dictionary<string, object?>(), entityName, liveData, entityId);
    }

    private static string? GetEntityDescription(Dictionary<string, object?> deserializedPayload, string entityName, Project liveData, Guid entityId)
    {
        return entityName switch
        {
            nameof(Case) => liveData.Cases.SingleOrDefault(x => x.Id == entityId)?.Name
                            ?? (deserializedPayload.TryGetValue(nameof(Case.Name), out var value) ? value?.ToString() : null),

            nameof(ProjectImage) => liveData.Images.SingleOrDefault(x => x.Id == entityId)?.Description
                                    ?? (deserializedPayload.TryGetValue(nameof(ProjectImage.Description), out var value) ? value?.ToString() : null),

            nameof(ProjectMember) => liveData.ProjectMembers.SingleOrDefault(x => x.Id == entityId)?.AzureAdUserId.ToString()
                                     ?? (deserializedPayload.TryGetValue(nameof(ProjectMember.AzureAdUserId), out var value) ? value?.ToString() : null),

            nameof(Well) => liveData.Wells.SingleOrDefault(x => x.Id == entityId)?.Name
                            ?? (deserializedPayload.TryGetValue(nameof(Well.Name), out var value) ? value?.ToString() : null),

            _ => null
        };
    }

    private static ChangeLogCategory CalculateCategoryForDeletedEntity(string entityName)
    {
        return entityName switch
        {
            nameof(ProjectMember) => ChangeLogCategory.AccessManagementTab,
            nameof(Well) => ChangeLogCategory.WellCostTab,
            nameof(ProjectImage) or nameof(Case) => ChangeLogCategory.ProjectOverviewTab,
            _ => ChangeLogCategory.None
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
