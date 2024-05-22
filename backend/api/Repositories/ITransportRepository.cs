using api.Models;

namespace api.Repositories;

public interface ITransportRepository
{
    Task<Transport?> GetTransport(Guid transportId);
    Task<Transport> UpdateTransport(Transport transport);
    Task<TransportCostProfileOverride?> GetTransportCostProfileOverride(Guid transportCostProfileOverrideId);
    Task<TransportCostProfileOverride> UpdateTransportCostProfileOverride(TransportCostProfileOverride transportCostProfileOverride);
}
