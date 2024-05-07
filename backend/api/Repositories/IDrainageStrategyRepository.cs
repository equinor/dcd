using api.Models;

namespace api.Repositories;

public interface IDrainageStrategyRepository
{
    Task<DrainageStrategy?> GetDrainageStrategy(Guid drainageStrategyId);
    Task<DrainageStrategy> UpdateDrainageStrategy(DrainageStrategy drainageStrategy);
    Task<ProductionProfileOil?> GetProductionProfileOil(Guid productionProfileOilId);
    Task<ProductionProfileOil> UpdateProductionProfileOil(ProductionProfileOil productionProfileOil);
}