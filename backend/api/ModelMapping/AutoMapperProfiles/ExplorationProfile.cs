using api.Features.Assets.CaseAssets.Explorations.Profiles.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Profiles.Dtos.Create;
using api.Features.Cases.GetWithAssets;
using api.Features.TechnicalInput.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class ExplorationProfile : Profile
{
    public ExplorationProfile()
    {
        CreateMap<Exploration, ExplorationDto>();
        CreateMap<Exploration, ExplorationWithProfilesDto>();
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
