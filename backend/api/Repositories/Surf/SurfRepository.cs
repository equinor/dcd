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
}
