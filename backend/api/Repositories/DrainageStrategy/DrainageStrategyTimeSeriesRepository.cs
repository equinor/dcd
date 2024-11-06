using api.Context;
using api.Models;


namespace api.Repositories;

public class DrainageStrategyTimeSeriesRepository(DcdDbContext context) : BaseRepository(context), IDrainageStrategyTimeSeriesRepository
{
    public ProductionProfileOil CreateProductionProfileOil(ProductionProfileOil productionProfileOil)
    {
        Context.ProductionProfileOil.Add(productionProfileOil);
        return productionProfileOil;
    }

    public async Task<ProductionProfileOil?> GetProductionProfileOil(Guid productionProfileOilId)
    {
        return await GetWithIncludes<ProductionProfileOil>(productionProfileOilId, d => d.DrainageStrategy);
    }

    public ProductionProfileOil UpdateProductionProfileOil(ProductionProfileOil productionProfileOil)
    {
        return Update(productionProfileOil);
    }

    public AdditionalProductionProfileOil CreateAdditionalProductionProfileOil(AdditionalProductionProfileOil additionalProductionProfileOil)
    {
        Context.AdditionalProductionProfileOil.Add(additionalProductionProfileOil);
        return additionalProductionProfileOil;
    }

    public async Task<AdditionalProductionProfileOil?> GetAdditionalProductionProfileOil(Guid additionalProductionProfileOilId)
    {
        return await GetWithIncludes<AdditionalProductionProfileOil>(additionalProductionProfileOilId, d => d.DrainageStrategy);
    }

    public AdditionalProductionProfileOil UpdateAdditionalProductionProfileOil(AdditionalProductionProfileOil additionalProductionProfileOil)
    {
        return Update(additionalProductionProfileOil);
    }

    public ProductionProfileGas CreateProductionProfileGas(ProductionProfileGas profile)
    {
        Context.ProductionProfileGas.Add(profile);
        return profile;
    }

    public async Task<ProductionProfileGas?> GetProductionProfileGas(Guid productionProfileId)
    {
        return await GetWithIncludes<ProductionProfileGas>(productionProfileId, d => d.DrainageStrategy);
    }

    public ProductionProfileGas UpdateProductionProfileGas(ProductionProfileGas productionProfile)
    {
        return Update(productionProfile);
    }

    public AdditionalProductionProfileGas CreateAdditionalProductionProfileGas(AdditionalProductionProfileGas profile)
    {
        Context.AdditionalProductionProfileGas.Add(profile);
        return profile;
    }

    public async Task<AdditionalProductionProfileGas?> GetAdditionalProductionProfileGas(Guid productionProfileId)
    {
        return await GetWithIncludes<AdditionalProductionProfileGas>(productionProfileId, d => d.DrainageStrategy);
    }

    public AdditionalProductionProfileGas UpdateAdditionalProductionProfileGas(AdditionalProductionProfileGas productionProfile)
    {
        return Update(productionProfile);
    }

    public ProductionProfileWater CreateProductionProfileWater(ProductionProfileWater profile)
    {
        Context.ProductionProfileWater.Add(profile);
        return profile;
    }

    public async Task<ProductionProfileWater?> GetProductionProfileWater(Guid productionProfileId)
    {
        return await GetWithIncludes<ProductionProfileWater>(productionProfileId, d => d.DrainageStrategy);
    }

    public ProductionProfileWater UpdateProductionProfileWater(ProductionProfileWater productionProfile)
    {
        return Update(productionProfile);
    }

    public ProductionProfileWaterInjection CreateProductionProfileWaterInjection(ProductionProfileWaterInjection profile)
    {
        Context.ProductionProfileWaterInjection.Add(profile);
        return profile;
    }
    public async Task<ProductionProfileWaterInjection?> GetProductionProfileWaterInjection(Guid productionProfileId)
    {
        return await GetWithIncludes<ProductionProfileWaterInjection>(productionProfileId, d => d.DrainageStrategy);
    }


    public ProductionProfileWaterInjection UpdateProductionProfileWaterInjection(ProductionProfileWaterInjection productionProfile)
    {
        return Update(productionProfile);
    }

    public FuelFlaringAndLossesOverride CreateFuelFlaringAndLossesOverride(FuelFlaringAndLossesOverride profile)
    {
        Context.FuelFlaringAndLossesOverride.Add(profile);
        return profile;
    }

    public async Task<FuelFlaringAndLossesOverride?> GetFuelFlaringAndLossesOverride(Guid profileId)
    {
        return await GetWithIncludes<FuelFlaringAndLossesOverride>(profileId, d => d.DrainageStrategy);
    }


    public FuelFlaringAndLossesOverride UpdateFuelFlaringAndLossesOverride(FuelFlaringAndLossesOverride profile)
    {
        return Update(profile);
    }

    public NetSalesGasOverride CreateNetSalesGasOverride(NetSalesGasOverride profile)
    {
        Context.NetSalesGasOverride.Add(profile);
        return profile;
    }

    public async Task<NetSalesGasOverride?> GetNetSalesGasOverride(Guid profileId)
    {
        return await GetWithIncludes<NetSalesGasOverride>(profileId, d => d.DrainageStrategy);
    }


    public NetSalesGasOverride UpdateNetSalesGasOverride(NetSalesGasOverride profile)
    {
        return Update(profile);
    }

    public Co2EmissionsOverride CreateCo2EmissionsOverride(Co2EmissionsOverride profile)
    {
        Context.Co2EmissionsOverride.Add(profile);
        return profile;
    }

    public async Task<Co2EmissionsOverride?> GetCo2EmissionsOverride(Guid profileId)
    {
        return await GetWithIncludes<Co2EmissionsOverride>(profileId, d => d.DrainageStrategy);
    }


    public Co2EmissionsOverride UpdateCo2EmissionsOverride(Co2EmissionsOverride profile)
    {
        return Update(profile);
    }

    public ImportedElectricityOverride CreateImportedElectricityOverride(ImportedElectricityOverride profile)
    {
        Context.ImportedElectricityOverride.Add(profile);
        return profile;
    }

    public async Task<ImportedElectricityOverride?> GetImportedElectricityOverride(Guid profileId)
    {
        return await GetWithIncludes<ImportedElectricityOverride>(profileId, d => d.DrainageStrategy);
    }


    public ImportedElectricityOverride UpdateImportedElectricityOverride(ImportedElectricityOverride profile)
    {
        return Update(profile);
    }

    public DeferredOilProduction CreateDeferredOilProduction(DeferredOilProduction profile)
    {
        Context.DeferredOilProduction.Add(profile);
        return profile;
    }

    public async Task<DeferredOilProduction?> GetDeferredOilProduction(Guid productionProfileId)
    {
        return await GetWithIncludes<DeferredOilProduction>(productionProfileId, d => d.DrainageStrategy);
    }


    public DeferredOilProduction UpdateDeferredOilProduction(DeferredOilProduction productionProfile)
    {
        return Update(productionProfile);
    }

    public DeferredGasProduction CreateDeferredGasProduction(DeferredGasProduction profile)
    {
        Context.DeferredGasProduction.Add(profile);
        return profile;
    }

    public async Task<DeferredGasProduction?> GetDeferredGasProduction(Guid productionProfileId)
    {
        return await GetWithIncludes<DeferredGasProduction>(productionProfileId, d => d.DrainageStrategy);
    }


    public DeferredGasProduction UpdateDeferredGasProduction(DeferredGasProduction productionProfile)
    {
        return Update(productionProfile);
    }
}
