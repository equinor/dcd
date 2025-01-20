using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class DrainageStrategyProfile : Profile
{
    public DrainageStrategyProfile()
    {
        CreateMap<DrainageStrategy, DrainageStrategyDto>();
        CreateMap<ProductionProfileOil, TimeSeriesVolumeDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileOil)
                    )
                    ));
        CreateMap<AdditionalProductionProfileOil, TimeSeriesVolumeDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(AdditionalProductionProfileOil)
                    )
                    ));
        CreateMap<ProductionProfileGas, TimeSeriesVolumeDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileGas)
                    )
                    ));
        CreateMap<AdditionalProductionProfileGas, TimeSeriesVolumeDto>()
                    .ForMember(
                        dest => dest.Values,
                        opt => opt.MapFrom((src, dest, destMember, context) =>
                            ConvertValuesToDTO(src.Values,
                            (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                            nameof(AdditionalProductionProfileGas)
                            )
                            ));
        CreateMap<ProductionProfileWater, TimeSeriesVolumeDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWater)
                    )
                    ));
        CreateMap<ProductionProfileWaterInjection, TimeSeriesVolumeDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWaterInjection)
                    )
                    ));
        CreateMap<FuelFlaringAndLosses, TimeSeriesVolumeDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(FuelFlaringAndLosses)
                    )
                    ));
        CreateMap<FuelFlaringAndLossesOverride, TimeSeriesVolumeOverrideDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(FuelFlaringAndLossesOverride)
                    )
                    ));
        CreateMap<NetSalesGas, TimeSeriesVolumeDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(NetSalesGas)
                    )
                    ));
        CreateMap<NetSalesGasOverride, TimeSeriesVolumeOverrideDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(NetSalesGasOverride)
                    )
                    ));
        CreateMap<Co2Emissions, TimeSeriesMassDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(Co2Emissions)
                    )
                    ));
        CreateMap<Co2EmissionsOverride, TimeSeriesMassOverrideDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(Co2EmissionsOverride)
                    )
                    ));
        CreateMap<Co2Intensity, TimeSeriesMassDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(Co2Intensity)
                    )
                    ));
        CreateMap<DeferredOilProduction, TimeSeriesVolumeDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredOilProduction)
                    )
                    ));
        CreateMap<DeferredGasProduction, TimeSeriesVolumeDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredGasProduction)
                    )
                    ));
        CreateMap<ProductionProfileNgl, TimeSeriesVolumeDto>();
        CreateMap<ImportedElectricity, TimeSeriesEnergyDto>();
        CreateMap<ImportedElectricityOverride, TimeSeriesEnergyOverrideDto>();
        CreateMap<Co2Intensity, TimeSeriesMassDto>();

        CreateMap<CreateTimeSeriesEnergyDto, ImportedElectricityOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ImportedElectricityOverride)
                    )
                    ));
        CreateMap<CreateTimeSeriesVolumeOverrideDto, FuelFlaringAndLossesOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(FuelFlaringAndLossesOverride)
                    )
                    ));
        CreateMap<CreateTimeSeriesVolumeOverrideDto, NetSalesGasOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(NetSalesGasOverride)
                    )
                    ));
        CreateMap<CreateTimeSeriesMassOverrideDto, Co2EmissionsOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(Co2EmissionsOverride)
                    )
                    ));
        CreateMap<CreateTimeSeriesVolumeDto, ProductionProfileOil>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileOil)
                    )
                    ));
        CreateMap<CreateTimeSeriesVolumeDto, AdditionalProductionProfileOil>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(AdditionalProductionProfileOil)
                    )
                    ));
        CreateMap<CreateTimeSeriesVolumeDto, ProductionProfileGas>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileGas)
                    )
                    ));
        CreateMap<CreateTimeSeriesVolumeDto, AdditionalProductionProfileGas>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(AdditionalProductionProfileGas)
                    )
                    ));
        CreateMap<CreateTimeSeriesVolumeDto, DeferredOilProduction>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredOilProduction)
                    )
                    ));
        CreateMap<CreateTimeSeriesVolumeDto, DeferredGasProduction>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredGasProduction)
                    )
                    ));
        CreateMap<CreateTimeSeriesVolumeDto, ProductionProfileWater>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWater)
                    )
                    ));
        CreateMap<CreateTimeSeriesVolumeDto, ProductionProfileWaterInjection>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWaterInjection)
                    )
                    ));

        CreateMap<UpdateTimeSeriesEnergyOverrideDto, ImportedElectricityOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ImportedElectricityOverride)
                    )
                    ));
        CreateMap<UpdateTimeSeriesVolumeOverrideDto, FuelFlaringAndLossesOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(FuelFlaringAndLossesOverride)
                    )
                    ));
        CreateMap<UpdateTimeSeriesVolumeOverrideDto, NetSalesGasOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(NetSalesGasOverride)
                    )
                    ));
        CreateMap<UpdateTimeSeriesEnergyOverrideDto, ImportedElectricityOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ImportedElectricityOverride)
                    )
                    ));
        CreateMap<UpdateTimeSeriesMassOverrideDto, Co2EmissionsOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(Co2EmissionsOverride)
                    )
                    ));
        CreateMap<UpdateTimeSeriesVolumeDto, ProductionProfileOil>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileOil)
                    )
                    ));
        CreateMap<UpdateTimeSeriesVolumeDto, AdditionalProductionProfileOil>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(AdditionalProductionProfileOil)
                    )
                    ));
        CreateMap<UpdateTimeSeriesVolumeDto, AdditionalProductionProfileGas>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(AdditionalProductionProfileGas)
                    )
                    ));
        CreateMap<UpdateTimeSeriesVolumeDto, ProductionProfileGas>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileGas)
                    )
                    ));
        CreateMap<UpdateTimeSeriesVolumeDto, DeferredOilProduction>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredOilProduction)
                    )
                    ));
        CreateMap<UpdateTimeSeriesVolumeDto, DeferredGasProduction>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredGasProduction)
                    )
                    ));
        CreateMap<UpdateTimeSeriesVolumeDto, ProductionProfileWater>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWater)
                    )
                    ));
        CreateMap<UpdateTimeSeriesVolumeDto, ProductionProfileWaterInjection>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWaterInjection)
                    )
                    ));
    }

    private static readonly Dictionary<string, double> ConversionFactors = new()
    {
        { nameof(Co2Emissions), 1_000_000 },
        { nameof(Co2EmissionsOverride), 1_000_000 },
        { nameof(ProductionProfileNgl), 1_000_000 },
        { nameof(ProductionProfileOil), 1_000_000 },
        { nameof(AdditionalProductionProfileOil), 1_000_000 },
        { nameof(ProductionProfileWater), 1_000_000 },
        { nameof(ProductionProfileWaterInjection), 1_000_000 },
        { nameof(ProductionProfileGas), 1_000_000_000 },
        { nameof(AdditionalProductionProfileGas), 1_000_000_000 },
        { nameof(FuelFlaringAndLosses), 1_000_000_000 },
        { nameof(FuelFlaringAndLossesOverride), 1_000_000_000 },
        { nameof(NetSalesGas), 1_000_000_000 },
        { nameof(NetSalesGasOverride), 1_000_000_000 }
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
                case nameof(ProductionProfileOil):
                case nameof(AdditionalProductionProfileOil):
                case nameof(ProductionProfileWater):
                case nameof(ProductionProfileWaterInjection):
                    return toDto ? 6.290 * returnValue : 1.0 / 6.290 * returnValue;
                case nameof(ProductionProfileGas):
                case nameof(AdditionalProductionProfileGas):
                case nameof(FuelFlaringAndLosses):
                case nameof(FuelFlaringAndLossesOverride):
                case nameof(DeferredOilProduction):
                case nameof(DeferredGasProduction):
                case nameof(NetSalesGas):
                case nameof(NetSalesGasOverride):
                    return toDto ? 35.315 * returnValue : 1.0 / 35.315 * returnValue;
            }
        }

        return returnValue;
    }
}
