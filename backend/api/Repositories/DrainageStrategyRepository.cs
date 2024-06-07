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
        return await _context.DrainageStrategies.FindAsync(drainageStrategyId);
    }

    public DrainageStrategy UpdateDrainageStrategy(DrainageStrategy drainageStrategy)
    {
        _context.DrainageStrategies.Update(drainageStrategy);
        return drainageStrategy;
    }

    public async Task<ProductionProfileOil?> GetProductionProfileOil(Guid productionProfileOilId)
    {
        return await _context.ProductionProfileOil.FindAsync(productionProfileOilId);
    }

    public ProductionProfileOil UpdateProductionProfileOil(ProductionProfileOil productionProfileOil)
    {
        _context.ProductionProfileOil.Update(productionProfileOil);
        return productionProfileOil;
    }

    public async Task<ProductionProfileGas?> GetProductionProfileGas(Guid productionProfileId)
    {
        return await _context.ProductionProfileGas.FindAsync(productionProfileId);
    }

    public ProductionProfileGas UpdateProductionProfileGas(ProductionProfileGas productionProfile)
    {
        _context.ProductionProfileGas.Update(productionProfile);
        return productionProfile;
    }

    public async Task<ProductionProfileWater?> GetProductionProfileWater(Guid productionProfileId)
    {
        return await _context.ProductionProfileWater.FindAsync(productionProfileId);
    }

    public ProductionProfileWater UpdateProductionProfileWater(ProductionProfileWater productionProfile)
    {
        _context.ProductionProfileWater.Update(productionProfile);
        return productionProfile;
    }

    public async Task<ProductionProfileWaterInjection?> GetProductionProfileWaterInjection(Guid productionProfileId)
    {
        return await _context.ProductionProfileWaterInjection.FindAsync(productionProfileId);
    }

    public ProductionProfileWaterInjection UpdateProductionProfileWaterInjection(ProductionProfileWaterInjection productionProfile)
    {
        _context.ProductionProfileWaterInjection.Update(productionProfile);
        return productionProfile;
    }

    public async Task<FuelFlaringAndLossesOverride?> GetFuelFlaringAndLossesOverride(Guid profileId)
    {
        return await _context.FuelFlaringAndLossesOverride.FindAsync(profileId);
    }

    public FuelFlaringAndLossesOverride UpdateFuelFlaringAndLossesOverride(FuelFlaringAndLossesOverride profileId)
    {
        _context.FuelFlaringAndLossesOverride.Update(profileId);
        return profileId;
    }

    public async Task<NetSalesGasOverride?> GetNetSalesGasOverride(Guid profileId)
    {
        return await _context.NetSalesGasOverride.FindAsync(profileId);
    }

    public NetSalesGasOverride UpdateNetSalesGasOverride(NetSalesGasOverride profileId)
    {
        _context.NetSalesGasOverride.Update(profileId);
        return profileId;
    }

    public async Task<Co2EmissionsOverride?> GetCo2EmissionsOverride(Guid profileId)
    {
        return await _context.Co2EmissionsOverride.FindAsync(profileId);
    }

    public Co2EmissionsOverride UpdateCo2EmissionsOverride(Co2EmissionsOverride profileId)
    {
        _context.Co2EmissionsOverride.Update(profileId);
        return profileId;
    }

    public async Task<ImportedElectricityOverride?> GetImportedElectricityOverride(Guid profileId)
    {
        return await _context.ImportedElectricityOverride.FindAsync(profileId);
    }

    public ImportedElectricityOverride UpdateImportedElectricityOverride(ImportedElectricityOverride profileId)
    {
        _context.ImportedElectricityOverride.Update(profileId);
        return profileId;
    }

    public async Task<DeferredOilProduction?> GetDeferredOilProduction(Guid productionProfileId)
    {
        return await _context.DeferredOilProduction.FindAsync(productionProfileId);
    }

    public DeferredOilProduction UpdateDeferredOilProduction(DeferredOilProduction productionProfile)
    {
        _context.DeferredOilProduction.Update(productionProfile);
        return productionProfile;
    }

    public async Task<DeferredGasProduction?> GetDeferredGasProduction(Guid productionProfileId)
    {
        return await _context.DeferredGasProduction.FindAsync(productionProfileId);
    }

    public DeferredGasProduction UpdateDeferredGasProduction(DeferredGasProduction productionProfile)
    {
        _context.DeferredGasProduction.Update(productionProfile);
        return productionProfile;
    }
}
