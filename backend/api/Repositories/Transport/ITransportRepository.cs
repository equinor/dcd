using api.Models;

namespace api.Repositories;

public interface ITransportRepository : IBaseRepository
{
    Task<Transport?> GetTransport(Guid transportId);
    Task<Transport?> GetTransportWithCostProfile(Guid transportId);
    Task<bool> TransportHasCostProfileOverride(Guid transportId);
    Transport UpdateTransport(Transport transport);
    TransportCostProfile CreateTransportCostProfile(TransportCostProfile transportCostProfile);
    Task<TransportCostProfile?> GetTransportCostProfile(Guid transportCostProfileId);
    TransportCostProfile UpdateTransportCostProfile(TransportCostProfile transportCostProfile);
    TransportCostProfileOverride CreateTransportCostProfileOverride(TransportCostProfileOverride profile);
    Task<TransportCostProfileOverride?> GetTransportCostProfileOverride(Guid transportCostProfileOverrideId);
    TransportCostProfileOverride UpdateTransportCostProfileOverride(TransportCostProfileOverride transportCostProfileOverride);
}