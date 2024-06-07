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
        return await _context.Transports.FindAsync(transportId);
    }

        public async Task<Transport?> GetTransportWithCostProfile(Guid transportId)
    {
        return await _context.Transports
                        .Include(t => t.CostProfile)
                        .FirstOrDefaultAsync(t => t.Id == transportId);
    }

    public Transport UpdateTransport(Transport transport)
    {
        _context.Transports.Update(transport);
        return transport;
    }

    public TransportCostProfile CreateTransportCostProfile(TransportCostProfile transportCostProfile)
    {
        _context.TransportCostProfile.Add(transportCostProfile);
        return transportCostProfile;
    }

    public async Task<TransportCostProfile?> GetTransportCostProfile(Guid transportCostProfileId)
    {
        return await Get<TransportCostProfile>(transportCostProfileId);
    }

    public TransportCostProfile UpdateTransportCostProfile(TransportCostProfile transportCostProfile)
    {
        return Update(transportCostProfile);
    }

    public async Task<TransportCostProfileOverride?> GetTransportCostProfileOverride(Guid transportCostProfileOverrideId)
    {
        return await _context.TransportCostProfileOverride.FindAsync(transportCostProfileOverrideId);
    }

    public TransportCostProfileOverride UpdateTransportCostProfileOverride(TransportCostProfileOverride transportCostProfileOverride)
    {
        _context.TransportCostProfileOverride.Update(transportCostProfileOverride);
        return transportCostProfileOverride;
    }
}
