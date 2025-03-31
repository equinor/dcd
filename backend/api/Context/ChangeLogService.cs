using System.Reflection;
using System.Text.Json;

using api.AppInfrastructure.Authorization;
using api.Models;
using api.Models.Infrastructure;
using api.Models.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace api.Context;

public static class ChangeLogService
{
    private static readonly IReadOnlyList<string> PropertyNamesToIgnore = new List<string> { "CreatedUtc", "UpdatedUtc" };

    public static List<ChangeLog> GenerateChangeLogs(DcdDbContext dbContext, CurrentUser? currentUser, DateTime utcNow)
    {
        var changes = dbContext.ChangeTracker
            .Entries<IChangeTrackable>()
            .SelectMany(entity => BuildChangeLog(entity, currentUser?.Username, utcNow))
            .ToList();

        changes.AddRange(dbContext.ChangeTracker
                             .Entries()
                             .Where(x => x.Entity is IChangeTrackable)
                             .Where(x => x.State is EntityState.Added or EntityState.Deleted)
                             .Select(x => new ChangeLog
                             {
                                 TimestampUtc = utcNow,
                                 Username = currentUser?.Username,
                                 EntityId = ((IChangeTrackable)x.Entity).Id,
                                 EntityName = x.Entity.GetType().Name,
                                 EntityState = x.State.ToString(),
                                 Payload = GenerateInitialState(x.Properties),
                                 NewValue = null,
                                 OldValue = null,
                                 PropertyName = null,
                                 ParentEntityId = ParentEntityId((IChangeTrackable)x.Entity)
                             }));

        return changes;
    }

    private static string GenerateInitialState(IEnumerable<PropertyEntry> properties)
    {
        return JsonSerializer.Serialize(properties.Where(x => IsSimpleType(x.Metadata.ClrType))
                                            .Where(x => !x.IsTemporary)
                                            .Where(x => !PropertyNamesToIgnore.Contains(x.Metadata.Name))
                                            .ToDictionary(x => x.Metadata.Name, x => x.CurrentValue));
    }

    public static bool IsSimpleType(Type type)
    {
        if (type.GetTypeInfo().IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
            type.GetGenericArguments().Any(x => x is { IsValueType: true, IsPrimitive: true }))
        {
            return true;
        }

        if (type is { IsValueType: true, IsPrimitive: true })
        {
            return true;
        }

        if (type == typeof(decimal) || type == typeof(decimal?) || type == typeof(string))
        {
            return true;
        }

        if (type == typeof(Guid) || type == typeof(Guid?))
        {
            return true;
        }

        return false;
    }

    private static List<ChangeLog> BuildChangeLog<T>(EntityEntry<T> entity,
                                                     string? username,
                                                     DateTime utcNow)
        where T : class, IChangeTrackable
    {
        var entityId = entity.Property(x => x.Id).CurrentValue;

        var changes = new List<ChangeLog>();

        foreach (var property in entity.Properties
                     .Where(x => x.IsModified)
                     .Where(x => !PropertyNamesToIgnore.Contains(x.Metadata.Name)))
        {
            var currentValue = property.CurrentValue;
            var originalValue = property.OriginalValue;

            if (property.IsModified && currentValue != originalValue)
            {
                changes.Add(new ChangeLog
                {
                    EntityName = entity.Entity.GetType().Name,
                    PropertyName = property.Metadata.Name,
                    EntityId = entityId,
                    OldValue = originalValue?.ToString(),
                    NewValue = currentValue?.ToString(),
                    Username = username,
                    TimestampUtc = utcNow,
                    EntityState = EntityState.Modified.ToString(),
                    Payload = null,
                    ParentEntityId = ParentEntityId(entity.Entity)
                });
            }
        }

        return changes;
    }

    private static Guid? ParentEntityId(IChangeTrackable entity)
    {
        // Project entities
        if (entity.GetType() == typeof(ProjectImage))
        {
            return (entity as ProjectImage)!.ProjectId;
        }

        if (entity.GetType() == typeof(ProjectMember))
        {
            return (entity as ProjectMember)!.ProjectId;
        }

        if (entity.GetType() == typeof(Well))
        {
            return (entity as Well)!.ProjectId;
        }

        if (entity.GetType() == typeof(Case))
        {
            return (entity as Case)!.ProjectId;
        }

        // Case entities
        if (entity.GetType() == typeof(Campaign))
        {
            return (entity as Campaign)!.CaseId;
        }

        if (entity.GetType() == typeof(TimeSeriesProfile))
        {
            return (entity as TimeSeriesProfile)!.CaseId;
        }

        if (entity.GetType() == typeof(CaseImage))
        {
            return (entity as CaseImage)!.CaseId;
        }

        if (entity.GetType() == typeof(Transport))
        {
            return (entity as Transport)!.CaseId;
        }

        if (entity.GetType() == typeof(Substructure))
        {
            return (entity as Substructure)!.CaseId;
        }

        if (entity.GetType() == typeof(Topside))
        {
            return (entity as Topside)!.CaseId;
        }

        if (entity.GetType() == typeof(Surf))
        {
            return (entity as Surf)!.CaseId;
        }

        if (entity.GetType() == typeof(DrainageStrategy))
        {
            return (entity as DrainageStrategy)!.CaseId;
        }

        if (entity.GetType() == typeof(OnshorePowerSupply))
        {
            return (entity as OnshorePowerSupply)!.CaseId;
        }

        // Campaign entities
        if (entity.GetType() == typeof(CampaignWell))
        {
            return (entity as CampaignWell)!.CampaignId;
        }

        return null;
    }
}
