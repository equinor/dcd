using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class TransportRepository : BaseRepository, ITransportRepository
{

    public TransportRepository(DcdDbContext context) : base(context)
    {
    }

    public async Task<Transport?> GetTransport(Guid transportId)
    {
        return await Get<Transport>(transportId);
    }

    public async Task<Transport?> GetTransportWithCostProfile(Guid transportId)
    {
        return await _context.Transports
                        .Include(t => t.CostProfile)
                        .FirstOrDefaultAsync(t => t.Id == transportId);
    }

    public async Task<bool> TransportHasCostProfileOverride(Guid transportId)
    {
        return await _context.Transports
            .AnyAsync(t => t.Id == transportId && t.CostProfileOverride != null);
    }

    public Transport UpdateTransport(Transport transport)
    {
        return Update(transport);
    }
}
