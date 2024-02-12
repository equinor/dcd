using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class DrainageStrategyProfile : Profile
{
    public DrainageStrategyProfile()
    {
        CreateMap<DrainageStrategy, DrainageStrategyDto>();
        CreateMap<ProductionProfileOil, ProductionProfileOilDto>();
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

        CreateMap<CreateDrainageStrategyDto, DrainageStrategy>();
    }
}
