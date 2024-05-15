using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class ExplorationWellProfile : Profile
{
    public ExplorationWellProfile()
    {
        CreateMap<ExplorationWell, ExplorationWellDto>();
        CreateMap<UpdateExplorationWellDto, ExplorationWell>();
        CreateMap<DrillingSchedule, DrillingScheduleDto>();
        CreateMap<UpdateDrillingScheduleDto, DrillingSchedule>();

        CreateMap<CreateExplorationWellDto, ExplorationWell>();
        CreateMap<ExplorationWell, CreateExplorationWellDto>();
    }
}
