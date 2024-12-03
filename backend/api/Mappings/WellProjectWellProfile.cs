using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class WellProjectWellProfile : Profile
{
    public WellProjectWellProfile()
    {
        CreateMap<WellProjectWell, WellProjectWellDto>();
        CreateMap<DrillingSchedule, DrillingScheduleDto>();

        CreateMap<UpdateWellProjectWellWithScheduleDto, WellProjectWell>();
    }
}
