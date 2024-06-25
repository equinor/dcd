using api.Models;

namespace api.Repositories;

public interface ISurfRepository : IBaseRepository
{
    Task<Surf?> GetSurf(Guid surfId);
    Task<Surf?> GetSurfWithCostProfile(Guid surfId);
    Task<bool> SurfHasCostProfileOverride(Guid topsideId);
    Surf UpdateSurf(Surf surf);
}
