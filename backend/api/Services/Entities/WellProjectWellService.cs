using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;

using AutoMapper;

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
