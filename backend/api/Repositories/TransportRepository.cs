using api.Context;
using api.Models;


namespace api.Repositories;

public class TransportRepository : ITransportRepository
{
    private readonly DcdDbContext _context;

    public TransportRepository(DcdDbContext context)
    {
        _context = context;
    }

    public async Task<Transport?> GetTransport(Guid transportId)
    {
        return await _context.Transports.FindAsync(transportId);
    }

    public async Task<Transport> UpdateTransport(Transport transport)
    {
        _context.Transports.Update(transport);
        await _context.SaveChangesAsync();
        return transport;
    }

    public async Task<TransportCostProfileOverride?> GetTransportCostProfileOverride(Guid transportCostProfileOverrideId)
    {
        return await _context.TransportCostProfileOverride.FindAsync(transportCostProfileOverrideId);
    }

    public async Task<TransportCostProfileOverride> UpdateTransportCostProfileOverride(TransportCostProfileOverride transportCostProfileOverride)
    {
        _context.TransportCostProfileOverride.Update(transportCostProfileOverride);
        await _context.SaveChangesAsync();
        return transportCostProfileOverride;
    }
}
