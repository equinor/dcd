using System.Linq.Expressions;

using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Models;

using Microsoft.EntityFrameworkCore;

public class OnshorePowerSupplyRepository(DcdDbContext context) : BaseRepository(context), IOnshorePowerSupplyRepository
{
    public async Task<OnshorePowerSupply?> GetOnshorePowerSupply(Guid onshorePowerSupplyId)
    {
        return await Get<OnshorePowerSupply>(onshorePowerSupplyId);
    }

    public async Task<OnshorePowerSupply?> GetOnshorePowerSupplyWithIncludes(Guid onshorePowerSupplyId, params Expression<Func<OnshorePowerSupply, object>>[] includes)
    {
        return await GetWithIncludes(onshorePowerSupplyId, includes);
    }

    public async Task<OnshorePowerSupply?> GetOnshorePowerSupplyWithCostProfile(Guid onshorePowerSupplyId)
    {
        return await Context.OnshorePowerSupplies
                        .Include(t => t.CostProfile)
                        .FirstOrDefaultAsync(t => t.Id == onshorePowerSupplyId);
    }

    public async Task<bool> OnshorePowerSupplyHasCostProfileOverride(Guid onshorePowerSupplyId)
    {
        return await Context.OnshorePowerSupplies
            .AnyAsync(t => t.Id == onshorePowerSupplyId && t.CostProfileOverride != null);
    }

    public OnshorePowerSupply UpdateOnshorePowerSupply(OnshorePowerSupply onshorePowerSupply)
    {
        return Update(onshorePowerSupply);
    }
}
