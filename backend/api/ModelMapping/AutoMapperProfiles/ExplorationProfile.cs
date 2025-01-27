using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class ExplorationProfile : Profile
{
    public ExplorationProfile()
    {
        CreateMap<Exploration, ExplorationDto>();
        CreateMap<ExplorationWell, ExplorationWellDto>().ReverseMap();
    }
}
