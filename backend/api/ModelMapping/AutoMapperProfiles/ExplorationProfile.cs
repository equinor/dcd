using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Explorations.CountryOfficeCosts.Dtos;
using api.Features.Profiles.Explorations.GAndGAdminCostOverrides.Dtos;
using api.Features.Profiles.Explorations.SeismicAcquisitionAndProcessings.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class ExplorationProfile : Profile
{
    public ExplorationProfile()
    {
        CreateMap<Exploration, ExplorationDto>();
        CreateMap<ExplorationWellCostProfile, ExplorationWellCostProfileDto>();
        CreateMap<AppraisalWellCostProfile, AppraisalWellCostProfileDto>();
        CreateMap<SidetrackCostProfile, SidetrackCostProfileDto>();
        CreateMap<GAndGAdminCost, GAndGAdminCostDto>();
        CreateMap<GAndGAdminCostOverride, GAndGAdminCostOverrideDto>();
        CreateMap<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto>();
        CreateMap<CountryOfficeCost, CountryOfficeCostDto>();
        CreateMap<ExplorationWell, ExplorationWellDto>().ReverseMap();

        CreateMap<UpdateGAndGAdminCostOverrideDto, GAndGAdminCostOverride>();
        CreateMap<UpdateSeismicAcquisitionAndProcessingDto, SeismicAcquisitionAndProcessing>();
        CreateMap<UpdateCountryOfficeCostDto, CountryOfficeCost>();

        CreateMap<CreateGAndGAdminCostOverrideDto, GAndGAdminCostOverride>();
        CreateMap<CreateSeismicAcquisitionAndProcessingDto, SeismicAcquisitionAndProcessing>();
        CreateMap<CreateCountryOfficeCostDto, CountryOfficeCost>();
    }
}
