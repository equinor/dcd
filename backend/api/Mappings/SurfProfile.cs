using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class SurfProfile : Profile
{
    public SurfProfile()
    {
        CreateMap<Surf, SurfDto>();
        CreateMap<SurfCostProfile, SurfCostProfileDto>();
        CreateMap<SurfCostProfileOverride, SurfCostProfileOverrideDto>();
        CreateMap<SurfCessationCostProfile, SurfCessationCostProfileDto>();

        CreateMap<UpdateSurfDto, Surf>();
        CreateMap<UpdateSurfCostProfileOverrideDto, SurfCostProfileOverride>();

        CreateMap<CreateSurfDto, Surf>();
    }
}
