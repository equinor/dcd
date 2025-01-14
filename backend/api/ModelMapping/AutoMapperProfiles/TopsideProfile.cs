using api.Features.Assets.CaseAssets.Topsides.Profiles.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Profiles.Dtos.Create;
using api.Features.Assets.CaseAssets.Topsides.Profiles.Dtos.Update;
using api.Features.Assets.CaseAssets.Topsides.Update;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class TopsideProfile : Profile
{
    public TopsideProfile()
    {
        CreateMap<Topside, TopsideDto>();
        CreateMap<Topside, TopsideWithProfilesDto>();
        CreateMap<TopsideCostProfile, TopsideCostProfileDto>();
        CreateMap<TopsideCostProfileOverride, TopsideCostProfileOverrideDto>();
        CreateMap<TopsideCessationCostProfile, TopsideCessationCostProfileDto>();

        CreateMap<UpdateTopsideDto, Topside>();
        CreateMap<ProspUpdateTopsideDto, Topside>();
        CreateMap<UpdateTopsideCostProfileDto, TopsideCostProfile>();
        CreateMap<UpdateTopsideCostProfileOverrideDto, TopsideCostProfileOverride>();
        CreateMap<CreateTopsideCostProfileOverrideDto, TopsideCostProfileOverride>();
    }
}
