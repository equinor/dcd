using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Substructures.Repositories;

public class SubstructureRepository(DcdDbContext context) : BaseRepository(context), ISubstructureRepository
{
    public async Task<Substructure?> GetSubstructure(Guid substructureId)
    {
        return await Get<Substructure>(substructureId);
    }

    public async Task<Substructure?> GetSubstructureWithCostProfile(Guid substructureId)
    {
        return await Context.Substructures
                        .Include(t => t.CostProfile)
                        .FirstOrDefaultAsync(t => t.Id == substructureId);
    }

    public async Task<bool> SubstructureHasCostProfileOverride(Guid substructureId)
    {
        return await Context.Substructures
            .AnyAsync(t => t.Id == substructureId && t.CostProfileOverride != null);
    }
}
