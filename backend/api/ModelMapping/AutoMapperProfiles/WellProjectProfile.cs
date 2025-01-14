using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Dtos.Create;
using api.Features.Cases.GetWithAssets;
using api.Features.TechnicalInput.Dtos;
using api.Models;

using AutoMapper;

namespace api.ModelMapping.AutoMapperProfiles;

public class WellProjectProfile : Profile
{
    public WellProjectProfile()
    {
        CreateMap<WellProject, WellProjectDto>();
        CreateMap<WellProject, WellProjectWithProfilesDto>();
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
