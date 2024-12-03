using api.Context;
using api.Models;
using api.Repositories;

namespace api.Features.Assets.CaseAssets.Surfs.Repositories;

public class SurfTimeSeriesRepository(DcdDbContext context) : BaseRepository(context), ISurfTimeSeriesRepository
{
    public SurfCostProfile CreateSurfCostProfile(SurfCostProfile surfCostProfile)
    {
        Context.SurfCostProfile.Add(surfCostProfile);
        return surfCostProfile;
    }

    public async Task<SurfCostProfile?> GetSurfCostProfile(Guid surfCostProfileId)
    {
        return await GetWithIncludes<SurfCostProfile>(surfCostProfileId, s => s.Surf);
    }

    public SurfCostProfile UpdateSurfCostProfile(SurfCostProfile surfCostProfile)
    {
        return Update(surfCostProfile);
    }

    public SurfCostProfileOverride CreateSurfCostProfileOverride(SurfCostProfileOverride profile)
    {
        Context.SurfCostProfileOverride.Add(profile);
        return profile;
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
