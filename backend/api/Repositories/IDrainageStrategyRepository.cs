using api.Enums;
using api.Models;

namespace api.Repositories;

public interface IDrainageStrategyRepository : IBaseRepository
{
    Task<DrainageStrategy?> GetDrainageStrategy(Guid drainageStrategyId);
    Task<bool> DrainageStrategyHasProfile(Guid drainageStrategyId, DrainageStrategyProfileNames profileType);
    DrainageStrategy UpdateDrainageStrategy(DrainageStrategy drainageStrategy);
    ProductionProfileOil CreateProductionProfileOil(ProductionProfileOil productionProfileOil);
    Task<ProductionProfileOil?> GetProductionProfileOil(Guid productionProfileOilId);
    ProductionProfileOil UpdateProductionProfileOil(ProductionProfileOil productionProfileOil);
    ProductionProfileGas CreateProductionProfileGas(ProductionProfileGas profile);
    Task<ProductionProfileGas?> GetProductionProfileGas(Guid productionProfileId);
    ProductionProfileGas UpdateProductionProfileGas(ProductionProfileGas productionProfile);
    ProductionProfileWater CreateProductionProfileWater(ProductionProfileWater profile);
    Task<ProductionProfileWater?> GetProductionProfileWater(Guid productionProfileId);
    ProductionProfileWater UpdateProductionProfileWater(ProductionProfileWater productionProfile);
    ProductionProfileWaterInjection CreateProductionProfileWaterInjection(ProductionProfileWaterInjection profile);
    Task<ProductionProfileWaterInjection?> GetProductionProfileWaterInjection(Guid productionProfileId);
    ProductionProfileWaterInjection UpdateProductionProfileWaterInjection(ProductionProfileWaterInjection productionProfile);
    FuelFlaringAndLossesOverride CreateFuelFlaringAndLossesOverride(FuelFlaringAndLossesOverride profile);
    Task<FuelFlaringAndLossesOverride?> GetFuelFlaringAndLossesOverride(Guid profileId);
    FuelFlaringAndLossesOverride UpdateFuelFlaringAndLossesOverride(FuelFlaringAndLossesOverride profile);
    NetSalesGasOverride CreateNetSalesGasOverride(NetSalesGasOverride profile);
    Task<NetSalesGasOverride?> GetNetSalesGasOverride(Guid profileId);
    NetSalesGasOverride UpdateNetSalesGasOverride(NetSalesGasOverride profile);
    Co2EmissionsOverride CreateCo2EmissionsOverride(Co2EmissionsOverride profile);
    Task<Co2EmissionsOverride?> GetCo2EmissionsOverride(Guid profileId);
    Co2EmissionsOverride UpdateCo2EmissionsOverride(Co2EmissionsOverride profile);
    ImportedElectricityOverride CreateImportedElectricityOverride(ImportedElectricityOverride profile);
    Task<ImportedElectricityOverride?> GetImportedElectricityOverride(Guid profileId);
    ImportedElectricityOverride UpdateImportedElectricityOverride(ImportedElectricityOverride profile);
    DeferredOilProduction CreateDeferredOilProduction(DeferredOilProduction profile);
    Task<DeferredOilProduction?> GetDeferredOilProduction(Guid productionProfileId);
    DeferredOilProduction UpdateDeferredOilProduction(DeferredOilProduction productionProfile);
    DeferredGasProduction CreateDeferredGasProduction(DeferredGasProduction profile);
    Task<DeferredGasProduction?> GetDeferredGasProduction(Guid productionProfileId);
    DeferredGasProduction UpdateDeferredGasProduction(DeferredGasProduction productionProfile);
}
