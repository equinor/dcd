using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class WellProjectProfile : Profile
{
    public WellProjectProfile()
    {
        CreateMap<WellProject, WellProjectDto>();
        CreateMap<WellProjectWell, WellProjectWellDto>().ReverseMap();
    }
}
