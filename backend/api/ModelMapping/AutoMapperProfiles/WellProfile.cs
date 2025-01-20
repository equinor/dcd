using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class WellProfile : Profile
{
    public WellProfile()
    {
        CreateMap<TimeSeriesScheduleDto, DrillingSchedule>().ReverseMap();
        CreateMap<CreateTimeSeriesScheduleDto, DrillingSchedule>();
        CreateMap<UpdateTimeSeriesScheduleDto, DrillingSchedule>();
    }
}
