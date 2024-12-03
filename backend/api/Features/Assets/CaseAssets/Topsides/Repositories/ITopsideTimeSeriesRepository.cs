using api.Models;
using api.Repositories;

namespace api.Features.Assets.CaseAssets.Topsides.Repositories;

public interface ITopsideTimeSeriesRepository : IBaseRepository
{
    TopsideCostProfile CreateTopsideCostProfile(TopsideCostProfile topsideCostProfile);
    Task<TopsideCostProfile?> GetTopsideCostProfile(Guid topsideCostProfileId);
    TopsideCostProfile UpdateTopsideCostProfile(TopsideCostProfile topsideCostProfile);
    TopsideCostProfileOverride CreateTopsideCostProfileOverride(TopsideCostProfileOverride profile);
    Task<TopsideCostProfileOverride?> GetTopsideCostProfileOverride(Guid topsideCostProfileOverrideId);
    TopsideCostProfileOverride UpdateTopsideCostProfileOverride(TopsideCostProfileOverride topsideCostProfileOverride);
}
