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
        CreateMap<ExplorationWellCostProfile, TimeSeriesCostDto>();
        CreateMap<AppraisalWellCostProfile, TimeSeriesCostDto>();
        CreateMap<SidetrackCostProfile, TimeSeriesCostDto>();
        CreateMap<GAndGAdminCost, TimeSeriesCostDto>();
        CreateMap<GAndGAdminCostOverride, TimeSeriesCostOverrideDto>();
        CreateMap<SeismicAcquisitionAndProcessing, TimeSeriesCostDto>();
        CreateMap<CountryOfficeCost, TimeSeriesCostDto>();
        CreateMap<ExplorationWell, ExplorationWellDto>().ReverseMap();

        CreateMap<UpdateTimeSeriesCostOverrideDto, GAndGAdminCostOverride>();
        CreateMap<UpdateTimeSeriesCostDto, SeismicAcquisitionAndProcessing>();
        CreateMap<UpdateTimeSeriesCostDto, CountryOfficeCost>();

        CreateMap<CreateTimeSeriesCostOverrideDto, GAndGAdminCostOverride>();
        CreateMap<CreateTimeSeriesCostDto, SeismicAcquisitionAndProcessing>();
        CreateMap<CreateTimeSeriesCostDto, CountryOfficeCost>();
    }
}
