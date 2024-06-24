using api.Models;

namespace api.Repositories;

public interface ISubstructureTimeSeriesRepository : IBaseRepository
{
    SubstructureCostProfile CreateSubstructureCostProfile(SubstructureCostProfile substructureCostProfile);
    Task<SubstructureCostProfile?> GetSubstructureCostProfile(Guid substructureCostProfileId);
    SubstructureCostProfile UpdateSubstructureCostProfile(SubstructureCostProfile substructureCostProfile);
    SubstructureCostProfileOverride CreateSubstructureCostProfileOverride(SubstructureCostProfileOverride profile);
    Task<SubstructureCostProfileOverride?> GetSubstructureCostProfileOverride(Guid substructureCostProfileOverrideId);
    SubstructureCostProfileOverride UpdateSubstructureCostProfileOverride(SubstructureCostProfileOverride substructureCostProfileOverride);
}
