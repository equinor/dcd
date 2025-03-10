using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Stea;

public class SteaRepository(DcdDbContext context)
{
    public async Task<Project> GetProjectWithCasesAndProfiles(Guid projectPk)
    {
        var project = await context.Projects
            .SingleAsync(p => p.Id == projectPk);

        await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => !x.Archived)
            .LoadAsync();

        await context.TimeSeriesProfiles
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => !x.Case.Archived)
            .LoadAsync();

        await context.Campaigns
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => !x.Case.Archived)
            .LoadAsync();

        project.Cases = project.Cases.OrderBy(c => c.CreatedUtc).ToList();

        return project;
    }
}
