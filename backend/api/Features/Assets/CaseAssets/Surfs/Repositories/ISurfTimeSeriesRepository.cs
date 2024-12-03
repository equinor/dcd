using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.Surfs.Repositories;

public interface ISurfTimeSeriesRepository : IBaseRepository
{
    SurfCostProfile CreateSurfCostProfile(SurfCostProfile surfCostProfile);
    Task<SurfCostProfile?> GetSurfCostProfile(Guid surfCostProfileId);
    SurfCostProfile UpdateSurfCostProfile(SurfCostProfile surfCostProfile);
    SurfCostProfileOverride CreateSurfCostProfileOverride(SurfCostProfileOverride profile);
    Task<SurfCostProfileOverride?> GetSurfCostProfileOverride(Guid surfCostProfileOverrideId);
    SurfCostProfileOverride UpdateSurfCostProfileOverride(SurfCostProfileOverride surfCostProfileOverride);
}
