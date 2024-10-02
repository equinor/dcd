using System.Linq.Expressions;

using api.Models;

namespace api.Repositories;

public interface ISubstructureRepository : IBaseRepository
{
    Task<Substructure?> GetSubstructure(Guid substructureId);
    Task<Substructure?> GetSubstructureWithIncludes(Guid substructureId, params Expression<Func<Substructure, object>>[] includes);
    Task<Substructure?> GetSubstructureWithCostProfile(Guid substructureId);
    Task<bool> SubstructureHasCostProfileOverride(Guid substructureId);
    Substructure UpdateSubstructure(Substructure substructure);
}
