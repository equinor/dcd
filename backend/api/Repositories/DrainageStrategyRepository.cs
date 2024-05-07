using api.Context;
using api.Models;


namespace api.Repositories;

public class DrainageStrategyRepository : IDrainageStrategyRepository
{
    private readonly DcdDbContext _context;

    public DrainageStrategyRepository(DcdDbContext context)
    {
        _context = context;
    }

    public async Task<DrainageStrategy?> GetDrainageStrategy(Guid drainageStrategyId)
    {
        return await _context.DrainageStrategies.FindAsync(drainageStrategyId);
    }

    public async Task<DrainageStrategy> UpdateDrainageStrategy(DrainageStrategy drainageStrategy)
    {
        _context.DrainageStrategies.Update(drainageStrategy);
        await _context.SaveChangesAsync();
        return drainageStrategy;
    }

    public async Task<ProductionProfileOil?> GetProductionProfileOil(Guid productionProfileOilId)
    {
        return await _context.ProductionProfileOil.FindAsync(productionProfileOilId);
    }

    public async Task<ProductionProfileOil> UpdateProductionProfileOil(ProductionProfileOil productionProfileOil)
    {
        _context.ProductionProfileOil.Update(productionProfileOil);
        await _context.SaveChangesAsync();
        return productionProfileOil;
    }
}
