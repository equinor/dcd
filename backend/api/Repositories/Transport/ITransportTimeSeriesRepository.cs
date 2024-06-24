using api.Models;

namespace api.Repositories;

public interface ITransportTimeSeriesRepository : IBaseRepository
{
    TransportCostProfile CreateTransportCostProfile(TransportCostProfile transportCostProfile);
    Task<TransportCostProfile?> GetTransportCostProfile(Guid transportCostProfileId);
    TransportCostProfile UpdateTransportCostProfile(TransportCostProfile transportCostProfile);
    TransportCostProfileOverride CreateTransportCostProfileOverride(TransportCostProfileOverride profile);
    Task<TransportCostProfileOverride?> GetTransportCostProfileOverride(Guid transportCostProfileOverrideId);
    TransportCostProfileOverride UpdateTransportCostProfileOverride(TransportCostProfileOverride transportCostProfileOverride);
}
