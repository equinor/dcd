using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class WellProjectProfile : Profile
{
    public WellProjectProfile()
    {
        CreateMap<WellProject, WellProjectDto>();
        CreateMap<OilProducerCostProfile, OilProducerCostProfileDto>();
        CreateMap<OilProducerCostProfileOverride, OilProducerCostProfileOverrideDto>();
        CreateMap<GasProducerCostProfile, GasProducerCostProfileDto>();
        CreateMap<GasProducerCostProfileOverride, GasProducerCostProfileOverrideDto>();
        CreateMap<WaterInjectorCostProfile, WaterInjectorCostProfileDto>();
        CreateMap<WaterInjectorCostProfileOverride, WaterInjectorCostProfileOverrideDto>();
        CreateMap<GasInjectorCostProfile, GasInjectorCostProfileDto>();
        CreateMap<GasInjectorCostProfileOverride, GasInjectorCostProfileOverrideDto>();
        CreateMap<WellProjectWell, WellProjectWellDto>().ReverseMap();

        CreateMap<UpdateWellProjectDto, WellProject>();
        CreateMap<UpdateOilProducerCostProfileOverrideDto, OilProducerCostProfileOverride>();
        CreateMap<UpdateGasProducerCostProfileOverrideDto, GasProducerCostProfileOverride>();
        CreateMap<UpdateWaterInjectorCostProfileOverrideDto, WaterInjectorCostProfileOverride>();
        CreateMap<UpdateGasInjectorCostProfileOverrideDto, GasInjectorCostProfileOverride>();

        CreateMap<CreateWellProjectDto, WellProject>();
        CreateMap<WellProjectDto, UpdateWellProjectDto>(); // Temp fix
    }
}
