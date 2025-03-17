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

        var liveData = await context.Projects
            .Include(x => x.DevelopmentOperationalWellCosts)
            .Include(x => x.ExplorationOperationalWellCosts)
            .Where(x => x.Id == projectPk)
            .SingleAsync();

        await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();

        await context.ProjectMembers.Where(x => x.ProjectId == projectPk).LoadAsync();
        await context.Wells.Where(x => x.ProjectId == projectPk).LoadAsync();

        return ProjectChangeLogDtoMapperService.MapToDtos(changes, liveData)
            .Where(x => x.Category != ChangeLogCategory.None)
            .ToList();
    }

    private async Task<List<Guid>> GetPrimaryKeysForProjectRelatedEntities(Guid projectPk)
    {
        var primaryKeys = new List<Guid>
        {
            projectPk
        };

        primaryKeys.AddRange(await context.ChangeLogs
                                 .Where(x => x.ParentEntityId == projectPk)
                                 .Select(x => x.EntityId)
                                 .ToListAsync());

        return primaryKeys;
    }
}
