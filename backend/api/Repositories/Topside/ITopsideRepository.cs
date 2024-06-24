using api.Models;

namespace api.Repositories;

public interface ITopsideRepository : IBaseRepository
{
    Task<Topside?> GetTopside(Guid topsideId);
    Task<bool> TopsideHasCostProfileOverride(Guid topsideId);
    Task<Topside?> GetTopsideWithCostProfile(Guid topsideId);
    Topside UpdateTopside(Topside topside);
}
