using System.Linq.Expressions;

using api.Context;
using api.Features.CaseProfiles.Enums;
using api.Features.CaseProfiles.Repositories;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.DrainageStrategies.Repositories;

public class DrainageStrategyRepository(DcdDbContext context) : BaseRepository(context), IDrainageStrategyRepository
{
    public async Task<DrainageStrategy?> GetDrainageStrategy(Guid drainageStrategyId)
    {
        return await Get<DrainageStrategy>(drainageStrategyId);
    }

    public async Task<DrainageStrategy?> GetDrainageStrategyWithIncludes(Guid drainageStrategyId, params Expression<Func<DrainageStrategy, object>>[] includes)
    {
        return await GetWithIncludes(drainageStrategyId, includes);
    }

    public async Task<bool> DrainageStrategyHasProfile(Guid drainageStrategyId, DrainageStrategyProfileNames profileType)
    {
        Expression<Func<DrainageStrategy, bool>> profileExistsExpression = profileType switch
        {
            DrainageStrategyProfileNames.ProductionProfileOil => d => d.ProductionProfileOil != null,
            DrainageStrategyProfileNames.AdditionalProductionProfileOil => d => d.AdditionalProductionProfileOil != null,
            DrainageStrategyProfileNames.ProductionProfileGas => d => d.ProductionProfileGas != null,
            DrainageStrategyProfileNames.AdditionalProductionProfileGas => d => d.AdditionalProductionProfileGas != null,
            DrainageStrategyProfileNames.ProductionProfileWater => d => d.ProductionProfileWater != null,
            DrainageStrategyProfileNames.ProductionProfileWaterInjection => d => d.ProductionProfileWaterInjection != null,
            DrainageStrategyProfileNames.FuelFlaringAndLossesOverride => d => d.FuelFlaringAndLossesOverride != null,
            DrainageStrategyProfileNames.NetSalesGasOverride => d => d.NetSalesGasOverride != null,
            DrainageStrategyProfileNames.Co2EmissionsOverride => d => d.Co2EmissionsOverride != null,
            DrainageStrategyProfileNames.ImportedElectricityOverride => d => d.ImportedElectricityOverride != null,
            DrainageStrategyProfileNames.DeferredOilProduction => d => d.DeferredOilProduction != null,
            DrainageStrategyProfileNames.DeferredGasProduction => d => d.DeferredGasProduction != null,
        };

        bool hasProfile = await Context.DrainageStrategies
            .Where(d => d.Id == drainageStrategyId)
            .AnyAsync(profileExistsExpression);

        return hasProfile;
    }
}
