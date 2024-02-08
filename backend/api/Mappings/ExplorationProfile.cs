using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class ExplorationProfile : Profile
{
    public ExplorationProfile()
    {
        CreateMap<Exploration, ExplorationDto>();

        CreateMap<CreateExplorationDto, Exploration>();
    }
}