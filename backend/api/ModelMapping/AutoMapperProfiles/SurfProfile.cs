using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class SurfProfile : Profile
{
    public SurfProfile()
    {
        CreateMap<Surf, SurfDto>();
        CreateMap<SurfCostProfile, TimeSeriesCostDto>();
        CreateMap<SurfCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<SurfCessationCostProfile, TimeSeriesCostDto>();

        CreateMap<UpdateTimeSeriesCostDto, SurfCostProfile>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, SurfCostProfileOverride>();
        CreateMap<CreateTimeSeriesCostOverrideDto, SurfCostProfileOverride>();
    }
}
