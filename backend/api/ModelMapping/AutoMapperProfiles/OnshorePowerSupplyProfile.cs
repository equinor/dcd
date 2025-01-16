using api.Features.Assets.CaseAssets.OnshorePowerSupplies;
using api.Features.Cases.GetWithAssets;
using api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfileOverrides.Dtos;
using api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfiles.Dtos;
using api.Features.Stea.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class OnshorePowerSupplyProfile : Profile
{
    public OnshorePowerSupplyProfile()
    {
        CreateMap<OnshorePowerSupply, OnshorePowerSupplyDto>();
        CreateMap<OnshorePowerSupply, OnshorePowerSupplyWithProfilesDto>();
        CreateMap<OnshorePowerSupplyCostProfile, OnshorePowerSupplyCostProfileDto>();
        CreateMap<OnshorePowerSupplyCostProfileOverride, OnshorePowerSupplyCostProfileOverrideDto>();

        CreateMap<UpdateOnshorePowerSupplyDto, OnshorePowerSupply>();
        CreateMap<ProspUpdateOnshorePowerSupplyDto, OnshorePowerSupply>();
        CreateMap<UpdateOnshorePowerSupplyCostProfileDto, OnshorePowerSupplyCostProfile>();
        CreateMap<UpdateOnshorePowerSupplyCostProfileOverrideDto, OnshorePowerSupplyCostProfileOverride>();
        CreateMap<CreateOnshorePowerSupplyCostProfileOverrideDto, OnshorePowerSupplyCostProfileOverride>();
    }
}
