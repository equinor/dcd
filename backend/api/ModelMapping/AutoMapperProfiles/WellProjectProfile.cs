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
        CreateMap<GasInjectorCostProfile, TimeSeriesCostDto>();
        CreateMap<GasInjectorCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<WellProjectWell, WellProjectWellDto>().ReverseMap();

        CreateMap<UpdateTimeSeriesCostOverrideDto, GasInjectorCostProfileOverride>();

        CreateMap<CreateTimeSeriesCostOverrideDto, GasInjectorCostProfileOverride>();
    }
}
