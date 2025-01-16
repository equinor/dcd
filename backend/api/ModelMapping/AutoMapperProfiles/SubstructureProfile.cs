using api.Features.Assets.CaseAssets.Substructures;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Substructures.SubstructureCostProfileOverrides.Dtos;
using api.Features.Profiles.Substructures.SubstructureCostProfiles.Dtos;
using api.Features.Stea.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class SubstructureProfile : Profile
{
    public SubstructureProfile()
    {
        CreateMap<Substructure, SubstructureDto>();
        CreateMap<SubstructureCostProfile, SubstructureCostProfileDto>();
        CreateMap<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto>();
        CreateMap<SubstructureCessationCostProfile, SubstructureCessationCostProfileDto>();

        CreateMap<UpdateSubstructureDto, Substructure>();
        CreateMap<ProspUpdateSubstructureDto, Substructure>();
        CreateMap<UpdateSubstructureCostProfileDto, SubstructureCostProfile>();
        CreateMap<UpdateSubstructureCostProfileOverrideDto, SubstructureCostProfileOverride>();
        CreateMap<CreateSubstructureCostProfileOverrideDto, SubstructureCostProfileOverride>();
    }
}
