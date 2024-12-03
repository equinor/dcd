using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.Transports.Repositories;

public class TransportTimeSeriesRepository(DcdDbContext context) : BaseRepository(context), ITransportTimeSeriesRepository
{
    public TransportCostProfile CreateTransportCostProfile(TransportCostProfile transportCostProfile)
    {
        Context.TransportCostProfile.Add(transportCostProfile);
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
        Context.TransportCostProfileOverride.Add(profile);
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
