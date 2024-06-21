using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellProjectService
{
    Task<WellProject> CreateWellProject(Guid projectId, Guid sourceCaseId, CreateWellProjectDto wellProjectDto);
    Task<WellProject> GetWellProject(Guid wellProjectId);
    Task<WellProjectDto> UpdateWellProject(
        Guid caseId,
        Guid wellProjectId,
        UpdateWellProjectDto updatedWellProjectDto
    );
    Task<WellProjectWellDto> UpdateWellProjectWell(
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        UpdateWellProjectWellDto updatedWellProjectWellDto
    );

    Task<OilProducerCostProfileOverrideDto> UpdateOilProducerCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateOilProducerCostProfileOverrideDto updateDto
    );

    Task<GasProducerCostProfileOverrideDto> UpdateGasProducerCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGasProducerCostProfileOverrideDto updateDto
    );

    Task<WaterInjectorCostProfileOverrideDto> UpdateWaterInjectorCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateWaterInjectorCostProfileOverrideDto updateDto
    );

    Task<GasInjectorCostProfileOverrideDto> UpdateGasInjectorCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGasInjectorCostProfileOverrideDto updateDto
    );

    Task<OilProducerCostProfileOverrideDto> CreateOilProducerCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        CreateOilProducerCostProfileOverrideDto createProfileDto
    );

    Task<GasProducerCostProfileOverrideDto> CreateGasProducerCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        CreateGasProducerCostProfileOverrideDto createProfileDto
    );

    Task<WaterInjectorCostProfileOverrideDto> CreateWaterInjectorCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        CreateWaterInjectorCostProfileOverrideDto createProfileDto
    );

    Task<GasInjectorCostProfileOverrideDto> CreateGasInjectorCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        CreateGasInjectorCostProfileOverrideDto createProfileDto
    );
}
