using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.Transports.Repositories;

public interface ITransportTimeSeriesRepository
{
    TransportCostProfile CreateTransportCostProfile(TransportCostProfile transportCostProfile);
    Task<TransportCostProfile?> GetTransportCostProfile(Guid transportCostProfileId);
    TransportCostProfile UpdateTransportCostProfile(TransportCostProfile transportCostProfile);
    TransportCostProfileOverride CreateTransportCostProfileOverride(TransportCostProfileOverride profile);
    Task<TransportCostProfileOverride?> GetTransportCostProfileOverride(Guid transportCostProfileOverrideId);
    TransportCostProfileOverride UpdateTransportCostProfileOverride(TransportCostProfileOverride transportCostProfileOverride);
}
