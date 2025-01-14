using api.Features.Assets.CaseAssets.Surfs.Profiles.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Profiles.Dtos.Create;
using api.Features.Assets.CaseAssets.Surfs.Profiles.Dtos.Update;
using api.Features.Assets.CaseAssets.Surfs.Update;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class SurfProfile : Profile
{
    public SurfProfile()
    {
        CreateMap<Surf, SurfDto>();
        CreateMap<Surf, SurfWithProfilesDto>();
        CreateMap<SurfCostProfile, SurfCostProfileDto>();
        CreateMap<SurfCostProfileOverride, SurfCostProfileOverrideDto>();
        CreateMap<SurfCessationCostProfile, SurfCessationCostProfileDto>();

        CreateMap<UpdateSurfDto, Surf>();
        CreateMap<ProspUpdateSurfDto, Surf>();
        CreateMap<UpdateSurfCostProfileDto, SurfCostProfile>();
        CreateMap<UpdateSurfCostProfileOverrideDto, SurfCostProfileOverride>();
        CreateMap<CreateSurfCostProfileOverrideDto, SurfCostProfileOverride>();
    }
}
