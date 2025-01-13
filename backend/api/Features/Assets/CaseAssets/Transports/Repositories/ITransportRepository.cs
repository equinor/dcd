using api.Models;

namespace api.Features.Assets.CaseAssets.Transports.Repositories;

public interface ITransportRepository
{
    Task<Transport?> GetTransport(Guid transportId);
    Task<Transport?> GetTransportWithCostProfile(Guid transportId);
    Task<bool> TransportHasCostProfileOverride(Guid transportId);
}
