using api.Models;
using api.Repositories;

namespace api.Features.Assets.CaseAssets.Transports.Repositories;

public interface ITransportTimeSeriesRepository : IBaseRepository
{
    TransportCostProfile CreateTransportCostProfile(TransportCostProfile transportCostProfile);
    Task<TransportCostProfile?> GetTransportCostProfile(Guid transportCostProfileId);
    TransportCostProfile UpdateTransportCostProfile(TransportCostProfile transportCostProfile);
    TransportCostProfileOverride CreateTransportCostProfileOverride(TransportCostProfileOverride profile);
    Task<TransportCostProfileOverride?> GetTransportCostProfileOverride(Guid transportCostProfileOverrideId);
    TransportCostProfileOverride UpdateTransportCostProfileOverride(TransportCostProfileOverride transportCostProfileOverride);
}
