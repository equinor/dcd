using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<DevelopmentOperationalWellCosts, DevelopmentOperationalWellCostsDto>().ReverseMap();
        CreateMap<ExplorationOperationalWellCosts, ExplorationOperationalWellCostsDto>().ReverseMap();
        CreateMap<Substructure, SubstructureDto>();
        CreateMap<Surf, SurfDto>();
        CreateMap<Topside, TopsideDto>();
        CreateMap<Transport, TransportDto>();
        CreateMap<OnshorePowerSupply, OnshorePowerSupplyDto>();
        CreateMap<DrainageStrategy, DrainageStrategyDto>();
        CreateMap<TimeSeriesScheduleDto, DrillingSchedule>().ReverseMap();
        CreateMap<Exploration, ExplorationDto>();
        CreateMap<ExplorationWell, ExplorationWellDto>().ReverseMap();
        CreateMap<WellProject, WellProjectDto>();
        CreateMap<WellProjectWell, WellProjectWellDto>().ReverseMap();
    }
}
