using api.Models;

namespace api.Features.Assets.CaseAssets.Topsides.Repositories;

public interface ITopsideRepository
{
    Task<Topside?> GetTopside(Guid topsideId);
    Task<bool> TopsideHasCostProfileOverride(Guid topsideId);
    Task<Topside?> GetTopsideWithCostProfile(Guid topsideId);
}
