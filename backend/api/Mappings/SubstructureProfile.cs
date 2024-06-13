using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class SubstructureProfile : Profile
{
    public SubstructureProfile()
    {
        CreateMap<Substructure, SubstructureWithProfilesDto>();
        CreateMap<Substructure, SubstructureDto>();
        CreateMap<SubstructureCostProfile, SubstructureCostProfileDto>();
        CreateMap<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto>();
        CreateMap<SubstructureCessationCostProfile, SubstructureCessationCostProfileDto>();

        CreateMap<APIUpdateSubstructureWithProfilesDto, Substructure>();
        CreateMap<APIUpdateSubstructureDto, Substructure>();
        CreateMap<PROSPUpdateSubstructureDto, Substructure>();
        CreateMap<UpdateSubstructureCostProfileDto, SubstructureCostProfile>();
        CreateMap<UpdateSubstructureCostProfileOverrideDto, SubstructureCostProfileOverride>();
        CreateMap<CreateSubstructureCostProfileOverrideDto, SubstructureCostProfileOverride>();

        CreateMap<CreateSubstructureDto, Substructure>();
    }
}
