using api.Context;
using api.Models;


namespace api.Repositories;

public class TopsideRepository : ITopsideRepository
{
    private readonly DcdDbContext _context;

    public TopsideRepository(DcdDbContext context)
    {
        _context = context;
    }

    public async Task<Topside?> GetTopside(Guid topsideId)
    {
        return await _context.Topsides.FindAsync(topsideId);
    }

    public async Task<Topside> UpdateTopside(Topside topside)
    {
        _context.Topsides.Update(topside);
        await _context.SaveChangesAsync();
        return topside;
    }

    public async Task<TopsideCostProfileOverride?> GetTopsideCostProfileOverride(Guid topsideCostProfileOverrideId)
    {
        return await _context.TopsideCostProfileOverride.FindAsync(topsideCostProfileOverrideId);
    }

    public async Task<TopsideCostProfileOverride> UpdateTopsideCostProfileOverride(TopsideCostProfileOverride topsideCostProfileOverride)
    {
        _context.TopsideCostProfileOverride.Update(topsideCostProfileOverride);
        await _context.SaveChangesAsync();
        return topsideCostProfileOverride;
    }
}
