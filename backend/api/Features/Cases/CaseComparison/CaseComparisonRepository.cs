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

        var cases = await context.Cases
            .Include(x => x.DrainageStrategy)
            .Include(x => x.Exploration)
            .Include(x => x.WellProject)
            .Include(x => x.Substructure)
            .Include(x => x.Surf)
            .Include(x => x.Topside)
            .Include(x => x.Transport)
            .Include(x => x.OnshorePowerSupply)
            .Where(x => x.ProjectId == project.Id)
            .Where(x => !x.Archived)
            .ToListAsync();

        var caseIds = cases.Select(x => x.Id).ToList();

        await context.TimeSeriesProfiles
            .Where(x => caseIds.Contains(x.CaseId))
            .LoadAsync();

        await context.Wells
            .Where(x => x.ProjectId == project.Id)
            .LoadAsync();

        await context.ExplorationOperationalWellCosts
            .Where(x => x.ProjectId == project.Id)
            .LoadAsync();

        await context.DevelopmentOperationalWellCosts
            .Where(x => x.ProjectId == project.Id)
            .LoadAsync();

        return project;
    }
}
