using api.Context;
using api.Models;


namespace api.Repositories;

public class TopsideTimeSeriesRepository(DcdDbContext context) : BaseRepository(context), ITopsideTimeSeriesRepository
{
    public async Task<TopsideCostProfile?> GetTopsideCostProfile(Guid topsideCostProfileId)
    {
        return await GetWithIncludes<TopsideCostProfile>(topsideCostProfileId, t => t.Topside);
    }

    public TopsideCostProfile CreateTopsideCostProfile(TopsideCostProfile topsideCostProfile)
    {
        Context.TopsideCostProfiles.Add(topsideCostProfile);
        return topsideCostProfile;
    }

    public TopsideCostProfile UpdateTopsideCostProfile(TopsideCostProfile topsideCostProfile)
    {
        return Update(topsideCostProfile);
    }

    public TopsideCostProfileOverride CreateTopsideCostProfileOverride(TopsideCostProfileOverride profile)
    {
        Context.TopsideCostProfileOverride.Add(profile);
        return profile;
    }

    public async Task<TopsideCostProfileOverride?> GetTopsideCostProfileOverride(Guid topsideCostProfileOverrideId)
    {
        return await GetWithIncludes<TopsideCostProfileOverride>(topsideCostProfileOverrideId, t => t.Topside);
    }

    public TopsideCostProfileOverride UpdateTopsideCostProfileOverride(TopsideCostProfileOverride topsideCostProfileOverride)
    {
        return Update(topsideCostProfileOverride);
    }
}
