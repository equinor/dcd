using api.Models;

namespace api.Features.Assets.CaseAssets.Substructures.Repositories;

public interface ISubstructureRepository
{
    Task<Substructure?> GetSubstructure(Guid substructureId);
    Task<Substructure?> GetSubstructureWithCostProfile(Guid substructureId);
    Task<bool> SubstructureHasCostProfileOverride(Guid substructureId);
}
