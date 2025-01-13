using System.Linq.Expressions;

using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Topsides.Repositories;

public class TopsideRepository(DcdDbContext context) : BaseRepository(context), ITopsideRepository
{
    public async Task<Topside?> GetTopside(Guid topsideId)
    {
        return await Get<Topside>(topsideId);
    }

    public async Task<Topside?> GetTopsideWithIncludes(Guid topsideId, params Expression<Func<Topside, object>>[] includes)
    {
        return await GetWithIncludes(topsideId, includes);
    }

    public async Task<Topside?> GetTopsideWithCostProfile(Guid topsideId)
    {
        return await Context.Topsides
                        .Include(t => t.CostProfile)
                        .FirstOrDefaultAsync(t => t.Id == topsideId);
    }

    public async Task<bool> TopsideHasCostProfileOverride(Guid topsideId)
    {
        return await Context.Topsides
            .AnyAsync(t => t.Id == topsideId && t.CostProfileOverride != null);
    }
}
