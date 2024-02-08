using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class WellProjectProfile : Profile
{
    public WellProjectProfile()
    {
        CreateMap<WellProject, WellProjectDto>();

        CreateMap<CreateWellProjectDto, WellProject>();
    }
}