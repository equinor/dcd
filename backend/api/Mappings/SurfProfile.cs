using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Create;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Update;
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
        CreateMap<CreateSurfCostProfileOverrideDto, SurfCostProfileOverride>();

        CreateMap<CreateSurfDto, Surf>();
    }
}
