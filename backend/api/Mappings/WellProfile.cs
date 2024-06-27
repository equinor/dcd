using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class WellProfile : Profile
{
    public WellProfile()
    {
        CreateMap<Well, WellDto>().ReverseMap();

        CreateMap<CreateWellDto, Well>();
        CreateMap<UpdateWellDto, Well>();

        CreateMap<DrillingScheduleDto, DrillingSchedule>().ReverseMap();
        CreateMap<CreateDrillingScheduleDto, DrillingSchedule>();
        CreateMap<UpdateDrillingScheduleDto, DrillingSchedule>();
    }
}
