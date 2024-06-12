using api.Models;

namespace api.Repositories;

public interface ITopsideRepository : IBaseRepository
{
    Task<Topside?> GetTopside(Guid topsideId);
    Task<Topside?> GetTopsideWithCostProfile(Guid topsideId);
    Topside UpdateTopside(Topside topside);
    TopsideCostProfile CreateTopsideCostProfile(TopsideCostProfile topsideCostProfile);
    Task<TopsideCostProfile?> GetTopsideCostProfile(Guid topsideCostProfileId);
    TopsideCostProfile UpdateTopsideCostProfile(TopsideCostProfile topsideCostProfile);
    Task<TopsideCostProfileOverride?> GetTopsideCostProfileOverride(Guid topsideCostProfileOverrideId);
    TopsideCostProfileOverride UpdateTopsideCostProfileOverride(TopsideCostProfileOverride topsideCostProfileOverride);
}
