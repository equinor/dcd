using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.Surfs.Repositories;

public class SurfTimeSeriesRepository(DcdDbContext context) : BaseRepository(context)
{
    public async Task<SurfCostProfile?> GetSurfCostProfile(Guid surfCostProfileId)
    {
        return await GetWithIncludes<SurfCostProfile>(surfCostProfileId, s => s.Surf);
    }

    public SurfCostProfile UpdateSurfCostProfile(SurfCostProfile surfCostProfile)
    {
        return Update(surfCostProfile);
    }

    public async Task<SurfCostProfileOverride?> GetSurfCostProfileOverride(Guid surfCostProfileOverrideId)
    {
        return await GetWithIncludes<SurfCostProfileOverride>(surfCostProfileOverrideId, s => s.Surf);
    }

    public SurfCostProfileOverride UpdateSurfCostProfileOverride(SurfCostProfileOverride surfCostProfileOverride)
    {
        return Update(surfCostProfileOverride);
    }
}
