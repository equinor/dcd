using api.Models;

namespace api.Repositories;

public interface ISubstructureRepository : IBaseRepository
{
    Task<Substructure?> GetSubstructure(Guid substructureId);
    Task<Substructure?> GetSubstructureWithCostProfile(Guid substructureId);
    Task<bool> SubstructureHasCostProfileOverride(Guid substructureId);
    Substructure UpdateSubstructure(Substructure substructure);
}
