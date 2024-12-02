using api.Dtos;
using api.Features.TechnicalInput.Dtos;

namespace api.Services;

public interface IWellProjectTimeSeriesService
{
    Task<OilProducerCostProfileOverrideDto> UpdateOilProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateOilProducerCostProfileOverrideDto updateDto
    );

    Task<GasProducerCostProfileOverrideDto> UpdateGasProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGasProducerCostProfileOverrideDto updateDto
    );

    Task<WaterInjectorCostProfileOverrideDto> UpdateWaterInjectorCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateWaterInjectorCostProfileOverrideDto updateDto
    );

    Task<GasInjectorCostProfileOverrideDto> UpdateGasInjectorCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGasInjectorCostProfileOverrideDto updateDto
    );

    Task<OilProducerCostProfileOverrideDto> CreateOilProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateOilProducerCostProfileOverrideDto createProfileDto
    );

    Task<GasProducerCostProfileOverrideDto> CreateGasProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateGasProducerCostProfileOverrideDto createProfileDto
    );

    Task<WaterInjectorCostProfileOverrideDto> CreateWaterInjectorCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateWaterInjectorCostProfileOverrideDto createProfileDto
    );

    Task<GasInjectorCostProfileOverrideDto> CreateGasInjectorCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateGasInjectorCostProfileOverrideDto createProfileDto
    );
}
