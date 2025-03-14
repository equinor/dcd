using api.Context;
using api.Context.Extensions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ChangeLogs;

public class ProjectChangeLogService(DcdDbContext context)
{
    public async Task<List<ProjectChangeLogDto>> GetProjectChangeLogs(Guid projectId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var primaryKeys = await GetPrimaryKeysForProjectRelatedEntities(projectPk);

        var changes = await context.ChangeLogs
            .Where(x => primaryKeys.Contains(x.EntityId))
            .OrderByDescending(x => x.TimestampUtc)
            .ThenBy(x => x.EntityName)
            .ToListAsync();

        return ProjectChangeLogDtoMapperService.MapToDtos(changes);
    }

    private async Task<List<Guid>> GetPrimaryKeysForProjectRelatedEntities(Guid projectPk)
    {
        var primaryKeys = new List<Guid>();

        primaryKeys.AddRange(await context.ProjectImages
                                 .Where(x => x.ProjectId == projectPk)
                                 .Select(x => x.Id)
                                 .ToListAsync());

        primaryKeys.AddRange(await context.ProjectMembers
                                 .Where(x => x.ProjectId == projectPk)
                                 .Select(x => x.Id)
                                 .ToListAsync());

        primaryKeys.AddRange(await context.Wells
                                 .Where(x => x.ProjectId == projectPk)
                                 .Select(x => x.Id)
                                 .ToListAsync());

        var operationalWellCostIds = await (from d in context.DevelopmentOperationalWellCosts
                                            join e in context.ExplorationOperationalWellCosts on d.ProjectId equals e.ProjectId
                                            where d.ProjectId == projectPk
                                            select new { Id1 = d.Id, Id2 = e.Id })
            .SingleAsync();

        primaryKeys.AddRange(operationalWellCostIds.Id1);
        primaryKeys.AddRange(operationalWellCostIds.Id2);

        return primaryKeys;
    }
}
