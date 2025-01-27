using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class OnshorePowerSupplyProfile : Profile
{
    public OnshorePowerSupplyProfile()
    {
        CreateMap<OnshorePowerSupply, OnshorePowerSupplyDto>();
    }
}
