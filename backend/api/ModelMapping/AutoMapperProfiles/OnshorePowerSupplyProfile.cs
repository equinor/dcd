using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Profiles.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Profiles.Dtos.Create;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Profiles.Dtos.Update;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Update;
using api.Models;

using AutoMapper;


public class OnshorePowerSupplyProfile : Profile
{
    public OnshorePowerSupplyProfile()
    {
        CreateMap<OnshorePowerSupply, OnshorePowerSupplyDto>();
        CreateMap<OnshorePowerSupply, OnshorePowerSupplyWithProfilesDto>();
        CreateMap<OnshorePowerSupplyCostProfile, OnshorePowerSupplyCostProfileDto>();
        CreateMap<OnshorePowerSupplyCostProfileOverride, OnshorePowerSupplyCostProfileOverrideDto>();

        CreateMap<UpdateOnshorePowerSupplyDto, OnshorePowerSupply>();
        CreateMap<UpdateOnshorePowerSupplyCostProfileDto, OnshorePowerSupplyCostProfile>();
        CreateMap<UpdateOnshorePowerSupplyCostProfileOverrideDto, OnshorePowerSupplyCostProfileOverride>();
        CreateMap<CreateOnshorePowerSupplyCostProfileOverrideDto, OnshorePowerSupplyCostProfileOverride>();
    }
}
