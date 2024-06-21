using api.Models;

namespace api.Repositories;

public interface ISubstructureRepository : IBaseRepository
{
    Task<Substructure?> GetSubstructure(Guid substructureId);
    Task<Substructure?> GetSubstructureWithCostProfile(Guid substructureId);
    Task<bool> SubstructureHasCostProfileOverride(Guid substructureId);
    Substructure UpdateSubstructure(Substructure substructure);
    SubstructureCostProfile CreateSubstructureCostProfile(SubstructureCostProfile substructureCostProfile);
    Task<SubstructureCostProfile?> GetSubstructureCostProfile(Guid substructureCostProfileId);
    SubstructureCostProfile UpdateSubstructureCostProfile(SubstructureCostProfile substructureCostProfile);
    SubstructureCostProfileOverride CreateSubstructureCostProfileOverride(SubstructureCostProfileOverride profile);
    Task<SubstructureCostProfileOverride?> GetSubstructureCostProfileOverride(Guid substructureCostProfileOverrideId);
    SubstructureCostProfileOverride UpdateSubstructureCostProfileOverride(SubstructureCostProfileOverride substructureCostProfileOverride);
}
