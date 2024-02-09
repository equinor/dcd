using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class ExplorationWellProfile : Profile
{
    public ExplorationWellProfile()
    {
        CreateMap<ExplorationWell, ExplorationWellDto>();
        CreateMap<DrillingSchedule, DrillingScheduleDto>();

        CreateMap<UpdateExplorationWellDto, ExplorationWell>();
    }
}