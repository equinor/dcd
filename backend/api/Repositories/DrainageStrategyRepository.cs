using api.Context;
using api.Models;


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

    public DrainageStrategy UpdateDrainageStrategy(DrainageStrategy drainageStrategy)
    {
        return Update(drainageStrategy);
    }

    public async Task<ProductionProfileOil?> GetProductionProfileOil(Guid productionProfileOilId)
    {
        return await Get<ProductionProfileOil>(productionProfileOilId);
    }

    public ProductionProfileOil UpdateProductionProfileOil(ProductionProfileOil productionProfileOil)
    {
        return Update(productionProfileOil);
    }

    public async Task<ProductionProfileGas?> GetProductionProfileGas(Guid productionProfileId)
    {
        return await Get<ProductionProfileGas>(productionProfileId);
    }

    public ProductionProfileGas UpdateProductionProfileGas(ProductionProfileGas productionProfile)
    {
        return Update(productionProfile);
    }

    public async Task<ProductionProfileWater?> GetProductionProfileWater(Guid productionProfileId)
    {
        return await Get<ProductionProfileWater>(productionProfileId);
    }

    public ProductionProfileWater UpdateProductionProfileWater(ProductionProfileWater productionProfile)
    {
        return Update(productionProfile);
    }

    public async Task<ProductionProfileWaterInjection?> GetProductionProfileWaterInjection(Guid productionProfileId)
    {
        return await Get<ProductionProfileWaterInjection>(productionProfileId);
    }

    public ProductionProfileWaterInjection UpdateProductionProfileWaterInjection(ProductionProfileWaterInjection productionProfile)
    {
        return Update(productionProfile);
    }

    public async Task<FuelFlaringAndLossesOverride?> GetFuelFlaringAndLossesOverride(Guid profileId)
    {
        return await Get<FuelFlaringAndLossesOverride>(profileId);
    }

    public FuelFlaringAndLossesOverride UpdateFuelFlaringAndLossesOverride(FuelFlaringAndLossesOverride profile)
    {
        return Update(profile);
    }

    public async Task<NetSalesGasOverride?> GetNetSalesGasOverride(Guid profileId)
    {
        return await Get<NetSalesGasOverride>(profileId);
    }

    public NetSalesGasOverride UpdateNetSalesGasOverride(NetSalesGasOverride profile)
    {
        return Update(profile);
    }

    public async Task<Co2EmissionsOverride?> GetCo2EmissionsOverride(Guid profileId)
    {
        return await Get<Co2EmissionsOverride>(profileId);
    }

    public Co2EmissionsOverride UpdateCo2EmissionsOverride(Co2EmissionsOverride profile)
    {
        return Update(profile);
    }

    public async Task<ImportedElectricityOverride?> GetImportedElectricityOverride(Guid profileId)
    {
        return await Get<ImportedElectricityOverride>(profileId);
    }

    public ImportedElectricityOverride UpdateImportedElectricityOverride(ImportedElectricityOverride profile)
    {
        return Update(profile);
    }

    public async Task<DeferredOilProduction?> GetDeferredOilProduction(Guid productionProfileId)
    {
        return await Get<DeferredOilProduction>(productionProfileId);
    }

    public DeferredOilProduction UpdateDeferredOilProduction(DeferredOilProduction productionProfile)
    {
        return Update(productionProfile);
    }

    public async Task<DeferredGasProduction?> GetDeferredGasProduction(Guid productionProfileId)
    {
        return await Get<DeferredGasProduction>(productionProfileId);
    }

    public DeferredGasProduction UpdateDeferredGasProduction(DeferredGasProduction productionProfile)
    {
        return Update(productionProfile);
    }
}
