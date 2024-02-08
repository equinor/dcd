using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class SubstructureProfile : Profile
{
    public SubstructureProfile()
    {
        CreateMap<Substructure, SubstructureDto>();

        CreateMap<CreateSubstructureDto, Substructure>();
    }
}