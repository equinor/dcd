using api.Models;

namespace api.Repositories;

public interface ISubstructureRepository
{
    Task<Substructure?> GetSubstructure(Guid substructureId);
    Task<Substructure> UpdateSubstructure(Substructure substructure);
    Task<SubstructureCostProfileOverride?> GetSubstructureCostProfileOverride(Guid substructureCostProfileOverrideId);
    Task<SubstructureCostProfileOverride> UpdateSubstructureCostProfileOverride(SubstructureCostProfileOverride substructureCostProfileOverride);
}
