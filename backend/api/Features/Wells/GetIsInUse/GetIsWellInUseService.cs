using api.Context;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.GetIsInUse;

public class GetIsWellInUseService(DcdDbContext context)
{
    public async Task<bool> IsWellInUse(Guid wellId)
    {
        var well = await context.Wells
            .Include(w => w.WellProjectWells).ThenInclude(wp => wp.DrillingSchedule)
            .Include(w => w.ExplorationWells).ThenInclude(ew => ew.DrillingSchedule)
            .SingleOrDefaultAsync(w => w.Id == wellId);

        if (well == null)
        {
            return false;
        }

        var wellProjectIds = well.WellProjectWells
            .Where(x => x.DrillingSchedule?.Values.Length != 0)
            .Select(x => x.WellProjectId)
            .Distinct();

        var explorationIds = well.ExplorationWells
            .Where(x => x.DrillingSchedule?.Values.Length != 0)
            .Select(x => x.ExplorationId)
            .Distinct();

        return await context.Cases.AnyAsync(x => wellProjectIds.Contains(x.WellProjectId) || explorationIds.Contains(x.ExplorationId));
    }
}
