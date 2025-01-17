using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
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
