using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

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
