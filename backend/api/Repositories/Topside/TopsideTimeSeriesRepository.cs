using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class TopsideTimeSeriesRepository : BaseRepository, ITopsideTimeSeriesRepository
{

    public TopsideTimeSeriesRepository(DcdDbContext context) : base(context)
    {
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
