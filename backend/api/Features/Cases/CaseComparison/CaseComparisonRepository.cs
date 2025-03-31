using api.Context;
using api.Context.Extensions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.CaseComparison;

public class CaseComparisonRepository(DcdDbContext context)
{
    public async Task<Guid> GetPrimaryKeyForProjectIdOrRevisionId(Guid projectId)
    {
        return await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);
    }

    public async Task<Project> LoadProject(Guid projectPk)
    {
        var project = await context.Projects
            .SingleAsync(p => p.Id == projectPk);

        await context.Wells
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();

        await context.Cases
            .Include(x => x.DrainageStrategy)
            .Include(x => x.OnshorePowerSupply)
            .Include(x => x.Substructure)
            .Include(x => x.Surf)
            .Include(x => x.Topside)
            .Include(x => x.Transport)
            .Where(x => x.ProjectId == projectPk)
            .Where(x => !x.Archived)
            .LoadAsync();

        await context.TimeSeriesProfiles
            .Where(x => x.Case.ProjectId == projectPk)
            .LoadAsync();

        return project;
    }
}
