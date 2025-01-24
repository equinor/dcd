using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class OnshorePowerSupplyProfile : Profile
{
    public OnshorePowerSupplyProfile()
    {
        CreateMap<OnshorePowerSupply, OnshorePowerSupplyDto>();
        CreateMap<OnshorePowerSupplyCostProfile, TimeSeriesCostDto>();
        CreateMap<OnshorePowerSupplyCostProfileOverride, TimeSeriesCostOverrideDto>();

        CreateMap<UpdateTimeSeriesCostDto, OnshorePowerSupplyCostProfile>();
        CreateMap<UpdateTimeSeriesCostOverrideDto, OnshorePowerSupplyCostProfileOverride>();
        CreateMap<CreateTimeSeriesCostOverrideDto, OnshorePowerSupplyCostProfileOverride>();
    }
}
