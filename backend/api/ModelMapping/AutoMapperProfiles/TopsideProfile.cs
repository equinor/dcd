using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Create;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Update;
using api.Features.Cases.GetWithAssets;
using api.Features.Stea.Dtos;
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

        CreateMap<APIUpdateTopsideDto, Topside>();
        CreateMap<PROSPUpdateTopsideDto, Topside>();
        CreateMap<UpdateTopsideCostProfileDto, TopsideCostProfile>();
        CreateMap<UpdateTopsideCostProfileOverrideDto, TopsideCostProfileOverride>();
        CreateMap<CreateTopsideCostProfileOverrideDto, TopsideCostProfileOverride>();
    }
}
