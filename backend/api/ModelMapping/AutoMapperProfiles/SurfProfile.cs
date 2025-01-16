using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Create;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Update;
using api.Features.Cases.GetWithAssets;
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

        CreateMap<APIUpdateSurfDto, Surf>();
        CreateMap<PROSPUpdateSurfDto, Surf>();
        CreateMap<UpdateSurfCostProfileDto, SurfCostProfile>();
        CreateMap<UpdateSurfCostProfileOverrideDto, SurfCostProfileOverride>();
        CreateMap<CreateSurfCostProfileOverrideDto, SurfCostProfileOverride>();
    }
}
