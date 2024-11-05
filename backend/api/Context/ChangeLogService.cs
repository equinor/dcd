using System.Reflection;
using System.Text.Json;

using api.Authorization;
using api.Models;
using api.Models.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace api.Context;

public static class ChangeLogService
{
    private static readonly IReadOnlyList<string> PropertyNamesToIgnore = new List<string> { "Foo", "Bar", "Baz" };
    public static List<ChangeLog> GenerateChangeLogs(DcdDbContext dbContext, CurrentUser? currentUser, DateTime utcNow)
    {
        var changes = dbContext.ChangeTracker
            .Entries<IChangeTrackable>()
            .SelectMany(entity => BuildChangeLog(entity, currentUser?.Username, utcNow))
            .ToList();

        changes.AddRange(dbContext.ChangeTracker
            .Entries()
            .Where(x => x.Entity is IChangeTrackable)
            .Where(x => x.Entity is EntityState.Added or EntityState.Deleted)
            .Select(x => new ChangeLog
            {
                TimestampUtc = utcNow,
                Username = currentUser?.Username,
                EntityId = ((IChangeTrackable)x.Entity).Id,
                EntityName = x.Entity.GetType().Name,
                EntityState = x.State.ToString(),
                Payload = GenerateInitialState(x.Properties)
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
            type.GetGenericArguments().Any(x => x.IsValueType && x.IsPrimitive))
        {
            return true;
        }

        if (type.IsValueType && type.IsPrimitive)
        {
            return true;
        }

        if (type == typeof(decimal) || type == typeof(decimal?) || type == typeof(string))
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
                    EntityState = EntityState.Modified.ToString()
                });
            }
        }

        return changes;
    }
}
