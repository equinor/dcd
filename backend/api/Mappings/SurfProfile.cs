using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class SurfProfile : Profile
{
    public SurfProfile()
    {
        CreateMap<Surf, SurfDto>();

        CreateMap<CreateSurfDto, Surf>();
    }
}