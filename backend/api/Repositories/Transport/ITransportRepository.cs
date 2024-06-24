using api.Models;

namespace api.Repositories;

public interface ITransportRepository : IBaseRepository
{
    Task<Transport?> GetTransport(Guid transportId);
    Task<Transport?> GetTransportWithCostProfile(Guid transportId);
    Task<bool> TransportHasCostProfileOverride(Guid transportId);
    Transport UpdateTransport(Transport transport);
}
