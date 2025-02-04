using api.Context;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.GetIsInUse;

public class GetIsWellInUseService(DcdDbContext context)
{
    public async Task<bool> IsWellInUse(Guid wellId)
    {
        var well = await context.Wells
            .Include(w => w.DevelopmentWells)
            .Include(w => w.ExplorationWells)
            .SingleOrDefaultAsync(w => w.Id == wellId);

        if (well == null)
        {
            return false;
        }

        var wellProjectIds = well.DevelopmentWells
            .Where(x => x.Values.Length != 0)
            .Select(x => x.WellProjectId)
            .Distinct();

        var explorationIds = well.ExplorationWells
            .Where(x => x.Values.Length != 0)
            .Select(x => x.ExplorationId)
            .Distinct();

        return await context.Cases.AnyAsync(x => wellProjectIds.Contains(x.WellProjectId) || explorationIds.Contains(x.ExplorationId));
    }
}
