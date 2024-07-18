using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class WellRepository : BaseRepository, IWellRepository
{

    public WellRepository(DcdDbContext context) : base(context)
    {
    }

    public async Task<Well?> GetWell(Guid wellId)
    {
        return await Get<Well>(wellId);
    }

    public async Task<IEnumerable<Case>> GetCasesAffectedByDeleteWell(Guid wellId)
    {
        var well = await _context.Wells
            .Include(w => w.WellProjectWells)!
                .ThenInclude(wp => wp.DrillingSchedule)
            .Include(w => w.WellProjectWells)!
                .ThenInclude(wp => wp.WellProject)
            .Include(w => w.ExplorationWells)!
                .ThenInclude(ew => ew.DrillingSchedule)
            .Include(w => w.ExplorationWells)!
                .ThenInclude(ew => ew.Exploration)
            .FirstOrDefaultAsync(w => w.Id == wellId);

        if (well == null) { return []; }

        var wellProjectIds = well.WellProjectWells?
            .Where(wpw => wpw.DrillingSchedule?.Values.Length != 0 == true)
            .Select(wpw => wpw.WellProject.Id)
            .Distinct() ?? [];

        var explorationIds = well.ExplorationWells?
            .Where(ew => ew.DrillingSchedule?.Values.Length != 0 == true)
            .Select(ew => ew.Exploration.Id)
            .Distinct() ?? [];

        var cases = await _context.Cases
            .Where(c => wellProjectIds.Contains(c.WellProjectLink) || explorationIds.Contains(c.ExplorationLink))
            .ToListAsync();

        return cases;
    }

    public Well UpdateWell(Well well)
    {
        return Update(well);
    }

    public Well AddWell(Well well)
    {
        _context.Add(well);
        return well;
    }

    public void DeleteWell(Well well)
    {
        _context.Remove(well);
    }
}
