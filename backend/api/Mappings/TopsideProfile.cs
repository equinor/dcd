using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class TopsideProfile : Profile
{
    public TopsideProfile()
    {
        CreateMap<Topside, TopsideDto>();
        CreateMap<TopsideCostProfile, TopsideCostProfileDto>();
        CreateMap<TopsideCostProfileOverride, TopsideCostProfileOverrideDto>();
        CreateMap<TopsideCessationCostProfile, TopsideCessationCostProfileDto>();

        CreateMap<UpdateTopsideDto, Topside>();
        CreateMap<UpdateTopsideCostProfileOverrideDto, TopsideCostProfileOverride>();

        CreateMap<CreateTopsideDto, Topside>();
    }
}