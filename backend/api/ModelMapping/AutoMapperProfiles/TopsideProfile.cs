using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Topsides.TopsideCostProfileOverrides.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class TopsideProfile : Profile
{
    public TopsideProfile()
    {
        CreateMap<Topside, TopsideDto>();
        CreateMap<TopsideCostProfile, TopsideCostProfileDto>();
        CreateMap<TopsideCostProfileOverride, TopsideCostProfileOverrideDto>();
        CreateMap<TopsideCessationCostProfile, TopsideCessationCostProfileDto>();

        CreateMap<UpdateTopsideCostProfileDto, TopsideCostProfile>();
        CreateMap<UpdateTopsideCostProfileOverrideDto, TopsideCostProfileOverride>();
        CreateMap<CreateTopsideCostProfileOverrideDto, TopsideCostProfileOverride>();
    }
}
