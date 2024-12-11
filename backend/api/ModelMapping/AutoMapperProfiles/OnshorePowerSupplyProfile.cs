using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Create;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;
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

        CreateMap<APIUpdateOnshorePowerSupplyDto, OnshorePowerSupply>();
        CreateMap<APIUpdateOnshorePowerSupplyWithProfilesDto, OnshorePowerSupply>();
        CreateMap<PROSPUpdateOnshorePowerSupplyDto, OnshorePowerSupply>();
        CreateMap<UpdateOnshorePowerSupplyCostProfileDto, OnshorePowerSupplyCostProfile>();
        CreateMap<UpdateOnshorePowerSupplyCostProfileOverrideDto, OnshorePowerSupplyCostProfileOverride>();
        CreateMap<CreateOnshorePowerSupplyCostProfileOverrideDto, OnshorePowerSupplyCostProfileOverride>();

        CreateMap<CreateOnshorePowerSupplyDto, OnshorePowerSupply>();
    }
}
