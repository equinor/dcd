using api.Models;

namespace api.Repositories;

public interface ITopsideRepository
{
    Task<Topside?> GetTopside(Guid topsideId);
    Task<Topside> UpdateTopside(Topside topside);
    Task<TopsideCostProfileOverride?> GetTopsideCostProfileOverride(Guid topsideCostProfileOverrideId);
    Task<TopsideCostProfileOverride> UpdateTopsideCostProfileOverride(TopsideCostProfileOverride topsideCostProfileOverride);
}
