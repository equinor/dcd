using api.Features.Assets.CaseAssets.DrillingSchedules.Dtos;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class WellProjectWellProfile : Profile
{
    public WellProjectWellProfile()
    {
        CreateMap<WellProjectWell, WellProjectWellDto>();
        CreateMap<DrillingSchedule, DrillingScheduleDto>();
    }
}
