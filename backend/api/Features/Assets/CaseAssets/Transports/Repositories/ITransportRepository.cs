using System.Linq.Expressions;

using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.Transports.Repositories;

public interface ITransportRepository
{
    Task<Transport?> GetTransport(Guid transportId);
    Task<Transport?> GetTransportWithIncludes(Guid transportId, params Expression<Func<Transport, object>>[] includes);
    Task<Transport?> GetTransportWithCostProfile(Guid transportId);
    Task<bool> TransportHasCostProfileOverride(Guid transportId);
    Transport UpdateTransport(Transport transport);
}
