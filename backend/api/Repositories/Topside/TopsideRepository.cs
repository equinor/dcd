using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class TopsideRepository : BaseRepository, ITopsideRepository
{

    public TopsideRepository(DcdDbContext context) : base(context)
    {
    }

    public async Task<Topside?> GetTopside(Guid topsideId)
    {
        return await Get<Topside>(topsideId);
    }

    public async Task<Topside?> GetTopsideWithCostProfile(Guid topsideId)
    {
        return await _context.Topsides
                        .Include(t => t.CostProfile)
                        .FirstOrDefaultAsync(t => t.Id == topsideId);
    }

    public async Task<bool> TopsideHasCostProfileOverride(Guid topsideId)
    {
        return await _context.Topsides
            .AnyAsync(t => t.Id == topsideId && t.CostProfileOverride != null);
    }

    public Topside UpdateTopside(Topside topside)
    {
        return Update(topside);
    }

    public async Task<TopsideCostProfile?> GetTopsideCostProfile(Guid topsideCostProfileId)
    {
        return await Get<TopsideCostProfile>(topsideCostProfileId);
    }

    public TopsideCostProfile CreateTopsideCostProfile(TopsideCostProfile topsideCostProfile)
    {
        _context.TopsideCostProfiles.Add(topsideCostProfile);
        return topsideCostProfile;
    }

    public TopsideCostProfile UpdateTopsideCostProfile(TopsideCostProfile topsideCostProfile)
    {
        return Update(topsideCostProfile);
    }

    public TopsideCostProfileOverride CreateTopsideCostProfileOverride(TopsideCostProfileOverride profile)
    {
        _context.TopsideCostProfileOverride.Add(profile);
        return profile;
    }

    public async Task<TopsideCostProfileOverride?> GetTopsideCostProfileOverride(Guid topsideCostProfileOverrideId)
    {
        return await Get<TopsideCostProfileOverride>(topsideCostProfileOverrideId);
    }

    public TopsideCostProfileOverride UpdateTopsideCostProfileOverride(TopsideCostProfileOverride topsideCostProfileOverride)
    {
        return Update(topsideCostProfileOverride);
    }
}
