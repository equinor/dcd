using System.Linq.Expressions;

using api.Models;

namespace api.Repositories;

public interface ITransportRepository : IBaseRepository
{
    Task<Transport?> GetTransport(Guid transportId);
    Task<Transport?> GetTransportWithIncludes(Guid transportId, params Expression<Func<Transport, object>>[] includes);
    Task<Transport?> GetTransportWithCostProfile(Guid transportId);
    Task<bool> TransportHasCostProfileOverride(Guid transportId);
    Transport UpdateTransport(Transport transport);
}
