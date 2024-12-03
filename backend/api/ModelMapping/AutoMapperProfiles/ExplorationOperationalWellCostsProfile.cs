using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class ExplorationOperationalWellCostsProfile : Profile
{
    public ExplorationOperationalWellCostsProfile()
    {
        CreateMap<ExplorationOperationalWellCosts, ExplorationOperationalWellCostsDto>().ReverseMap();
        CreateMap<UpdateExplorationOperationalWellCostsDto, ExplorationOperationalWellCosts>();
    }
}
