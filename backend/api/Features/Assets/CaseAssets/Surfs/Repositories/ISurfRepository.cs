using System.Linq.Expressions;

using api.Models;

namespace api.Features.Assets.CaseAssets.Surfs.Repositories;

public interface ISurfRepository
{
    Task<Surf?> GetSurfWithIncludes(Guid surfId, params Expression<Func<Surf, object>>[] includes);
    Task<Surf?> GetSurf(Guid surfId);
    Task<Surf?> GetSurfWithCostProfile(Guid surfId);
    Task<bool> SurfHasCostProfileOverride(Guid topsideId);
    Surf UpdateSurf(Surf surf);
}
