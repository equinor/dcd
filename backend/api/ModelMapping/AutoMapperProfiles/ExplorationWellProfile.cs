using api.Features.Assets.CaseAssets.Explorations.Update.Dtos;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Features.Cases.GetWithAssets;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class ExplorationWellProfile : Profile
{
    public ExplorationWellProfile()
    {
        CreateMap<ExplorationWell, ExplorationWellDto>();
        CreateMap<DrillingSchedule, DrillingScheduleDto>();
        CreateMap<UpdateDrillingScheduleDto, DrillingSchedule>();
    }
}
