using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class SubstructureProfile : Profile
{
    public SubstructureProfile()
    {
        CreateMap<Substructure, SubstructureDto>();
        CreateMap<SubstructureCostProfile, SubstructureCostProfileDto>();
        CreateMap<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto>();
        CreateMap<SubstructureCessationCostProfile, SubstructureCessationCostProfileDto>();

        CreateMap<UpdateSubstructureDto, Substructure>();
        CreateMap<UpdateSubstructureCostProfileOverrideDto, SubstructureCostProfileOverride>();

        CreateMap<CreateSubstructureDto, Substructure>();
    }
}
