using System.Linq.Expressions;

using api.Models;
using api.Repositories;

namespace api.Features.Assets.CaseAssets.Topsides.Repositories;

public interface ITopsideRepository : IBaseRepository
{
    Task<Topside?> GetTopside(Guid topsideId);
    Task<Topside?> GetTopsideWithIncludes(Guid topsideId, params Expression<Func<Topside, object>>[] includes);
    Task<bool> TopsideHasCostProfileOverride(Guid topsideId);
    Task<Topside?> GetTopsideWithCostProfile(Guid topsideId);
    Topside UpdateTopside(Topside topside);
}
