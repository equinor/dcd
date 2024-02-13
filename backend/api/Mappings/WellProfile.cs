using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class WellProfile : Profile
{
    public WellProfile()
    {
        CreateMap<Well, WellDto>();

        CreateMap<CreateWellDto, Well>();
    }
}
