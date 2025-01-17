using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfileOverrides.Dtos;
using api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class OnshorePowerSupplyProfile : Profile
{
    public OnshorePowerSupplyProfile()
    {
        CreateMap<OnshorePowerSupply, OnshorePowerSupplyDto>();
        CreateMap<OnshorePowerSupplyCostProfile, OnshorePowerSupplyCostProfileDto>();
        CreateMap<OnshorePowerSupplyCostProfileOverride, OnshorePowerSupplyCostProfileOverrideDto>();

        CreateMap<UpdateOnshorePowerSupplyCostProfileDto, OnshorePowerSupplyCostProfile>();
        CreateMap<UpdateOnshorePowerSupplyCostProfileOverrideDto, OnshorePowerSupplyCostProfileOverride>();
        CreateMap<CreateOnshorePowerSupplyCostProfileOverrideDto, OnshorePowerSupplyCostProfileOverride>();
    }
}
