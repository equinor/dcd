using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class DrainageStrategyProfile : Profile
{
    public DrainageStrategyProfile()
    {
        CreateMap<DrainageStrategy, DrainageStrategyDto>();

        CreateMap<CreateDrainageStrategyDto, DrainageStrategy>();
    }
}