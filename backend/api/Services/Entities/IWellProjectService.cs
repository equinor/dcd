using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellProjectService
{
    Task<WellProjectWithProfilesDto> CopyWellProject(Guid wellProjectId, Guid sourceCaseId);
    Task<WellProject> CreateWellProject(Guid projectId, Guid sourceCaseId, CreateWellProjectDto wellProjectDto);
    Task<WellProjectWithProfilesDto> UpdateWellProjectAndCostProfiles(WellProjectWithProfilesDto updatedWellProjectDto);
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
}
