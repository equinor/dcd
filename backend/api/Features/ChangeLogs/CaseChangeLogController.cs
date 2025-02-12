using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

using api.AppInfrastructure.ControllerAttributes;
using api.Context;
using api.Models;

using DocumentFormat.OpenXml.Wordprocessing;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Features.ChangeLogs.Case;

public class CaseChangeLogController(DcdDbContext context) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/cases/{caseId:guid}/change-log")]
    [AuthorizeActionType(ActionType.Read)]
    public async Task<CaseChangeLogDto> GetProjectChangeLog([FromRoute] Guid projectId, Guid caseId)
    {
        var stopwatch = Stopwatch.StartNew();

        var caseAssetIds = await context.Cases
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.Id == caseId)
            .Select(x => new
            {
                x.Id,
                x.SurfId,
                x.DrainageStrategyId,
                x.SubstructureId,
                x.TopsideId,
                x.TransportId,
                x.OnshorePowerSupplyId
            })
            .SingleAsync();

        var profileDataIds = await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .ToDictionaryAsync(x => x.Id, x => x.ProfileType);

        var allIds = new List<Guid>
        {
            caseAssetIds.Id,
            caseAssetIds.SurfId,
            caseAssetIds.DrainageStrategyId,
            caseAssetIds.SubstructureId,
            caseAssetIds.TopsideId,
            caseAssetIds.TransportId,
            caseAssetIds.OnshorePowerSupplyId,
        }
        .Union(profileDataIds.Keys);

        var entityNames = new List<string>
        {
            nameof(Case),
            nameof(Surf),
            nameof(DrainageStrategy),
            nameof(Substructure),
            nameof(Topside),
            nameof(Transport),
            nameof(OnshorePowerSupply),
            nameof(TimeSeriesProfile)
        };

        var caseAndAssetChanges = await context.ChangeLogs
            .Where(x => allIds.Contains(x.EntityId))
            .Where(x => entityNames.Contains(x.EntityName))
            .OrderByDescending(x => x.TimestampUtc)
            .ToListAsync();

        return new CaseChangeLogDto
        {
            RequestDurationInMilliseconds = (int)stopwatch.ElapsedMilliseconds,
            FieldChanges = caseAndAssetChanges
                .Where(x => x.PropertyName != null)
                .Where(x => x.EntityState == "Modified")
                .GroupBy(x =>
                {
                    if (x.EntityName == nameof(TimeSeriesProfile))
                    {
                        return $"{x.EntityName}-{profileDataIds[x.EntityId]}-{x.PropertyName!}";
                    }

                    return $"{x.EntityName}-{x.PropertyName!}";
                })
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.TimestampUtc)
                    .Select(y => new ShortChangeDto
                    {
                        EntityName = y.EntityName == nameof(TimeSeriesProfile) ? (nameof(TimeSeriesProfile)+"-"+profileDataIds[y.EntityId]) : y.EntityName,
                        PropertyName = y.PropertyName!,
                        OldValue = y.OldValue,
                        NewValue = y.NewValue,
                        Username = y.Username,
                        TimestampUtc = y.TimestampUtc

                    })
                .ToList())
        };
    }
}

public class CaseChangeLogDto
{
    [Required] public required int RequestDurationInMilliseconds { get; set; }
    [Required] public required Dictionary<string, List<ShortChangeDto>> FieldChanges { get; set; }
}
public class ShortChangeDto
{
    public required string EntityName { get; set; }
    public required string PropertyName { get; set; }
    public required string? NewValue { get; set; }
    public required string? OldValue { get; set; }
    public required string? Username { get; set; }
    public required DateTime TimestampUtc { get; set; }
}
