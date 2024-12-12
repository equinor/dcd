using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.Substructures.Repositories;

public interface ISubstructureTimeSeriesRepository
{
    SubstructureCostProfile CreateSubstructureCostProfile(SubstructureCostProfile substructureCostProfile);
    Task<SubstructureCostProfile?> GetSubstructureCostProfile(Guid substructureCostProfileId);
    SubstructureCostProfile UpdateSubstructureCostProfile(SubstructureCostProfile substructureCostProfile);
    SubstructureCostProfileOverride CreateSubstructureCostProfileOverride(SubstructureCostProfileOverride profile);
    Task<SubstructureCostProfileOverride?> GetSubstructureCostProfileOverride(Guid substructureCostProfileOverrideId);
    SubstructureCostProfileOverride UpdateSubstructureCostProfileOverride(SubstructureCostProfileOverride substructureCostProfileOverride);
}
