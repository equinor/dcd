using System.Linq.Expressions;

using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Transports.Repositories;

public class TransportRepository(DcdDbContext context) : BaseRepository(context), ITransportRepository
{
    public async Task<Transport?> GetTransport(Guid transportId)
    {
        return await Get<Transport>(transportId);
    }

    public async Task<Transport?> GetTransportWithIncludes(Guid transportId, params Expression<Func<Transport, object>>[] includes)
    {
        return await GetWithIncludes(transportId, includes);
    }

    public async Task<Transport?> GetTransportWithCostProfile(Guid transportId)
    {
        return await Context.Transports
                        .Include(t => t.CostProfile)
                        .FirstOrDefaultAsync(t => t.Id == transportId);
    }

    public async Task<bool> TransportHasCostProfileOverride(Guid transportId)
    {
        return await Context.Transports
            .AnyAsync(t => t.Id == transportId && t.CostProfileOverride != null);
    }

    public Transport UpdateTransport(Transport transport)
    {
        return Update(transport);
    }
}
