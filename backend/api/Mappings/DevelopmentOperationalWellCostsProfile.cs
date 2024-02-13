using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class DevelopmentOperationalWellCostsProfile : Profile
{
    public DevelopmentOperationalWellCostsProfile()
    {
        CreateMap<DevelopmentOperationalWellCosts, DevelopmentOperationalWellCostsDto>().ReverseMap();
    }
}
