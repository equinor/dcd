using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class TopsideProfile : Profile
{
    public TopsideProfile()
    {
        CreateMap<Topside, TopsideDto>();
        CreateMap<Topside, TopsideWithProfilesDto>();
        CreateMap<TopsideCostProfile, TopsideCostProfileDto>();
        CreateMap<TopsideCostProfileOverride, TopsideCostProfileOverrideDto>();
        CreateMap<TopsideCessationCostProfile, TopsideCessationCostProfileDto>();

        CreateMap<APIUpdateTopsideWithProfilesDto, Topside>();
        CreateMap<APIUpdateTopsideDto, Topside>();
        CreateMap<PROSPUpdateTopsideDto, Topside>();
        CreateMap<UpdateTopsideCostProfileDto, TopsideCostProfile>();
        CreateMap<UpdateTopsideCostProfileOverrideDto, TopsideCostProfileOverride>();

        CreateMap<CreateTopsideDto, Topside>();
    }
}
