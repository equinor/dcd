using api.Models;

namespace api.Repositories;

public interface ISurfRepository
{
    Task<Surf?> GetSurf(Guid surfId);
    Task<Surf> UpdateSurf(Surf surf);
    Task<SurfCostProfileOverride?> GetSurfCostProfileOverride(Guid surfCostProfileOverrideId);
    Task<SurfCostProfileOverride> UpdateSurfCostProfileOverride(SurfCostProfileOverride surfCostProfileOverride);
}
