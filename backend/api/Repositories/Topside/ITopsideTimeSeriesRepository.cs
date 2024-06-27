using api.Models;

namespace api.Repositories;

public interface ITopsideTimeSeriesRepository : IBaseRepository
{
    TopsideCostProfile CreateTopsideCostProfile(TopsideCostProfile topsideCostProfile);
    Task<TopsideCostProfile?> GetTopsideCostProfile(Guid topsideCostProfileId);
    TopsideCostProfile UpdateTopsideCostProfile(TopsideCostProfile topsideCostProfile);
    TopsideCostProfileOverride CreateTopsideCostProfileOverride(TopsideCostProfileOverride profile);
    Task<TopsideCostProfileOverride?> GetTopsideCostProfileOverride(Guid topsideCostProfileOverrideId);
    TopsideCostProfileOverride UpdateTopsideCostProfileOverride(TopsideCostProfileOverride topsideCostProfileOverride);
}
