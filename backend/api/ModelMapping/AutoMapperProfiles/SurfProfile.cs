using api.Features.Assets.CaseAssets.Surfs;
using api.Features.Cases.GetWithAssets;
using api.Features.Profiles.Surfs.SurfCostProfileOverrides.Dtos;
using api.Features.Profiles.Surfs.SurfCostProfiles.Dtos;
using api.Features.Stea.Dtos;
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
