using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationWellService : IExplorationWellService
{
    private readonly DcdDbContext _context;

    public ExplorationWellService(DcdDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExplorationWell>> GetExplorationWellsForExploration(Guid explorationId)
    {
        var explorationWells = await _context.ExplorationWell!
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.ExplorationId == explorationId).ToListAsync();
        if (explorationWells == null)
        {
            throw new ArgumentException(string.Format("ExplorationWell for Exploration {0} not found.", explorationId));
        }
        return explorationWells;
    }
}
