using api.Features.Assets.CaseAssets.WellProjects;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.WellProjects.GasInjectorCostProfileOverrides.Dtos;
using api.Features.Profiles.WellProjects.GasProducerCostProfileOverrides.Dtos;
using api.Features.Profiles.WellProjects.OilProducerCostProfileOverrides.Dtos;
using api.Features.Profiles.WellProjects.WaterInjectorCostProfileOverrides.Dtos;
using api.Features.Stea.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class WellProjectProfile : Profile
{
    public WellProjectProfile()
    {
        CreateMap<WellProject, WellProjectDto>();
        CreateMap<OilProducerCostProfile, OilProducerCostProfileDto>();
        CreateMap<OilProducerCostProfileOverride, OilProducerCostProfileOverrideDto>();
        CreateMap<GasProducerCostProfile, GasProducerCostProfileDto>();
        CreateMap<GasProducerCostProfileOverride, GasProducerCostProfileOverrideDto>();
        CreateMap<WaterInjectorCostProfile, WaterInjectorCostProfileDto>();
        CreateMap<WaterInjectorCostProfileOverride, WaterInjectorCostProfileOverrideDto>();
        CreateMap<GasInjectorCostProfile, GasInjectorCostProfileDto>();
        CreateMap<GasInjectorCostProfileOverride, GasInjectorCostProfileOverrideDto>();
        CreateMap<WellProjectWell, WellProjectWellDto>().ReverseMap();

        CreateMap<UpdateWellProjectDto, WellProject>();
        CreateMap<UpdateOilProducerCostProfileOverrideDto, OilProducerCostProfileOverride>();
        CreateMap<UpdateGasProducerCostProfileOverrideDto, GasProducerCostProfileOverride>();
        CreateMap<UpdateWaterInjectorCostProfileOverrideDto, WaterInjectorCostProfileOverride>();
        CreateMap<UpdateGasInjectorCostProfileOverrideDto, GasInjectorCostProfileOverride>();

        CreateMap<CreateOilProducerCostProfileOverrideDto, OilProducerCostProfileOverride>();
        CreateMap<CreateGasProducerCostProfileOverrideDto, GasProducerCostProfileOverride>();
        CreateMap<CreateWaterInjectorCostProfileOverrideDto, WaterInjectorCostProfileOverride>();
        CreateMap<CreateGasInjectorCostProfileOverrideDto, GasInjectorCostProfileOverride>();
    }
}
