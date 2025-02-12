using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

using api.AppInfrastructure.ControllerAttributes;
using api.Context;
using api.Models;
using api.Models.Infrastructure;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Features.ChangeLogs;

public class ProjectChangeLogController(DcdDbContext context) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/change-log")]
    [AuthorizeActionType(ActionType.Read)]
    public async Task<ProjectChangeLogDto> GetProjectChangeLog([FromRoute] Guid projectId)
    {
        var stopwatch = Stopwatch.StartNew();

        var projectChanges = await context.ChangeLogs
            .Where(x => x.EntityName == nameof(Project))
            .Where(x => x.EntityId == projectId)
            .OrderByDescending(x => x.TimestampUtc)
            .ToListAsync();

        return new ProjectChangeLogDto
        {
            RequestDurationInMilliseconds = (int)stopwatch.ElapsedMilliseconds,
            Changes = projectChanges.Select(x => new ChangeDto
            {
                EntityState = x.EntityState,
                PropertyName = x.PropertyName,
                OldValue = x.OldValue,
                NewValue = x.NewValue,
                Username = x.Username,
                TimestampUtc = x.TimestampUtc
            })
            .ToList(),
            FieldChanges = projectChanges
                .Where(x => x.PropertyName != null)
                .GroupBy(x => x.PropertyName!)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.TimestampUtc)
                    .Select(y => new ShortChangeDto
                    {
                        EntityState = y.EntityState,
                        OldValue = y.OldValue,
                        NewValue = y.NewValue,
                        Username = y.Username,
                        TimestampUtc = y.TimestampUtc

                    })
                .ToList())
        };
    }
}

public class ProjectChangeLogDto
{
    [Required] public required int RequestDurationInMilliseconds { get; set; }
    [Required] public required List<ChangeDto> Changes { get; set; }
    [Required] public required Dictionary<string, List<ShortChangeDto>> FieldChanges { get; set; }
}

public class ChangeDto
{
    public required string? PropertyName { get; set; }
    public required string EntityState { get; set; }
    public required string? OldValue { get; set; }
    public required string? NewValue { get; set; }
    public required string? Username { get; set; }
    public required DateTime TimestampUtc { get; set; }
}

public class ShortChangeDto
{
    public required string EntityState { get; set; }
    public required string? OldValue { get; set; }
    public required string? NewValue { get; set; }
    public required string? Username { get; set; }
    public required DateTime TimestampUtc { get; set; }
}
