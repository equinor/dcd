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
            drainageStrategy.ProductionProfileOil = Convert<ProductionProfileOilDto, ProductionProfileOil>(drainageStrategyDto.ProductionProfileOil, drainageStrategy, null,
                unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileGas != null)
        {
            drainageStrategy.ProductionProfileGas = Convert<ProductionProfileGasDto, ProductionProfileGas>(drainageStrategyDto.ProductionProfileGas, drainageStrategy, null,
                unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileWater != null)
        {
            drainageStrategy.ProductionProfileWater = Convert<ProductionProfileWaterDto, ProductionProfileWater>(drainageStrategyDto.ProductionProfileWater,
                drainageStrategy, null, unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileWaterInjection != null)
        {
            drainageStrategy.ProductionProfileWaterInjection =
                Convert<ProductionProfileWaterInjectionDto, ProductionProfileWaterInjection>(drainageStrategyDto.ProductionProfileWaterInjection, drainageStrategy, null, unit, initialCreate);
        }

        if (drainageStrategyDto.FuelFlaringAndLosses != null)
        {
            drainageStrategy.FuelFlaringAndLosses = Convert<FuelFlaringAndLossesDto, FuelFlaringAndLosses>(drainageStrategyDto.FuelFlaringAndLosses, drainageStrategy, null,
                unit, initialCreate);
        }

        if (drainageStrategyDto.NetSalesGas != null)
        {
            drainageStrategy.NetSalesGas =
                Convert<NetSalesGasDto, NetSalesGas>(drainageStrategyDto.NetSalesGas, drainageStrategy, null, unit, initialCreate);
        }

        if (drainageStrategyDto.Co2Emissions != null)
        {
            drainageStrategy.Co2Emissions =
                Convert<Co2EmissionsDto, Co2Emissions>(drainageStrategyDto.Co2Emissions, drainageStrategy, null, unit, initialCreate);
        }

        if (drainageStrategyDto.Co2EmissionsOverride != null)
        {
            drainageStrategy.Co2EmissionsOverride =
                ConvertOverride<Co2EmissionsOverrideDto, Co2EmissionsOverride>(drainageStrategyDto.Co2EmissionsOverride, drainageStrategy, null, unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileNGL != null)
        {
            drainageStrategy.ProductionProfileNGL = Convert<ProductionProfileNGLDto, ProductionProfileNGL>(drainageStrategyDto.ProductionProfileNGL, drainageStrategy, null,
                unit, initialCreate);
        }

        if (drainageStrategyDto.ImportedElectricity != null)
        {
            drainageStrategy.ImportedElectricity = Convert<ImportedElectricityDto, ImportedElectricity>(drainageStrategyDto.ImportedElectricity, drainageStrategy, null,
                unit, initialCreate);
        }

        if (drainageStrategyDto.ImportedElectricityOverride != null)
        {
            drainageStrategy.ImportedElectricityOverride = ConvertOverride<ImportedElectricityOverrideDto, ImportedElectricityOverride>(drainageStrategyDto.ImportedElectricityOverride, drainageStrategy, null,
                unit, initialCreate);
        }

        return drainageStrategy;
    }

    public static DrainageStrategy ConvertExisting(DrainageStrategy existing, DrainageStrategyDto drainageStrategyDto,
        PhysUnit unit, bool initialCreate)
    {
        DrainagestrategyDtoToDrainagestrategy(existing, drainageStrategyDto);

        if (drainageStrategyDto.ProductionProfileOil != null
            && (existing.ProductionProfileOil?.Values.SequenceEqual(drainageStrategyDto.ProductionProfileOil.Values) != true))
        {
            existing.ProductionProfileOil =
                Convert<ProductionProfileOilDto, ProductionProfileOil>(drainageStrategyDto.ProductionProfileOil, existing,
                existing.ProductionProfileOil, unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileGas != null
            && (existing.ProductionProfileGas?.Values.SequenceEqual(drainageStrategyDto.ProductionProfileGas.Values) != true))
        {
            existing.ProductionProfileGas =
                Convert<ProductionProfileGasDto, ProductionProfileGas>(drainageStrategyDto.ProductionProfileGas, existing, existing.ProductionProfileGas, unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileWater != null
            && (existing.ProductionProfileWater?.Values.SequenceEqual(drainageStrategyDto.ProductionProfileWater
                    .Values) != true))
        {
            existing.ProductionProfileWater =
                Convert<ProductionProfileWaterDto, ProductionProfileWater>(drainageStrategyDto.ProductionProfileWater, existing, existing.ProductionProfileWater, unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileWaterInjection != null
            && (existing.ProductionProfileWaterInjection?.Values.SequenceEqual(drainageStrategyDto
                    .ProductionProfileWaterInjection.Values) != true))
        {
            existing.ProductionProfileWaterInjection = Convert<ProductionProfileWaterInjectionDto, ProductionProfileWaterInjection>(drainageStrategyDto.ProductionProfileWaterInjection,
                existing, existing.ProductionProfileWaterInjection, unit, initialCreate);
        }

        if (drainageStrategyDto.FuelFlaringAndLosses != null
            && (existing.FuelFlaringAndLosses?.Values.SequenceEqual(drainageStrategyDto.FuelFlaringAndLosses.Values) != true))
        {
            existing.FuelFlaringAndLosses =
                Convert<FuelFlaringAndLossesDto, FuelFlaringAndLosses>(drainageStrategyDto.FuelFlaringAndLosses, existing, existing.FuelFlaringAndLosses, unit, initialCreate);
        }

        if (drainageStrategyDto.NetSalesGas != null
            && (existing.NetSalesGas?.Values.SequenceEqual(drainageStrategyDto.NetSalesGas.Values) != true))
        {
            existing.NetSalesGas = Convert<NetSalesGasDto, NetSalesGas>(drainageStrategyDto.NetSalesGas, existing, existing.NetSalesGas, unit, initialCreate);
        }

        if (drainageStrategyDto.Co2Emissions != null
            && (existing.Co2Emissions?.Values.SequenceEqual(drainageStrategyDto.Co2Emissions.Values) != true))
        {
            existing.Co2Emissions = Convert<Co2EmissionsDto, Co2Emissions>(drainageStrategyDto.Co2Emissions, existing, existing.Co2Emissions, unit, initialCreate);
        }

        if ((drainageStrategyDto.Co2EmissionsOverride != null
            && (existing.Co2EmissionsOverride?.Values
            .SequenceEqual(drainageStrategyDto.Co2EmissionsOverride.Values) != true)) ||
            existing.Co2EmissionsOverride?.Override != drainageStrategyDto.Co2EmissionsOverride?.Override)
        {
            existing.Co2EmissionsOverride =
                ConvertOverride<Co2EmissionsOverrideDto, Co2EmissionsOverride>(drainageStrategyDto.Co2EmissionsOverride, existing, existing.Co2EmissionsOverride, unit, initialCreate);
        }

        if (drainageStrategyDto.ProductionProfileNGL != null
            && (existing.ProductionProfileNGL?.Values.SequenceEqual(drainageStrategyDto.ProductionProfileNGL.Values) != true))
        {
            existing.ProductionProfileNGL =
                Convert<ProductionProfileNGLDto, ProductionProfileNGL>(drainageStrategyDto.ProductionProfileNGL, existing, existing.ProductionProfileNGL, unit, initialCreate);
        }

        if (drainageStrategyDto.ImportedElectricity != null
            && (existing.ImportedElectricity?.Values.SequenceEqual(drainageStrategyDto.ImportedElectricity.Values) != true))
        {
            existing.ImportedElectricity =
                Convert<ImportedElectricityDto, ImportedElectricity>(drainageStrategyDto.ImportedElectricity, existing, existing.ImportedElectricity, unit, initialCreate);
        }

        if ((drainageStrategyDto.ImportedElectricityOverride != null
            && (existing.ImportedElectricityOverride?.Values
            .SequenceEqual(drainageStrategyDto.ImportedElectricityOverride.Values) != true)) ||
            existing.ImportedElectricityOverride?.Override != drainageStrategyDto.ImportedElectricityOverride?.Override)
        {
            existing.ImportedElectricityOverride =
                ConvertOverride<ImportedElectricityOverrideDto, ImportedElectricityOverride>(drainageStrategyDto.ImportedElectricityOverride, existing, existing.ImportedElectricityOverride, unit, initialCreate);
        }

        return existing;
    }

    private static TModel? ConvertOverride<TDto, TModel>(TDto? dto, DrainageStrategy drainageStrategy,
        TimeSeries<double>? existingProfile, PhysUnit unit, bool initialCreate)
            where TDto : TimeSeriesDto<double>, ITimeSeriesOverrideDto
            where TModel : TimeSeries<double>, ITimeSeriesOverride, IDrainageStrategyTimeSeries, new()
    {
        var needToConvertValues = existingProfile?.Values == null;
        if (dto != null && existingProfile?.Values != null &&
            !existingProfile.Values.SequenceEqual(dto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = dto == null || drainageStrategy == null
            ? null
            : new TModel
            {
                Id = dto.Id,
                Override = dto.Override,
                StartYear = dto.StartYear,
                DrainageStrategy = drainageStrategy,
            };

        if (convertedTimeSeries == null || dto == null) { return null; }

        convertedTimeSeries.Values = needToConvertValues || initialCreate
                ? ConvertUnitValues(dto.Values, unit, convertedTimeSeries.GetType().Name)
                : dto.Values;

        return convertedTimeSeries;
    }

    private static TModel? Convert<TDto, TModel>(TDto? dto, DrainageStrategy drainageStrategy,
        TimeSeries<double>? existingProfile, PhysUnit unit, bool initialCreate)
            where TDto : TimeSeriesDto<double>
            where TModel : TimeSeries<double>, IDrainageStrategyTimeSeries, new()
    {
        var needToConvertValues = existingProfile?.Values == null;
        if (dto != null && existingProfile?.Values != null &&
            !existingProfile.Values.SequenceEqual(dto.Values))
        {
            needToConvertValues = true;
        }

        var convertedTimeSeries = dto == null || drainageStrategy == null
            ? null
            : new TModel
            {
                Id = dto.Id,
                StartYear = dto.StartYear,
                DrainageStrategy = drainageStrategy,
            };

        if (convertedTimeSeries == null || dto == null) { return null; }

        convertedTimeSeries.Values = needToConvertValues || initialCreate
                ? ConvertUnitValues(dto.Values, unit, convertedTimeSeries.GetType().Name)
                : dto.Values;

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
        string[] MTPA_Units = { nameof(Co2Emissions), nameof(Co2EmissionsOverride), nameof(ProductionProfileNGL) };
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
}
