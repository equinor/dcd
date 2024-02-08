using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class WellProjectWellProfile : Profile
{
    public WellProjectWellProfile()
    {
        CreateMap<WellProjectWell, WellProjectWellDto>();

        CreateMap<UpdateWellProjectWellDto, WellProjectWell>();
    }
}