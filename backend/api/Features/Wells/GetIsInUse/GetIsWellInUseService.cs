using api.Context;
using api.Context.Extensions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.GetIsInUse;

public class GetIsWellInUseService(DcdDbContext context)
{
    public async Task<bool> IsWellInUse(Guid projectId, Guid wellId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);

        var well = await context.Wells
            .Include(w => w.DevelopmentWells)
            .Include(w => w.ExplorationWells)
            .SingleOrDefaultAsync(w => w.ProjectId == projectPk && w.Id == wellId);

        if (well == null)
        {
            return false;
        }

        var wellProjectIds = context.DevelopmentWells
            .Where(x => x.WellProject.Case.ProjectId == projectPk)
            .Where(x => x.WellId == wellId)
            .Select(x => x.WellProjectId)
            .Distinct();

        var explorationIds = context.ExplorationWell
            .Where(x => x.Exploration.Case.ProjectId == projectPk)
            .Where(x => x.WellId == wellId)
            .Select(x => x.ExplorationId)
            .Distinct();

        return await context.Cases.AnyAsync(x => wellProjectIds.Contains(x.WellProjectId) || explorationIds.Contains(x.ExplorationId));
    }
}
