using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Create;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Update;
using api.Features.Assets.CaseAssets.Substructures.Update;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class SubstructureProfile : Profile
{
    public SubstructureProfile()
    {
        CreateMap<Substructure, SubstructureWithProfilesDto>();
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
