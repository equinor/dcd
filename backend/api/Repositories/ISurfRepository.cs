using api.Models;

namespace api.Repositories;

public interface ISurfRepository : IBaseRepository
{
    Task<Surf?> GetSurf(Guid surfId);
    Task<Surf?> GetSurfWithCostProfile(Guid surfId);
    Surf UpdateSurf(Surf surf);
    SurfCostProfile CreateSurfCostProfile(SurfCostProfile surfCostProfile);
    Task<SurfCostProfile?> GetSurfCostProfile(Guid surfCostProfileId);
    SurfCostProfile UpdateSurfCostProfile(SurfCostProfile surfCostProfile);
    Task<SurfCostProfileOverride?> GetSurfCostProfileOverride(Guid surfCostProfileOverrideId);
    SurfCostProfileOverride UpdateSurfCostProfileOverride(SurfCostProfileOverride surfCostProfileOverride);
}
