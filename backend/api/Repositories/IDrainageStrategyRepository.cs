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
    Task<FuelFlaringAndLossesOverride?> GetFuelFlaringAndLossesOverride(Guid profileId);
    Task<FuelFlaringAndLossesOverride> UpdateFuelFlaringAndLossesOverride(FuelFlaringAndLossesOverride profileId);
    Task<NetSalesGasOverride?> GetNetSalesGasOverride(Guid profileId);
    Task<NetSalesGasOverride> UpdateNetSalesGasOverride(NetSalesGasOverride profileId);
    Task<Co2EmissionsOverride?> GetCo2EmissionsOverride(Guid profileId);
    Task<Co2EmissionsOverride> UpdateCo2EmissionsOverride(Co2EmissionsOverride profileId);
    Task<ImportedElectricityOverride?> GetImportedElectricityOverride(Guid profileId);
    Task<ImportedElectricityOverride> UpdateImportedElectricityOverride(ImportedElectricityOverride profileId);
}
