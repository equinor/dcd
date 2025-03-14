using System.Text.Json;

using api.Models.Infrastructure;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ChangeLogs;

public static class ProjectChangeLogDtoMapperService
{
    public static List<ProjectChangeLogDto> MapToDtos(List<ChangeLog> changes)
    {
        var result = new List<ProjectChangeLogDto>();

        foreach (var change in changes)
        {
            if (change.EntityState == EntityState.Modified.ToString())
            {
                result.Add(new ProjectChangeLogDto
                {
                    EntityId = change.EntityId,
                    EntityName = change.EntityName,
                    PropertyName = change.PropertyName,
                    OldValue = change.OldValue,
                    NewValue = change.NewValue,
                    Username = change.Username,
                    TimestampUtc = change.TimestampUtc,
                    EntityState = change.EntityState
                });

                continue;
            }

            if (change.EntityState == EntityState.Deleted.ToString())
            {
                result.Add(new ProjectChangeLogDto
                {
                    EntityId = change.EntityId,
                    EntityName = change.EntityName,
                    PropertyName = null,
                    OldValue = null,
                    NewValue = null,
                    Username = change.Username,
                    TimestampUtc = change.TimestampUtc,
                    EntityState = change.EntityState
                });

                continue;
            }

            if (change.EntityState == EntityState.Added.ToString())
            {
                result.AddRange(ExpandToFields(change.EntityName, change.EntityId, change.Username, change.TimestampUtc, change.Payload!));
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

    private static List<ProjectChangeLogDto> ExpandToFields(string entityName, Guid entityId, string? username, DateTime timestampUtc, string payload)
    {
        var mapping = JsonSerializer.Deserialize<Dictionary<string, object?>>(payload, JsonSerializerOptions)!;

        var entitySpecificPropertiesToIgnore = EntitySpecificPropertiesToIgnore.TryGetValue(entityName, out var value) ? value : [];

        return mapping.Where(x => !PropertyNamesToIgnore.Contains(x.Key))
            .Where(x => !entitySpecificPropertiesToIgnore.Contains(x.Key))
            .Select(change => new ProjectChangeLogDto
            {
                EntityId = entityId,
                EntityName = entityName,
                PropertyName = change.Key,
                OldValue = null,
                NewValue = change.Value?.ToString(),
                Username = username,
                TimestampUtc = timestampUtc,
                EntityState = EntityState.Added.ToString()
            })
            .ToList();
    }
}
