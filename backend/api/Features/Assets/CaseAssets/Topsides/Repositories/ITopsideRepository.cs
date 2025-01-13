using System.Linq.Expressions;

using api.Models;

namespace api.Features.Assets.CaseAssets.Topsides.Repositories;

public interface ITopsideRepository
{
    Task<Topside?> GetTopside(Guid topsideId);
    Task<Topside?> GetTopsideWithIncludes(Guid topsideId, params Expression<Func<Topside, object>>[] includes);
    Task<bool> TopsideHasCostProfileOverride(Guid topsideId);
    Task<Topside?> GetTopsideWithCostProfile(Guid topsideId);
}
