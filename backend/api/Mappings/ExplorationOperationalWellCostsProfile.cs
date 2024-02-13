using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class ExplorationOperationalWellCostsProfile : Profile
{
    public ExplorationOperationalWellCostsProfile()
    {
        CreateMap<ExplorationOperationalWellCostsProfile, ExplorationOperationalWellCostsDto>();
    }
}
