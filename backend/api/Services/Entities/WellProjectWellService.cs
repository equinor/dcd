using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellProjectWellService : IWellProjectWellService
{
    private readonly DcdDbContext _context;

    public WellProjectWellService(DcdDbContext context)
    {
        _context = context;
    }

    public async Task<List<WellProjectWell>> GetWellProjectWellsForWellProject(Guid wellProjectId)
    {
        var wellProjectWells = await _context.WellProjectWell!
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.WellProjectId == wellProjectId).ToListAsync();
        if (wellProjectWells == null)
        {
            throw new ArgumentException(string.Format("WellProjectWells for WellProject {0} not found.", wellProjectId));
        }
        return wellProjectWells;
    }
}
