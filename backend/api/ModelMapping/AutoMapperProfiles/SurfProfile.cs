using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class SurfProfile : Profile
{
    public SurfProfile()
    {
        CreateMap<Surf, SurfDto>();
        CreateMap<SurfCessationCostProfile, TimeSeriesCostDto>();
    }
}
