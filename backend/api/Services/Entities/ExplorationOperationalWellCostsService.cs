using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationOperationalWellCostsService(DcdDbContext context) : IExplorationOperationalWellCostsService
{
    public async Task<ExplorationOperationalWellCosts?> GetOperationalWellCosts(Guid id)
    {
        var operationalWellCosts = await context.ExplorationOperationalWellCosts!
            .Include(eowc => eowc.Project)
            .FirstOrDefaultAsync(o => o.Id == id);
        return operationalWellCosts;
    }
}
