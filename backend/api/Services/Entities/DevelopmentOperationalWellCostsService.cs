using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class DevelopmentOperationalWellCostsService(DcdDbContext context) : IDevelopmentOperationalWellCostsService
{
    public async Task<DevelopmentOperationalWellCosts?> GetOperationalWellCosts(Guid id)
    {
        var operationalWellCosts = await context.DevelopmentOperationalWellCosts
            .Include(dowc => dowc.Project)
            .FirstOrDefaultAsync(o => o.Id == id);
        return operationalWellCosts;
    }
}
