using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Mappings;

public class ExplorationProfile : Profile
{
    public ExplorationProfile()
    {
        CreateMap<Exploration, ExplorationDto>();
        CreateMap<ExplorationWellCostProfile, ExplorationWellCostProfileDto>();
        CreateMap<AppraisalWellCostProfile, AppraisalWellCostProfileDto>();
        CreateMap<SidetrackCostProfile, SidetrackCostProfileDto>();
        CreateMap<GAndGAdminCost, GAndGAdminCostDto>();
        CreateMap<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto>();
        CreateMap<CountryOfficeCost, CountryOfficeCostDto>();
        CreateMap<ExplorationWell, ExplorationWellDto>();

        CreateMap<UpdateExplorationDto, Exploration>();

        CreateMap<CreateExplorationDto, Exploration>();
        CreateMap<ExplorationDto, UpdateExplorationDto>(); // Temp fix
    }
}