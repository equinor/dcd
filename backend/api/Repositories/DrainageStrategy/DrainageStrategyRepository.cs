using System.Linq.Expressions;

using api.Context;
using api.Enums;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class DrainageStrategyRepository : BaseRepository, IDrainageStrategyRepository
{

    public DrainageStrategyRepository(DcdDbContext context) : base(context)
    {
    }

    public async Task<DrainageStrategy?> GetDrainageStrategy(Guid drainageStrategyId)
    {
        return await Get<DrainageStrategy>(drainageStrategyId);
    }

    public async Task<bool> DrainageStrategyHasProfile(Guid drainageStrategyId, DrainageStrategyProfileNames profileType)
    {
        Expression<Func<DrainageStrategy, bool>> profileExistsExpression = profileType switch
        {
            DrainageStrategyProfileNames.ProductionProfileOil => d => d.ProductionProfileOil != null,
            DrainageStrategyProfileNames.ProductionProfileGas => d => d.ProductionProfileGas != null,
            DrainageStrategyProfileNames.ProductionProfileWater => d => d.ProductionProfileWater != null,
            DrainageStrategyProfileNames.ProductionProfileWaterInjection => d => d.ProductionProfileWaterInjection != null,
            DrainageStrategyProfileNames.FuelFlaringAndLossesOverride => d => d.FuelFlaringAndLossesOverride != null,
            DrainageStrategyProfileNames.NetSalesGasOverride => d => d.NetSalesGasOverride != null,
            DrainageStrategyProfileNames.Co2EmissionsOverride => d => d.Co2EmissionsOverride != null,
            DrainageStrategyProfileNames.ImportedElectricityOverride => d => d.ImportedElectricityOverride != null,
            DrainageStrategyProfileNames.DeferredOilProduction => d => d.DeferredOilProduction != null,
            DrainageStrategyProfileNames.DeferredGasProduction => d => d.DeferredGasProduction != null,
        };

        bool hasProfile = await _context.DrainageStrategies
            .Where(d => d.Id == drainageStrategyId)
            .AnyAsync(profileExistsExpression);

        return hasProfile;
    }

    public DrainageStrategy UpdateDrainageStrategy(DrainageStrategy drainageStrategy)
    {
        return Update(drainageStrategy);
    }
}
