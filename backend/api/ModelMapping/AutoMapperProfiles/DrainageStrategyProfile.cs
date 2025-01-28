using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

using static System.Enum;

namespace api.ModelMapping.AutoMapperProfiles;

public class DrainageStrategyProfile : Profile
{
    public DrainageStrategyProfile()
    {
        CreateMap<DrainageStrategy, DrainageStrategyDto>();
        CreateMap<TimeSeriesProfile, TimeSeriesCostDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesToDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.ProductionProfileOil)));
        CreateMap<AdditionalProductionProfileOil, TimeSeriesCostDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(AdditionalProductionProfileOil)
                    )
                    ));
        CreateMap<ProductionProfileGas, TimeSeriesCostDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileGas)
                    )
                    ));
        CreateMap<AdditionalProductionProfileGas, TimeSeriesCostDto>()
                    .ForMember(
                        dest => dest.Values,
                        opt => opt.MapFrom((src, dest, destMember, context) =>
                            ConvertValuesToDTO(src.Values,
                            (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                            nameof(AdditionalProductionProfileGas)
                            )
                            ));
        CreateMap<ProductionProfileWater, TimeSeriesCostDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWater)
                    )
                    ));
        CreateMap<ProductionProfileWaterInjection, TimeSeriesCostDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWaterInjection)
                    )
                    ));
        CreateMap<TimeSeriesProfile, TimeSeriesCostDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesToDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.FuelFlaringAndLosses)));
        CreateMap<TimeSeriesProfile, TimeSeriesCostOverrideDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesToDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.FuelFlaringAndLossesOverride)));
        CreateMap<TimeSeriesProfile, TimeSeriesCostDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesToDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.NetSalesGas)));
        CreateMap<TimeSeriesProfile, TimeSeriesCostOverrideDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesToDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.NetSalesGasOverride)));
        CreateMap<TimeSeriesProfile, TimeSeriesCostDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesToDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.Co2Emissions)));
        CreateMap<TimeSeriesProfile, TimeSeriesCostOverrideDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesToDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.Co2EmissionsOverride)));
        CreateMap<Co2Intensity, TimeSeriesCostDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(Co2Intensity)
                    )
                    ));
        CreateMap<DeferredOilProduction, TimeSeriesCostDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredOilProduction)
                    )
                    ));
        CreateMap<DeferredGasProduction, TimeSeriesCostDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredGasProduction)
                    )
                    ));

        CreateMap<CreateTimeSeriesCostDto, TimeSeriesProfile>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesFromDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.ImportedElectricityOverride)));
        CreateMap<CreateTimeSeriesCostOverrideDto, TimeSeriesProfile>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesFromDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.FuelFlaringAndLossesOverride)));
        CreateMap<CreateTimeSeriesCostOverrideDto, TimeSeriesProfile>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesFromDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.NetSalesGasOverride)));
        CreateMap<CreateTimeSeriesCostOverrideDto, TimeSeriesProfile>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesFromDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.Co2EmissionsOverride)));
        CreateMap<CreateTimeSeriesCostDto, TimeSeriesProfile>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesFromDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.ProductionProfileOil)));
        CreateMap<CreateTimeSeriesCostDto, AdditionalProductionProfileOil>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(AdditionalProductionProfileOil)
                    )
                    ));
        CreateMap<CreateTimeSeriesCostDto, ProductionProfileGas>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileGas)
                    )
                    ));
        CreateMap<CreateTimeSeriesCostDto, AdditionalProductionProfileGas>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(AdditionalProductionProfileGas)
                    )
                    ));
        CreateMap<CreateTimeSeriesCostDto, DeferredOilProduction>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredOilProduction)
                    )
                    ));
        CreateMap<CreateTimeSeriesCostDto, DeferredGasProduction>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredGasProduction)
                    )
                    ));
        CreateMap<CreateTimeSeriesCostDto, ProductionProfileWater>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWater)
                    )
                    ));
        CreateMap<CreateTimeSeriesCostDto, ProductionProfileWaterInjection>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWaterInjection)
                    )
                    ));

        CreateMap<UpdateTimeSeriesCostOverrideDto, TimeSeriesProfile>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesFromDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.ImportedElectricityOverride)));
        CreateMap<UpdateTimeSeriesCostOverrideDto, TimeSeriesProfile>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesFromDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.FuelFlaringAndLossesOverride)));
        CreateMap<UpdateTimeSeriesCostOverrideDto, TimeSeriesProfile>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesFromDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.NetSalesGasOverride)));
        CreateMap<UpdateTimeSeriesCostOverrideDto, TimeSeriesProfile>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesFromDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.ImportedElectricityOverride)));
        CreateMap<UpdateTimeSeriesCostOverrideDto, TimeSeriesProfile>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesFromDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.Co2EmissionsOverride)));
        CreateMap<UpdateTimeSeriesCostDto, TimeSeriesProfile>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, _, _, context) =>
                    ConvertValuesFromDTO(src.Values,
                        Parse<PhysUnit>(context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                        ProfileTypes.ProductionProfileOil)));
        CreateMap<UpdateTimeSeriesCostDto, AdditionalProductionProfileOil>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(AdditionalProductionProfileOil)
                    )
                    ));
        CreateMap<UpdateTimeSeriesCostDto, AdditionalProductionProfileGas>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(AdditionalProductionProfileGas)
                    )
                    ));
        CreateMap<UpdateTimeSeriesCostDto, ProductionProfileGas>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileGas)
                    )
                    ));
        CreateMap<UpdateTimeSeriesCostDto, DeferredOilProduction>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredOilProduction)
                    )
                    ));
        CreateMap<UpdateTimeSeriesCostDto, DeferredGasProduction>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredGasProduction)
                    )
                    ));
        CreateMap<UpdateTimeSeriesCostDto, ProductionProfileWater>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWater)
                    )
                    ));
        CreateMap<UpdateTimeSeriesCostDto, ProductionProfileWaterInjection>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWaterInjection)
                    )
                    ));
    }

    private static readonly Dictionary<string, double> ConversionFactors = new()
    {
        { ProfileTypes.Co2Emissions, 1_000_000 },
        { ProfileTypes.Co2EmissionsOverride, 1_000_000 },
        { nameof(ProductionProfileNgl), 1_000_000 },
        { ProfileTypes.ProductionProfileOil, 1_000_000 },
        { nameof(AdditionalProductionProfileOil), 1_000_000 },
        { nameof(ProductionProfileWater), 1_000_000 },
        { nameof(ProductionProfileWaterInjection), 1_000_000 },
        { nameof(ProductionProfileGas), 1_000_000_000 },
        { nameof(AdditionalProductionProfileGas), 1_000_000_000 },
        { ProfileTypes.FuelFlaringAndLosses, 1_000_000_000 },
        { ProfileTypes.FuelFlaringAndLossesOverride, 1_000_000_000 },
        { ProfileTypes.NetSalesGas, 1_000_000_000 },
        { ProfileTypes.NetSalesGasOverride, 1_000_000_000 }
    };

    private static double[] ConvertValuesToDTO(double[] values, PhysUnit unit, string type)
    {
        double conversionFactor = GetConversionFactor(type, unit, toDto: true);
        return Array.ConvertAll(values, x => x * conversionFactor);
    }

    private static double[] ConvertValuesFromDTO(double[]? values, PhysUnit unit, string type)
    {
        if (values == null) { return Array.Empty<double>(); }
        double conversionFactor = GetConversionFactor(type, unit, toDto: false);
        return Array.ConvertAll(values, x => x * conversionFactor);
    }

    private static double GetConversionFactor(string type, PhysUnit unit, bool toDto)
    {
        var returnValue = 1.0;
        if (ConversionFactors.TryGetValue(type, out double conversionFactor))
        {
            returnValue = toDto ? 1.0 / conversionFactor : conversionFactor;
        }
        if (unit == PhysUnit.OilField)
        {
            switch (type)
            {
                case ProfileTypes.ProductionProfileOil:
                case nameof(AdditionalProductionProfileOil):
                case nameof(ProductionProfileWater):
                case nameof(ProductionProfileWaterInjection):
                    return toDto ? 6.290 * returnValue : 1.0 / 6.290 * returnValue;
                case nameof(ProductionProfileGas):
                case nameof(AdditionalProductionProfileGas):
                case ProfileTypes.FuelFlaringAndLosses:
                case ProfileTypes.FuelFlaringAndLossesOverride:
                case nameof(DeferredOilProduction):
                case nameof(DeferredGasProduction):
                case ProfileTypes.NetSalesGas:
                case ProfileTypes.NetSalesGasOverride:
                    return toDto ? 35.315 * returnValue : 1.0 / 35.315 * returnValue;
            }
        }

        return returnValue;
    }
}
