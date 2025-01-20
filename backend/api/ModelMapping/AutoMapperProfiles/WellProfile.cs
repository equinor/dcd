using api.Features.Assets.CaseAssets.DrillingSchedules.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class WellProfile : Profile
{
    public WellProfile()
    {
        CreateMap<DrillingScheduleDto, DrillingSchedule>().ReverseMap();
        CreateMap<CreateDrillingScheduleDto, DrillingSchedule>();
        CreateMap<UpdateDrillingScheduleDto, DrillingSchedule>();
    }
}
