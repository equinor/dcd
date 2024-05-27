using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class SurfProfile : Profile
{
    public SurfProfile()
    {
        CreateMap<Surf, SurfDto>();
        CreateMap<Surf, SurfWithProfilesDto>();
        CreateMap<SurfCostProfile, SurfCostProfileDto>();
        CreateMap<SurfCostProfileOverride, SurfCostProfileOverrideDto>();
        CreateMap<SurfCessationCostProfile, SurfCessationCostProfileDto>();

        CreateMap<APIUpdateSurfDto, Surf>();
        CreateMap<APIUpdateSurfWithProfilesDto, Surf>();
        CreateMap<PROSPUpdateSurfDto, Surf>();
        CreateMap<UpdateSurfCostProfileDto, SurfCostProfile>();
        CreateMap<UpdateSurfCostProfileOverrideDto, SurfCostProfileOverride>();

        CreateMap<CreateSurfDto, Surf>();
    }
}
