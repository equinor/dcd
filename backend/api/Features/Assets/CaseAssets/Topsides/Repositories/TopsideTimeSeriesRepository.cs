using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.Topsides.Repositories;

public class TopsideTimeSeriesRepository(DcdDbContext context) : BaseRepository(context)
{
    public async Task<TopsideCostProfile?> GetTopsideCostProfile(Guid topsideCostProfileId)
    {
        return await GetWithIncludes<TopsideCostProfile>(topsideCostProfileId, t => t.Topside);
    }

    public TopsideCostProfile UpdateTopsideCostProfile(TopsideCostProfile topsideCostProfile)
    {
        return Update(topsideCostProfile);
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
