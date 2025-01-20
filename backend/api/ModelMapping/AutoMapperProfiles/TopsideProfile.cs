using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class TopsideProfile : Profile
{
    public TopsideProfile()
    {
        CreateMap<Topside, TopsideDto>();
        CreateMap<TopsideCostProfile, TimeSeriesCostDto>();
        CreateMap<TopsideCostProfileOverride, TimeSeriesCostOverrideDto>();
        CreateMap<TopsideCessationCostProfile, TimeSeriesCostDto>();

        CreateMap<UpdateTimeSeriesCostDto, TopsideCostProfile>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, TopsideCostProfileOverride>();
        CreateMap<CreateTimeSeriesCostOverrideDto, TopsideCostProfileOverride>();
    }
}
