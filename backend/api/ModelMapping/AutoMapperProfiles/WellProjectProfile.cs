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
        CreateMap<WaterInjectorCostProfile, TimeSeriesCostDto>();
        CreateMap<WaterInjectorCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<GasInjectorCostProfile, TimeSeriesCostDto>();
        CreateMap<GasInjectorCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<WellProjectWell, WellProjectWellDto>().ReverseMap();

        CreateMap<UpdateTimeSeriesCostOverrideDto, WaterInjectorCostProfileOverride>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, GasInjectorCostProfileOverride>();

        CreateMap<CreateTimeSeriesCostOverrideDto, WaterInjectorCostProfileOverride>();
        CreateMap<CreateTimeSeriesCostOverrideDto, GasInjectorCostProfileOverride>();
    }
}
