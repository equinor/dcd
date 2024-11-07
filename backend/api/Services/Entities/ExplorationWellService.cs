using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationWellService(DcdDbContext context) : IExplorationWellService
{
    public async Task<List<ExplorationWell>> GetExplorationWellsForExploration(Guid explorationId)
    {
        var explorationWells = await context.ExplorationWell!
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.ExplorationId == explorationId).ToListAsync();

        return explorationWells;
    }
}
