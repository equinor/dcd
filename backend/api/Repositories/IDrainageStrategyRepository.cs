using api.Models;

namespace api.Repositories;

public interface IDrainageStrategyRepository
{
    Task<DrainageStrategy?> GetDrainageStrategy(Guid drainageStrategyId);
    Task<DrainageStrategy> UpdateDrainageStrategy(DrainageStrategy drainageStrategy);
    Task<ProductionProfileOil?> GetProductionProfileOil(Guid productionProfileOilId);
    Task<ProductionProfileOil> UpdateProductionProfileOil(ProductionProfileOil productionProfileOil);
    Task<ProductionProfileGas?> GetProductionProfileGas(Guid productionProfileId);
    Task<ProductionProfileGas> UpdateProductionProfileGas(ProductionProfileGas productionProfile);
    Task<ProductionProfileWater?> GetProductionProfileWater(Guid productionProfileId);
    Task<ProductionProfileWater> UpdateProductionProfileWater(ProductionProfileWater productionProfile);
    Task<ProductionProfileWaterInjection?> GetProductionProfileWaterInjection(Guid productionProfileId);
    Task<ProductionProfileWaterInjection> UpdateProductionProfileWaterInjection(ProductionProfileWaterInjection productionProfile);
}
