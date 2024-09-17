using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class TransportTimeSeriesRepository : BaseRepository, ITransportTimeSeriesRepository
{

    public TransportTimeSeriesRepository(DcdDbContext context) : base(context)
    {
    }

    public TransportCostProfile CreateTransportCostProfile(TransportCostProfile transportCostProfile)
    {
        _context.TransportCostProfile.Add(transportCostProfile);
        return transportCostProfile;
    }

    public async Task<TransportCostProfile?> GetTransportCostProfile(Guid transportCostProfileId)
    {
        return await GetWithIncludes<TransportCostProfile>(transportCostProfileId, t => t.Transport);
    }

    public TransportCostProfile UpdateTransportCostProfile(TransportCostProfile transportCostProfile)
    {
        return Update(transportCostProfile);
    }

    public TransportCostProfileOverride CreateTransportCostProfileOverride(TransportCostProfileOverride profile)
    {
        _context.TransportCostProfileOverride.Add(profile);
        return profile;
    }


    public async Task<TransportCostProfileOverride?> GetTransportCostProfileOverride(Guid transportCostProfileOverrideId)
    {
        return await GetWithIncludes<TransportCostProfileOverride>(transportCostProfileOverrideId, t => t.Transport);
    }

    public TransportCostProfileOverride UpdateTransportCostProfileOverride(TransportCostProfileOverride transportCostProfileOverride)
    {
        return Update(transportCostProfileOverride);
    }
}
