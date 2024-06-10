using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class DrainageStrategyProfile : Profile
{
    public DrainageStrategyProfile()
    {
        CreateMap<DrainageStrategy, DrainageStrategyWithProfilesDto>();
        CreateMap<DrainageStrategy, DrainageStrategyDto>();
        CreateMap<ProductionProfileOil, ProductionProfileOilDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileOil)
                    )
                    ));
        CreateMap<ProductionProfileGas, ProductionProfileGasDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileGas)
                    )
                    ));
        CreateMap<ProductionProfileWater, ProductionProfileWaterDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWater)
                    )
                    ));
        CreateMap<ProductionProfileWaterInjection, ProductionProfileWaterInjectionDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWaterInjection)
                    )
                    ));
        CreateMap<FuelFlaringAndLosses, FuelFlaringAndLossesDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(FuelFlaringAndLosses)
                    )
                    ));
        CreateMap<FuelFlaringAndLossesOverride, FuelFlaringAndLossesOverrideDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(FuelFlaringAndLossesOverride)
                    )
                    ));
        CreateMap<NetSalesGas, NetSalesGasDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(NetSalesGas)
                    )
                    ));
        CreateMap<NetSalesGasOverride, NetSalesGasOverrideDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(NetSalesGasOverride)
                    )
                    ));
        CreateMap<Co2Emissions, Co2EmissionsDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(Co2Emissions)
                    )
                    ));
        CreateMap<Co2EmissionsOverride, Co2EmissionsOverrideDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(Co2EmissionsOverride)
                    )
                    ));
        CreateMap<DeferredOilProduction, DeferredOilProductionDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredOilProduction)
                    )
                    ));
        CreateMap<DeferredGasProduction, DeferredGasProductionDto>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesToDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredGasProduction)
                    )
                    ));
        CreateMap<ProductionProfileNGL, ProductionProfileNGLDto>();
        CreateMap<ImportedElectricity, ImportedElectricityDto>();
        CreateMap<ImportedElectricityOverride, ImportedElectricityOverrideDto>();
        CreateMap<Co2Intensity, Co2IntensityDto>();

        CreateMap<UpdateDrainageStrategyDto, DrainageStrategy>();
        CreateMap<UpdateDrainageStrategyWithProfilesDto, DrainageStrategy>();
        CreateMap<UpdateFuelFlaringAndLossesOverrideDto, FuelFlaringAndLossesOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(FuelFlaringAndLossesOverride)
                    )
                    ));
        CreateMap<UpdateNetSalesGasOverrideDto, NetSalesGasOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(NetSalesGasOverride)
                    )
                    ));
        CreateMap<UpdateImportedElectricityOverrideDto, ImportedElectricityOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ImportedElectricityOverride)
                    )
                    ));
        CreateMap<UpdateCo2EmissionsOverrideDto, Co2EmissionsOverride>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(Co2EmissionsOverride)
                    )
                    ));
        CreateMap<UpdateProductionProfileOilDto, ProductionProfileOil>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileOil)
                    )
                    ));
        CreateMap<UpdateProductionProfileGasDto, ProductionProfileGas>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileGas)
                    )
                    ));
        CreateMap<UpdateDeferredOilProductionDto, DeferredOilProduction>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredOilProduction)
                    )
                    ));
        CreateMap<UpdateDeferredGasProductionDto, DeferredGasProduction>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(DeferredGasProduction)
                    )
                    ));
        CreateMap<UpdateProductionProfileWaterDto, ProductionProfileWater>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWater)
                    )
                    ));
        CreateMap<UpdateProductionProfileWaterInjectionDto, ProductionProfileWaterInjection>()
            .ForMember(
                dest => dest.Values,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                    ConvertValuesFromDTO(src.Values,
                    (PhysUnit)Enum.Parse(typeof(PhysUnit), context.Items["ConversionUnit"].ToString() ?? throw new InvalidOperationException()),
                    nameof(ProductionProfileWaterInjection)
                    )
                    ));

        CreateMap<CreateDrainageStrategyDto, DrainageStrategy>();
    }

    private static readonly Dictionary<string, double> ConversionFactors = new Dictionary<string, double>
    {
        { nameof(Co2Emissions), 1_000_000 },
        { nameof(Co2EmissionsOverride), 1_000_000 },
        { nameof(ProductionProfileNGL), 1_000_000 },
        { nameof(ProductionProfileOil), 1_000_000 },
        { nameof(ProductionProfileWater), 1_000_000 },
        { nameof(ProductionProfileWaterInjection), 1_000_000 },
        { nameof(ProductionProfileGas), 1_000_000_000 },
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
                case nameof(ProductionProfileWater):
                case nameof(ProductionProfileWaterInjection):
                    return toDto ? 6.290 * returnValue : 1.0 / 6.290 * returnValue;
                case nameof(ProductionProfileGas):
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
