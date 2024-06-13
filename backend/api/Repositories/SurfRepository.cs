using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class SurfRepository : BaseRepository, ISurfRepository
{

    public SurfRepository(DcdDbContext context) : base(context)
    {
    }

    public async Task<Surf?> GetSurf(Guid surfId)
    {
        return await Get<Surf>(surfId);
    }

    public async Task<Surf?> GetSurfWithCostProfile(Guid surfId)
    {
        return await _context.Surfs
                        .Include(t => t.CostProfile)
                        .FirstOrDefaultAsync(t => t.Id == surfId);
    }

    public async Task<bool> SurfHasCostProfileOverride(Guid surfId)
    {
        return await _context.Surfs
            .AnyAsync(t => t.Id == surfId && t.CostProfileOverride != null);
    }

    public Surf UpdateSurf(Surf surf)
    {
        return Update(surf);
    }

    public SurfCostProfile CreateSurfCostProfile(SurfCostProfile surfCostProfile)
    {
        _context.SurfCostProfile.Add(surfCostProfile);
        return surfCostProfile;
    }

    public async Task<SurfCostProfile?> GetSurfCostProfile(Guid surfCostProfileId)
    {
        return await Get<SurfCostProfile>(surfCostProfileId);
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
        return await Get<SurfCostProfileOverride>(surfCostProfileOverrideId);
    }

    public SurfCostProfileOverride UpdateSurfCostProfileOverride(SurfCostProfileOverride surfCostProfileOverride)
    {
        return Update(surfCostProfileOverride);
    }
}
