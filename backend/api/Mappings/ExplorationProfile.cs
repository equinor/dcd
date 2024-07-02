using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

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
        CreateMap<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto>();
        CreateMap<CountryOfficeCost, CountryOfficeCostDto>();
        CreateMap<ExplorationWell, ExplorationWellDto>().ReverseMap();

        CreateMap<UpdateExplorationDto, Exploration>();
        CreateMap<UpdateExplorationWithProfilesDto, Exploration>();
        CreateMap<UpdateSeismicAcquisitionAndProcessingDto, SeismicAcquisitionAndProcessing>();
        CreateMap<UpdateCountryOfficeCostDto, CountryOfficeCost>();
        CreateMap<UpdateExplorationWellCostProfileDto, ExplorationWellCostProfile>();
        CreateMap<UpdateAppraisalWellCostProfileDto, AppraisalWellCostProfile>();
        CreateMap<UpdateSidetrackCostProfileDto, SidetrackCostProfile>();

        CreateMap<CreateSeismicAcquisitionAndProcessingDto, SeismicAcquisitionAndProcessing>();
        CreateMap<CreateCountryOfficeCostDto, CountryOfficeCost>();

        CreateMap<CreateExplorationDto, Exploration>();
        CreateMap<ExplorationWithProfilesDto, UpdateExplorationWithProfilesDto>(); // Temp fix
    }
}
