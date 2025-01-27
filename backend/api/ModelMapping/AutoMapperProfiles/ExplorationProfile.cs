using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class ExplorationProfile : Profile
{
    public ExplorationProfile()
    {
        CreateMap<Exploration, ExplorationDto>();
        CreateMap<SidetrackCostProfile, TimeSeriesCostDto>();
        CreateMap<SeismicAcquisitionAndProcessing, TimeSeriesCostDto>();
        CreateMap<CountryOfficeCost, TimeSeriesCostDto>();
        CreateMap<ExplorationWell, ExplorationWellDto>().ReverseMap();

        CreateMap<UpdateTimeSeriesCostDto, SeismicAcquisitionAndProcessing>();
        CreateMap<UpdateTimeSeriesCostDto, CountryOfficeCost>();

        CreateMap<CreateTimeSeriesCostDto, SeismicAcquisitionAndProcessing>();
        CreateMap<CreateTimeSeriesCostDto, CountryOfficeCost>();
    }
}
