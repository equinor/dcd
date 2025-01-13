using api.Models;

namespace api.Features.Assets.CaseAssets.Surfs.Repositories;

public interface ISurfRepository
{
    Task<Surf?> GetSurf(Guid surfId);
    Task<Surf?> GetSurfWithCostProfile(Guid surfId);
    Task<bool> SurfHasCostProfileOverride(Guid topsideId);
}
