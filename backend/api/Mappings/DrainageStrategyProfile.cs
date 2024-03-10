using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class DrainageStrategyProfile : Profile
{
    public DrainageStrategyProfile()
    {
        CreateMap<DrainageStrategy, DrainageStrategyDto>();
        CreateMap<ProductionProfileOil, ProductionProfileOilDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValues(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileOil)
                    )
                    ));
        CreateMap<ProductionProfileGas, ProductionProfileGasDto>();
        CreateMap<ProductionProfileWater, ProductionProfileWaterDto>();
        CreateMap<ProductionProfileWaterInjection, ProductionProfileWaterInjectionDto>();
        CreateMap<FuelFlaringAndLosses, FuelFlaringAndLossesDto>();
        CreateMap<FuelFlaringAndLossesOverride, FuelFlaringAndLossesOverrideDto>();
        CreateMap<NetSalesGas, NetSalesGasDto>();
        CreateMap<NetSalesGasOverride, NetSalesGasOverrideDto>();
        CreateMap<Co2Emissions, Co2EmissionsDto>();
        CreateMap<Co2EmissionsOverride, Co2EmissionsOverrideDto>();
        CreateMap<ProductionProfileNGL, ProductionProfileNGLDto>();
        CreateMap<ImportedElectricity, ImportedElectricityDto>();
        CreateMap<ImportedElectricityOverride, ImportedElectricityOverrideDto>();
        CreateMap<Co2Intensity, Co2IntensityDto>();

        CreateMap<UpdateDrainageStrategyDto, DrainageStrategy>();
        CreateMap<UpdateFuelFlaringAndLossesOverrideDto, FuelFlaringAndLossesOverride>();
        CreateMap<UpdateNetSalesGasOverrideDto, NetSalesGasOverride>();
        CreateMap<UpdateCo2EmissionsOverrideDto, Co2EmissionsOverride>();
        CreateMap<UpdateProductionProfileOilDto, ProductionProfileOil>();
        CreateMap<UpdateProductionProfileGasDto, ProductionProfileGas>();
        CreateMap<UpdateProductionProfileWaterDto, ProductionProfileWater>();
        CreateMap<UpdateProductionProfileWaterInjectionDto, ProductionProfileWaterInjection>();

        CreateMap<CreateDrainageStrategyDto, DrainageStrategy>();
    }

    // private static double[] ConvertValues(double[] values, string unit)
    // {
    //     if (unit == "OilField")
    //     {
    //         // Unit: From BBL to baseunit Sm3
    //         return Array.ConvertAll(values, x => x / 6.290);
    //     }
    //     else
    //     {
    //         // Unit: From SCF to baseunit Sm3
    //         return Array.ConvertAll(values, x => x / 35.315);
    //     }
    // }

    // private static TModel? Convert<TDto, TModel>(TDto? dto, DrainageStrategy drainageStrategy,
    //     TimeSeries<double>? existingProfile, PhysUnit unit, bool initialCreate)
    //         where TDto : TimeSeriesDto<double>
    //         where TModel : TimeSeries<double>, IDrainageStrategyTimeSeries, new()
    // {
    //     var needToConvertValues = existingProfile?.Values == null;
    //     if (dto != null && existingProfile?.Values != null &&
    //         !existingProfile.Values.SequenceEqual(dto.Values))
    //     {
    //         needToConvertValues = true;
    //     }

    //     var convertedTimeSeries = dto == null || drainageStrategy == null
    //         ? new TModel()
    //         : new TModel
    //         {
    //             Id = dto.Id,
    //             StartYear = dto.StartYear,
    //             DrainageStrategy = drainageStrategy,
    //         };

    //     if (convertedTimeSeries == null || dto == null) { return null; }

    //     convertedTimeSeries.Values = needToConvertValues || initialCreate
    //             ? ConvertUnitValues(dto.Values, unit, convertedTimeSeries.GetType().Name)
    //             : dto.Values;

    //     return convertedTimeSeries;
    // }

    //     private static TModel? ConvertOverride<TDto, TModel>(TDto? dto, DrainageStrategy drainageStrategy,
    //     TimeSeries<double>? existingProfile, PhysUnit unit, bool initialCreate)
    //         where TDto : TimeSeriesDto<double>, ITimeSeriesOverrideDto
    //         where TModel : TimeSeries<double>, ITimeSeriesOverride, IDrainageStrategyTimeSeries, new()
    // {
    //     var needToConvertValues = existingProfile?.Values == null;
    //     if (dto != null && existingProfile?.Values != null &&
    //         !existingProfile.Values.SequenceEqual(dto.Values))
    //     {
    //         needToConvertValues = true;
    //     }

    //     var convertedTimeSeries = dto == null || drainageStrategy == null
    //         ? new TModel()
    //         : new TModel
    //         {
    //             Id = dto.Id,
    //             Override = dto.Override,
    //             StartYear = dto.StartYear,
    //             DrainageStrategy = drainageStrategy,
    //         };

    //     if (convertedTimeSeries == null || dto == null) { return null; }

    //     convertedTimeSeries.Values = needToConvertValues || initialCreate
    //             ? ConvertUnitValues(dto.Values, unit, convertedTimeSeries.GetType().Name)
    //             : dto.Values;

    //     return convertedTimeSeries;
    // }

    private static double[] ConvertValues(double[] values, PhysUnit unit, string type)
    {
        string[] MTPA_Units = [nameof(Co2Emissions), nameof(Co2EmissionsOverride), nameof(ProductionProfileNGL)];
        string[] BBL_Units = [nameof(ProductionProfileOil), nameof(ProductionProfileWater), nameof(ProductionProfileWaterInjection)];
        string[] SCF_Units = [nameof(ProductionProfileGas), nameof(FuelFlaringAndLosses), nameof(FuelFlaringAndLossesOverride), nameof(NetSalesGas), nameof(NetSalesGasOverride)];

        // Per now - the timeseriestypes which use millions are the same in both SI and Oilfield
        if (SCF_Units.Contains(type))
        {
            // These types should be saved in billions
            values = Array.ConvertAll(values, x => x * 1_000_000_000);
        }
        else if (MTPA_Units.Contains(type) || BBL_Units.Contains(type))
        {
            // These types should be saved in millions
            values = Array.ConvertAll(values, x => x * 1_000_000);
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
