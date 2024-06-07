using api.Models;

namespace api.Repositories;

public interface IDrainageStrategyRepository : IBaseRepository
{
    Task<DrainageStrategy?> GetDrainageStrategy(Guid drainageStrategyId);
    DrainageStrategy UpdateDrainageStrategy(DrainageStrategy drainageStrategy);
    Task<ProductionProfileOil?> GetProductionProfileOil(Guid productionProfileOilId);
    ProductionProfileOil UpdateProductionProfileOil(ProductionProfileOil productionProfileOil);
    Task<ProductionProfileGas?> GetProductionProfileGas(Guid productionProfileId);
    ProductionProfileGas UpdateProductionProfileGas(ProductionProfileGas productionProfile);
    Task<ProductionProfileWater?> GetProductionProfileWater(Guid productionProfileId);
    ProductionProfileWater UpdateProductionProfileWater(ProductionProfileWater productionProfile);
    Task<ProductionProfileWaterInjection?> GetProductionProfileWaterInjection(Guid productionProfileId);
    ProductionProfileWaterInjection UpdateProductionProfileWaterInjection(ProductionProfileWaterInjection productionProfile);
    Task<FuelFlaringAndLossesOverride?> GetFuelFlaringAndLossesOverride(Guid profileId);
    FuelFlaringAndLossesOverride UpdateFuelFlaringAndLossesOverride(FuelFlaringAndLossesOverride profileId);
    Task<NetSalesGasOverride?> GetNetSalesGasOverride(Guid profileId);
    NetSalesGasOverride UpdateNetSalesGasOverride(NetSalesGasOverride profileId);
    Task<Co2EmissionsOverride?> GetCo2EmissionsOverride(Guid profileId);
    Co2EmissionsOverride UpdateCo2EmissionsOverride(Co2EmissionsOverride profileId);
    Task<ImportedElectricityOverride?> GetImportedElectricityOverride(Guid profileId);
    ImportedElectricityOverride UpdateImportedElectricityOverride(ImportedElectricityOverride profileId);
    Task<DeferredOilProduction?> GetDeferredOilProduction(Guid productionProfileId);
    DeferredOilProduction UpdateDeferredOilProduction(DeferredOilProduction productionProfile);
    Task<DeferredGasProduction?> GetDeferredGasProduction(Guid productionProfileId);
    DeferredGasProduction UpdateDeferredGasProduction(DeferredGasProduction productionProfile);
}
