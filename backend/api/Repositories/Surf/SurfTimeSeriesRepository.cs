using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class SurfTimeSeriesRepository : BaseRepository, ISurfTimeSeriesRepository
{

    public SurfTimeSeriesRepository(DcdDbContext context) : base(context)
    {
    }

    public SurfCostProfile CreateSurfCostProfile(SurfCostProfile surfCostProfile)
    {
        _context.SurfCostProfile.Add(surfCostProfile);
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
        _context.SurfCostProfileOverride.Add(profile);
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
