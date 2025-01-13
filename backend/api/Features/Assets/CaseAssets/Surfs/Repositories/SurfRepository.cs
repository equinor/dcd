using System.Linq.Expressions;

using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Surfs.Repositories;

public class SurfRepository(DcdDbContext context) : BaseRepository(context), ISurfRepository
{
    public async Task<Surf?> GetSurf(Guid surfId)
    {
        return await Get<Surf>(surfId);
    }

    public async Task<Surf?> GetSurfWithCostProfile(Guid surfId)
    {
        return await Context.Surfs
                        .Include(t => t.CostProfile)
                        .FirstOrDefaultAsync(t => t.Id == surfId);
    }

    public async Task<bool> SurfHasCostProfileOverride(Guid surfId)
    {
        return await Context.Surfs
            .AnyAsync(t => t.Id == surfId && t.CostProfileOverride != null);
    }
}
