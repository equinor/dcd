using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class TopsideProfile : Profile
{
    public TopsideProfile()
    {
        CreateMap<Topside, TopsideDto>();

        CreateMap<CreateTopsideDto, Topside>();
    }
}