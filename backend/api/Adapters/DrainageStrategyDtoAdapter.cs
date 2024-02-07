using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class DrainageStrategyDtoAdapter
{
    public static DrainageStrategyDto Convert(DrainageStrategy drainageStrategy, PhysUnit unit)
    {
        var drainageStrategyDto = new DrainageStrategyDto
        {
            Id = drainageStrategy.Id,
            Name = drainageStrategy.Name,
            Description = drainageStrategy.Description,
            ProjectId = drainageStrategy.ProjectId,
            NGLYield = drainageStrategy.NGLYield,
            ArtificialLift = drainageStrategy.ArtificialLift,
            GasSolution = drainageStrategy.GasSolution,
            ProducerCount = drainageStrategy.ProducerCount,
            GasInjectorCount = drainageStrategy.GasInjectorCount,
            WaterInjectorCount = drainageStrategy.WaterInjectorCount,
            ProductionProfileOil = Convert<ProductionProfileOilDto, ProductionProfileOil>(drainageStrategy.ProductionProfileOil, unit) ?? new ProductionProfileOilDto(),
            ProductionProfileGas = Convert<ProductionProfileGasDto, ProductionProfileGas>(drainageStrategy.ProductionProfileGas, unit) ?? new ProductionProfileGasDto(),
            ProductionProfileWater = Convert<ProductionProfileWaterDto, ProductionProfileWater>(drainageStrategy.ProductionProfileWater, unit) ?? new ProductionProfileWaterDto(),
            ProductionProfileWaterInjection = Convert<ProductionProfileWaterInjectionDto, ProductionProfileWaterInjection>(drainageStrategy.ProductionProfileWaterInjection, unit) ?? new ProductionProfileWaterInjectionDto(),
            FuelFlaringAndLosses = Convert<FuelFlaringAndLossesDto, FuelFlaringAndLosses>(drainageStrategy.FuelFlaringAndLosses, unit) ?? new FuelFlaringAndLossesDto(),
            FuelFlaringAndLossesOverride = ConvertOverride<FuelFlaringAndLossesOverrideDto, FuelFlaringAndLossesOverride>(drainageStrategy.FuelFlaringAndLossesOverride, unit) ?? new FuelFlaringAndLossesOverrideDto(),
            NetSalesGas = Convert<NetSalesGasDto, NetSalesGas>(drainageStrategy.NetSalesGas, unit) ?? new NetSalesGasDto(),
            NetSalesGasOverride = ConvertOverride<NetSalesGasOverrideDto, NetSalesGasOverride>(drainageStrategy.NetSalesGasOverride, unit) ?? new NetSalesGasOverrideDto(),
            Co2Emissions = Convert<Co2EmissionsDto, Co2Emissions>(drainageStrategy.Co2Emissions, unit) ?? new Co2EmissionsDto(),
            Co2EmissionsOverride = ConvertOverride<Co2EmissionsOverrideDto, Co2EmissionsOverride>(drainageStrategy.Co2EmissionsOverride, unit) ?? new Co2EmissionsOverrideDto(),
            ProductionProfileNGL = Convert<ProductionProfileNGLDto, ProductionProfileNGL>(drainageStrategy.ProductionProfileNGL, unit) ?? new ProductionProfileNGLDto(),
            ImportedElectricity = Convert<ImportedElectricityDto, ImportedElectricity>(drainageStrategy.ImportedElectricity, unit) ?? new ImportedElectricityDto(),
            ImportedElectricityOverride = ConvertOverride<ImportedElectricityOverrideDto, ImportedElectricityOverride>(drainageStrategy.ImportedElectricityOverride, unit) ?? new ImportedElectricityOverrideDto(),
        };
        return drainageStrategyDto;
    }

    public static TDto? ConvertOverride<TDto, TModel>(TModel? model, PhysUnit unit)
        where TDto : TimeSeriesDto<double>, ITimeSeriesOverrideDto, new()
        where TModel : TimeSeries<double>, ITimeSeriesOverride
    {
        if (model != null)
        {
            return new TDto
            {
                Id = model.Id,
                Override = model.Override,
                StartYear = model.StartYear,
                Values = ConvertUnitValues(model.Values, unit, model.GetType().Name)
            };
        }
        return null;
    }

    public static TDto? Convert<TDto, TModel>(TModel? model, PhysUnit unit)
    where TDto : TimeSeriesDto<double>, new()
    where TModel : TimeSeries<double>
    {
        if (model != null)
        {
            return new TDto
            {
                Id = model.Id,
                StartYear = model.StartYear,
                Values = ConvertUnitValues(model.Values, unit, model.GetType().Name)
            };
        }
        return null;
    }

    private static double[] ConvertUnitValues(double[] values, PhysUnit unit, string type)
    {
        string[] MTPA_Units = { nameof(Co2Emissions), nameof(Co2EmissionsOverride), nameof(ProductionProfileNGL) };
        string[] BBL_Units =
            { nameof(ProductionProfileOil), nameof(ProductionProfileWater), nameof(ProductionProfileWaterInjection) };
        string[] SCF_Units = { nameof(ProductionProfileGas), nameof(FuelFlaringAndLosses), nameof(FuelFlaringAndLossesOverride), nameof(NetSalesGas), nameof(NetSalesGasOverride) };

        // Per now - the timeseriestypes which use millions are the same in both SI and Oilfield
        if (SCF_Units.Contains(type))
        {
            // Trim zeroes for SCF when sending back to frontend
            values = Array.ConvertAll(values, x => x / 1E9);
        }
        else if (MTPA_Units.Contains(type) || BBL_Units.Contains(type))
        {
            // Trim zeroes for BBL when sending back to frontend
            values = Array.ConvertAll(values, x => x / 1E6);
        }

        // If Oilfield is selected, convert to respective values
        if (unit == PhysUnit.OilField && !MTPA_Units.Contains(type))
        {
            if (BBL_Units.Contains(type))
            {
                // Unit: From baseunit Sm3 to BBL
                values = Array.ConvertAll(values, x => x * 6.290);
            }
            else if (SCF_Units.Contains(type))
            {
                // Unit: From baseunit Sm3 to SCF
                values = Array.ConvertAll(values, x => x * 35.315);
            }
        }

        return values;
    }
}
