using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellProjectWellService(DcdDbContext context) : IWellProjectWellService
{
    public async Task<List<WellProjectWell>> GetWellProjectWellsForWellProject(Guid wellProjectId)
    {
        return await context.WellProjectWell!
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.WellProjectId == wellProjectId).ToListAsync();
    }
}
