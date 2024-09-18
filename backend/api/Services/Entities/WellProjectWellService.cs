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
        return await _context.WellProjectWell!
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.WellProjectId == wellProjectId).ToListAsync();
    }
}
