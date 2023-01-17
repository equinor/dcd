using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class DrainageStrategyAdapter
{
    public static DrainageStrategy Convert(DrainageStrategyDto drainageStrategyDto, PhysUnit unit, bool initialCreate)
    {
        var drainageStrategy = DrainagestrategyDtoToDrainagestrategy(null, drainageStrategyDto);

        if (drainageStrategyDto.ProductionProfileOil != null)
        {
            drainageStrategy.ProductionProfileOil = Convert(drainageStrategyDto.ProductionProfileOil, drainageStrategy,
                unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileGas != null)
        {
            drainageStrategy.ProductionProfileGas = Convert(drainageStrategyDto.ProductionProfileGas, drainageStrategy,
                unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileWater != null)
        {
            drainageStrategy.ProductionProfileWater = Convert(drainageStrategyDto.ProductionProfileWater,
                drainageStrategy, unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileWaterInjection != null)
        {
            drainageStrategy.ProductionProfileWaterInjection =
                Convert(drainageStrategyDto.ProductionProfileWaterInjection, drainageStrategy, unit, initialCreate);
        }

        if (drainageStrategyDto.FuelFlaringAndLosses != null)
        {
            drainageStrategy.FuelFlaringAndLosses = Convert(drainageStrategyDto.FuelFlaringAndLosses, drainageStrategy,
                unit, initialCreate);
        }

        if (drainageStrategyDto.NetSalesGas != null)
        {
            drainageStrategy.NetSalesGas =
                Convert(drainageStrategyDto.NetSalesGas, drainageStrategy, unit, initialCreate);
        }

        if (drainageStrategyDto.Co2Emissions != null)
        {
            drainageStrategy.Co2Emissions =
                Convert(drainageStrategyDto.Co2Emissions, drainageStrategy, unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileNGL != null)
        {
            drainageStrategy.ProductionProfileNGL = Convert(drainageStrategyDto.ProductionProfileNGL, drainageStrategy,
                unit, initialCreate);
        }

        if (drainageStrategyDto.ImportedElectricity != null)
        {
            drainageStrategy.ImportedElectricity = Convert(drainageStrategyDto.ImportedElectricity, drainageStrategy,
                unit, initialCreate);
        }

        if (drainageStrategyDto.ImportedElectricityOverride != null)
        {
            drainageStrategy.ImportedElectricityOverride = Convert(drainageStrategyDto.ImportedElectricityOverride, drainageStrategy,
                unit, initialCreate);
        }

        return drainageStrategy;
    }

    public static DrainageStrategy ConvertExisting(DrainageStrategy existing, DrainageStrategyDto drainageStrategyDto,
        PhysUnit unit, bool initialCreate)
    {
        DrainagestrategyDtoToDrainagestrategy(existing, drainageStrategyDto);

        if (drainageStrategyDto.ProductionProfileOil != null
            && (existing.ProductionProfileOil == null ||
                !existing.ProductionProfileOil.Values.SequenceEqual(drainageStrategyDto.ProductionProfileOil.Values)))
        {
            existing.ProductionProfileOil =
                Convert(drainageStrategyDto.ProductionProfileOil, existing, unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileGas != null
            && (existing.ProductionProfileGas == null ||
                !existing.ProductionProfileGas.Values.SequenceEqual(drainageStrategyDto.ProductionProfileGas.Values)))
        {
            existing.ProductionProfileGas =
                Convert(drainageStrategyDto.ProductionProfileGas, existing, unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileWater != null
            && (existing.ProductionProfileWater == null ||
                !existing.ProductionProfileWater.Values.SequenceEqual(drainageStrategyDto.ProductionProfileWater
                    .Values)))
        {
            existing.ProductionProfileWater =
                Convert(drainageStrategyDto.ProductionProfileWater, existing, unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileWaterInjection != null
            && (existing.ProductionProfileWaterInjection == null ||
                !existing.ProductionProfileWaterInjection.Values.SequenceEqual(drainageStrategyDto
                    .ProductionProfileWaterInjection.Values)))
        {
            existing.ProductionProfileWaterInjection = Convert(drainageStrategyDto.ProductionProfileWaterInjection,
                existing, unit, initialCreate);
        }

        if (drainageStrategyDto.FuelFlaringAndLosses != null
            && (existing.FuelFlaringAndLosses == null ||
                !existing.FuelFlaringAndLosses.Values.SequenceEqual(drainageStrategyDto.FuelFlaringAndLosses.Values)))
        {
            existing.FuelFlaringAndLosses =
                Convert(drainageStrategyDto.FuelFlaringAndLosses, existing, unit, initialCreate);
        }

        if (drainageStrategyDto.NetSalesGas != null
            && (existing.NetSalesGas == null ||
                !existing.NetSalesGas.Values.SequenceEqual(drainageStrategyDto.NetSalesGas.Values)))
        {
            existing.NetSalesGas = Convert(drainageStrategyDto.NetSalesGas, existing, unit, initialCreate);
        }

        if (drainageStrategyDto.Co2Emissions != null
            && (existing.Co2Emissions == null ||
                !existing.Co2Emissions.Values.SequenceEqual(drainageStrategyDto.Co2Emissions.Values)))
        {
            existing.Co2Emissions = Convert(drainageStrategyDto.Co2Emissions, existing, unit, initialCreate);
        }

        if ((drainageStrategyDto.Co2EmissionsOverride != null
            && (existing.Co2EmissionsOverride?.Values
            .SequenceEqual(drainageStrategyDto.Co2EmissionsOverride.Values) != true)) ||
            existing.Co2EmissionsOverride?.Override != drainageStrategyDto?.Co2EmissionsOverride?.Override)
        {
            existing.Co2EmissionsOverride =
                Convert<Co2EmissionsOverrideDto, Co2EmissionsOverride>(drainageStrategyDto.Co2EmissionsOverride, existing, existing.Co2EmissionsOverride, unit, initialCreate);
                existing.Co2EmissionsOverride.DrainageStrategy = existing;
        }

        if (drainageStrategyDto.ProductionProfileNGL != null
            && (existing.ProductionProfileNGL == null ||
                !existing.ProductionProfileNGL.Values.SequenceEqual(drainageStrategyDto.ProductionProfileNGL.Values)))
        {
            existing.ProductionProfileNGL =
                Convert(drainageStrategyDto.ProductionProfileNGL, existing, unit, initialCreate);
        }

        if (drainageStrategyDto.ImportedElectricity != null
            && (existing.ImportedElectricity == null ||
                !existing.ImportedElectricity.Values.SequenceEqual(drainageStrategyDto.ImportedElectricity.Values)))
        {
            existing.ImportedElectricity =
                Convert(drainageStrategyDto.ImportedElectricity, existing, unit, initialCreate);
        }

        // TODO: Check if this is necessary or can be improved
        if ((drainageStrategyDto.ImportedElectricityOverride != null
            && (existing.ImportedElectricityOverride?.Values
            .SequenceEqual(drainageStrategyDto.ImportedElectricityOverride.Values) != true)) ||
            existing.ImportedElectricityOverride?.Override != drainageStrategyDto?.ImportedElectricityOverride?.Override)
        {
            existing.ImportedElectricityOverride =
                Convert(drainageStrategyDto.ImportedElectricityOverride, existing, unit, initialCreate);
        }

        return existing;
    }

        private static TModel Convert<TDto, TModel>(TDto overrideDto,
        DrainageStrategy drainageStrategy, TimeSeries<double> existingProfile,
         PhysUnit unit, bool initialCreate)
        where TDto : TimeSeriesDto<double>, ITimeSeriesOverrideDto
        where TModel : TimeSeries<double>, ITimeSeriesOverride, new()
    {
        var needToConvertValues = existingProfile?.Values == null;
        if (overrideDto != null && existingProfile?.Values != null &&
            !existingProfile.Values.SequenceEqual(overrideDto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = overrideDto == null || drainageStrategy == null
            ? null
            : new TModel
            {
                Id = overrideDto.Id,
                // DrainageStrategy = drainageStrategy,
                Override = overrideDto.Override,
                StartYear = overrideDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(overrideDto.Values, unit, nameof(Co2Emissions))
                    : overrideDto.Values,
            };
        return convertedTimeSeries;
    }

    private static DrainageStrategy DrainagestrategyDtoToDrainagestrategy(DrainageStrategy? existing,
        DrainageStrategyDto drainageStrategyDto)
    {
        if (existing == null)
        {
            return new DrainageStrategy
            {
                Id = drainageStrategyDto.Id,
                Name = drainageStrategyDto.Name,
                Description = drainageStrategyDto.Description,
                ProjectId = drainageStrategyDto.ProjectId,
                NGLYield = drainageStrategyDto.NGLYield,
                ArtificialLift = drainageStrategyDto.ArtificialLift,
                GasSolution = drainageStrategyDto.GasSolution,
                ProducerCount = drainageStrategyDto.ProducerCount,
                GasInjectorCount = drainageStrategyDto.GasInjectorCount,
                WaterInjectorCount = drainageStrategyDto.WaterInjectorCount,
            };
        }

        existing.Id = drainageStrategyDto.Id;
        existing.Name = drainageStrategyDto.Name;
        existing.Description = drainageStrategyDto.Description;
        existing.ProjectId = drainageStrategyDto.ProjectId;
        existing.NGLYield = drainageStrategyDto.NGLYield;
        existing.ArtificialLift = drainageStrategyDto.ArtificialLift;
        existing.GasSolution = drainageStrategyDto.GasSolution;
        existing.ProducerCount = drainageStrategyDto.ProducerCount;
        existing.GasInjectorCount = drainageStrategyDto.GasInjectorCount;
        existing.WaterInjectorCount = drainageStrategyDto.WaterInjectorCount;

        return existing;
    }

    private static double[] ConvertUnitValues(double[] values, PhysUnit unit, string type)
    {
        string[] MTPA_Units = { nameof(Co2Emissions), nameof(ProductionProfileNGL) };
        string[] BBL_Units =
            { nameof(ProductionProfileOil), nameof(ProductionProfileWater), nameof(ProductionProfileWaterInjection) };
        string[] SCF_Units = { nameof(ProductionProfileGas), nameof(FuelFlaringAndLosses), nameof(NetSalesGas) };

        // Per now - the timeseriestypes which use millions are the same in both SI and Oilfield
        if (SCF_Units.Contains(type))
        {
            // These types should be saved in billions
            values = Array.ConvertAll(values, x => x * 1E9);
        }
        else if (MTPA_Units.Contains(type) || BBL_Units.Contains(type))
        {
            // These types should be saved in millions
            values = Array.ConvertAll(values, x => x * 1E6);
        }

        // If values were inserted in Oilfield, convert to baseunit
        if (unit == PhysUnit.OilField && !MTPA_Units.Contains(type))
        {
            if (BBL_Units.Contains(type))
            {
                // Unit: From BBL to baseunit Sm3
                values = Array.ConvertAll(values, x => x / 6.290);
            }
            else if (SCF_Units.Contains(type))
            {
                // Unit: From SCF to baseunit Sm3
                values = Array.ConvertAll(values, x => x / 35.315);
            }
        }

        return values;
    }

    private static ProductionProfileOil? Convert(ProductionProfileOilDto? productionProfileOilDto,
        DrainageStrategy drainageStrategy, PhysUnit unit, bool initialCreate)
    {
        var needToConvertValues = drainageStrategy?.ProductionProfileOil?.Values == null;
        if (productionProfileOilDto != null && drainageStrategy?.ProductionProfileOil?.Values != null &&
            !drainageStrategy.ProductionProfileOil.Values.SequenceEqual(productionProfileOilDto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = productionProfileOilDto == null || drainageStrategy == null
            ? null
            : new ProductionProfileOil
            {
                Id = productionProfileOilDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileOilDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(productionProfileOilDto.Values, unit, nameof(ProductionProfileOil))
                    : productionProfileOilDto.Values,
            };
        return convertedTimeSeries;
    }

    private static ProductionProfileGas? Convert(ProductionProfileGasDto? productionProfileGasDto,
        DrainageStrategy drainageStrategy, PhysUnit unit, bool initialCreate)
    {
        var needToConvertValues = drainageStrategy?.ProductionProfileGas?.Values == null;
        if (productionProfileGasDto != null && drainageStrategy?.ProductionProfileGas?.Values != null &&
            !drainageStrategy.ProductionProfileGas.Values.SequenceEqual(productionProfileGasDto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = productionProfileGasDto == null || drainageStrategy == null
            ? null
            : new ProductionProfileGas
            {
                Id = productionProfileGasDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileGasDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(productionProfileGasDto.Values, unit, nameof(ProductionProfileGas))
                    : productionProfileGasDto.Values,
            };
        return convertedTimeSeries;
    }

    private static ProductionProfileWater? Convert(ProductionProfileWaterDto? productionProfileWaterDto,
        DrainageStrategy drainageStrategy, PhysUnit unit, bool initialCreate)
    {
        var needToConvertValues = drainageStrategy?.ProductionProfileWater?.Values == null;
        if (productionProfileWaterDto != null && drainageStrategy?.ProductionProfileWater?.Values != null &&
            !drainageStrategy.ProductionProfileWater.Values.SequenceEqual(productionProfileWaterDto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = productionProfileWaterDto == null || drainageStrategy == null
            ? null
            : new ProductionProfileWater
            {
                Id = productionProfileWaterDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileWaterDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(productionProfileWaterDto.Values, unit, nameof(ProductionProfileWater))
                    : productionProfileWaterDto.Values,
            };
        return convertedTimeSeries;
    }

    private static ProductionProfileWaterInjection? Convert(
        ProductionProfileWaterInjectionDto? productionProfileWaterInjectionDto, DrainageStrategy drainageStrategy,
        PhysUnit unit, bool initialCreate)
    {
        var needToConvertValues = drainageStrategy?.ProductionProfileWaterInjection?.Values == null;
        if (productionProfileWaterInjectionDto != null &&
            drainageStrategy?.ProductionProfileWaterInjection?.Values != null &&
            !drainageStrategy.ProductionProfileWaterInjection.Values.SequenceEqual(productionProfileWaterInjectionDto
                .Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = productionProfileWaterInjectionDto == null || drainageStrategy == null
            ? null
            : new ProductionProfileWaterInjection
            {
                Id = productionProfileWaterInjectionDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileWaterInjectionDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(productionProfileWaterInjectionDto.Values, unit,
                        nameof(ProductionProfileWaterInjection))
                    : productionProfileWaterInjectionDto.Values,
            };
        return convertedTimeSeries;
    }

    private static FuelFlaringAndLosses? Convert(FuelFlaringAndLossesDto? fuelFlaringAndLossesDto,
        DrainageStrategy drainageStrategy, PhysUnit unit, bool initialCreate)
    {
        var needToConvertValues = drainageStrategy?.FuelFlaringAndLosses?.Values == null;
        if (fuelFlaringAndLossesDto != null && drainageStrategy?.FuelFlaringAndLosses?.Values != null &&
            !drainageStrategy.FuelFlaringAndLosses.Values.SequenceEqual(fuelFlaringAndLossesDto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = fuelFlaringAndLossesDto == null || drainageStrategy == null
            ? null
            : new FuelFlaringAndLosses
            {
                Id = fuelFlaringAndLossesDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = fuelFlaringAndLossesDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(fuelFlaringAndLossesDto.Values, unit, nameof(FuelFlaringAndLosses))
                    : fuelFlaringAndLossesDto.Values,
            };
        return convertedTimeSeries;
    }

    private static NetSalesGas? Convert(NetSalesGasDto? netSalesGasDto, DrainageStrategy drainageStrategy,
        PhysUnit unit, bool initialCreate)
    {
        var needToConvertValues = drainageStrategy?.NetSalesGas?.Values == null;
        if (netSalesGasDto != null && drainageStrategy?.NetSalesGas?.Values != null &&
            !drainageStrategy.NetSalesGas.Values.SequenceEqual(netSalesGasDto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = netSalesGasDto == null || drainageStrategy == null
            ? null
            : new NetSalesGas
            {
                Id = netSalesGasDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = netSalesGasDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(netSalesGasDto.Values, unit, nameof(NetSalesGas))
                    : netSalesGasDto.Values,
            };
        return convertedTimeSeries;
    }

    private static Co2Emissions? Convert(Co2EmissionsDto? co2EmissionsDto, DrainageStrategy drainageStrategy,
        PhysUnit unit, bool initialCreate)
    {
        var needToConvertValues = drainageStrategy?.Co2Emissions?.Values == null;
        if (co2EmissionsDto != null && drainageStrategy?.Co2Emissions?.Values != null &&
            !drainageStrategy.Co2Emissions.Values.SequenceEqual(co2EmissionsDto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = co2EmissionsDto == null || drainageStrategy == null
            ? null
            : new Co2Emissions
            {
                Id = co2EmissionsDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = co2EmissionsDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(co2EmissionsDto.Values, unit, nameof(Co2Emissions))
                    : co2EmissionsDto.Values,
            };
        return convertedTimeSeries;
    }

    private static Co2EmissionsOverride? Convert(Co2EmissionsOverrideDto? co2EmissionsOverrideDto,
        DrainageStrategy drainageStrategy, PhysUnit unit, bool initialCreate)
    {
        var needToConvertValues = drainageStrategy?.Co2EmissionsOverride?.Values == null;
        if (co2EmissionsOverrideDto != null && drainageStrategy?.Co2EmissionsOverride?.Values != null &&
            !drainageStrategy.Co2EmissionsOverride.Values.SequenceEqual(co2EmissionsOverrideDto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = co2EmissionsOverrideDto == null || drainageStrategy == null
            ? null
            : new Co2EmissionsOverride
            {
                Id = co2EmissionsOverrideDto.Id,
                DrainageStrategy = drainageStrategy,
                Override = co2EmissionsOverrideDto.Override,
                StartYear = co2EmissionsOverrideDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(co2EmissionsOverrideDto.Values, unit, nameof(Co2Emissions))
                    : co2EmissionsOverrideDto.Values,
            };
        return convertedTimeSeries;
    }

    private static ProductionProfileNGL? Convert(ProductionProfileNGLDto? productionProfileNGLDto,
        DrainageStrategy drainageStrategy, PhysUnit unit, bool initialCreate)
    {
        var needToConvertValues = drainageStrategy?.ProductionProfileNGL?.Values == null;
        if (productionProfileNGLDto != null && drainageStrategy?.ProductionProfileNGL?.Values != null &&
            !drainageStrategy.ProductionProfileNGL.Values.SequenceEqual(productionProfileNGLDto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = productionProfileNGLDto == null || drainageStrategy == null
            ? null
            : new ProductionProfileNGL
            {
                Id = productionProfileNGLDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileNGLDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(productionProfileNGLDto.Values, unit, nameof(ProductionProfileNGL))
                    : productionProfileNGLDto.Values,
            };
        return convertedTimeSeries;
    }

    private static ImportedElectricity? Convert(ImportedElectricityDto? importedElectricityDto,
        DrainageStrategy drainageStrategy, PhysUnit unit, bool initialCreate)
    {
        var needToConvertValues = drainageStrategy?.ImportedElectricity?.Values == null;
        if (importedElectricityDto != null && drainageStrategy?.ImportedElectricity?.Values != null &&
            !drainageStrategy.ImportedElectricity.Values.SequenceEqual(importedElectricityDto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = importedElectricityDto == null || drainageStrategy == null
            ? null
            : new ImportedElectricity
            {
                Id = importedElectricityDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = importedElectricityDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(importedElectricityDto.Values, unit, nameof(ImportedElectricity))
                    : importedElectricityDto.Values,
            };
        return convertedTimeSeries;
    }

    private static ImportedElectricityOverride? Convert(ImportedElectricityOverrideDto? importedElectricityDto,
    DrainageStrategy drainageStrategy, PhysUnit unit, bool initialCreate)
    {
        var needToConvertValues = drainageStrategy?.ImportedElectricityOverride?.Values == null;
        if (importedElectricityDto != null && drainageStrategy?.ImportedElectricityOverride?.Values != null &&
            !drainageStrategy.ImportedElectricityOverride.Values.SequenceEqual(importedElectricityDto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = importedElectricityDto == null || drainageStrategy == null
            ? null
            : new ImportedElectricityOverride
            {
                Id = importedElectricityDto.Id,
                DrainageStrategy = drainageStrategy,
                Override = importedElectricityDto.Override,
                StartYear = importedElectricityDto.StartYear,
                Values = needToConvertValues || initialCreate
                    ? ConvertUnitValues(importedElectricityDto.Values, unit, nameof(ImportedElectricity))
                    : importedElectricityDto.Values,
            };
        return convertedTimeSeries;
    }
}
