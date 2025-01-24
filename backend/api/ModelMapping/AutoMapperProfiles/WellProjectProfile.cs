using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class WellProjectProfile : Profile
{
    public WellProjectProfile()
    {
        CreateMap<WellProject, WellProjectDto>();
        CreateMap<OilProducerCostProfile, TimeSeriesCostDto>();
        CreateMap<OilProducerCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<GasProducerCostProfile, TimeSeriesCostDto>();
        CreateMap<GasProducerCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<WaterInjectorCostProfile, TimeSeriesCostDto>();
        CreateMap<WaterInjectorCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<GasInjectorCostProfile, TimeSeriesCostDto>();
        CreateMap<GasInjectorCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<WellProjectWell, WellProjectWellDto>().ReverseMap();

        CreateMap<UpdateTimeSeriesCostOverrideDto, OilProducerCostProfileOverride>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, GasProducerCostProfileOverride>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, WaterInjectorCostProfileOverride>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, GasInjectorCostProfileOverride>();

        CreateMap<CreateTimeSeriesCostOverrideDto, OilProducerCostProfileOverride>();
        CreateMap<CreateTimeSeriesCostOverrideDto, GasProducerCostProfileOverride>();
        CreateMap<CreateTimeSeriesCostOverrideDto, WaterInjectorCostProfileOverride>();
        CreateMap<CreateTimeSeriesCostOverrideDto, GasInjectorCostProfileOverride>();
    }
}
