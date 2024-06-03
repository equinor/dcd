using api.Context;
using api.Models;


namespace api.Repositories;

public class SurfRepository : ISurfRepository
{
    private readonly DcdDbContext _context;

    public SurfRepository(DcdDbContext context)
    {
        _context = context;
    }

    public async Task<Surf?> GetSurf(Guid surfId)
    {
        return await _context.Surfs.FindAsync(surfId);
    }

    public async Task<Surf> UpdateSurf(Surf surf)
    {
        _context.Surfs.Update(surf);
        await _context.SaveChangesAsync();
        return surf;
    }

    public async Task<SurfCostProfileOverride?> GetSurfCostProfileOverride(Guid surfCostProfileOverrideId)
    {
        return await _context.SurfCostProfileOverride.FindAsync(surfCostProfileOverrideId);
    }

    public async Task<SurfCostProfileOverride> UpdateSurfCostProfileOverride(SurfCostProfileOverride surfCostProfileOverride)
    {
        _context.SurfCostProfileOverride.Update(surfCostProfileOverride);
        await _context.SaveChangesAsync();
        return surfCostProfileOverride;
    }
}
