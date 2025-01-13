using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Dtos.Create;
using api.Features.Assets.CaseAssets.WellProjects.Services;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Features.TechnicalInput.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.WellProjects;

[ApiController]
[Route("projects/{projectId}/cases/{caseId}/well-projects")]
[AuthorizeActionType(ActionType.Edit)]
public class WellProjectsController(
    WellProjectService wellProjectService,
    WellProjectTimeSeriesService wellProjectTimeSeriesService)
    : ControllerBase
{
    [HttpPut("{wellProjectId}")]
    public async Task<WellProjectDto> UpdateWellProject(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] UpdateWellProjectDto dto)
    {
        return await wellProjectService.UpdateWellProject(projectId, caseId, wellProjectId, dto);
    }

    [HttpPut("{wellProjectId}/wells/{wellId}/drilling-schedule/{drillingScheduleId}")]
    public async Task<DrillingScheduleDto> UpdateWellProjectWellDrillingSchedule(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid wellId,
        [FromRoute] Guid drillingScheduleId,
        [FromBody] UpdateDrillingScheduleDto dto)
    {
        return await wellProjectService.UpdateWellProjectWellDrillingSchedule(projectId, caseId, wellProjectId, wellId, drillingScheduleId, dto);
    }

    [HttpPost("{wellProjectId}/wells/{wellId}/drilling-schedule")]
    public async Task<DrillingScheduleDto> CreateWellProjectWellDrillingSchedule(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid wellId,
        [FromBody] CreateDrillingScheduleDto dto)
    {
        return await wellProjectService.CreateWellProjectWellDrillingSchedule(projectId, caseId, wellProjectId, wellId, dto);
    }

    [HttpPut("{wellProjectId}/oil-producer-cost-profile-override/{costProfileId}")]
    public async Task<OilProducerCostProfileOverrideDto> UpdateOilProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOilProducerCostProfileOverrideDto dto)
    {
        return await wellProjectTimeSeriesService.UpdateOilProducerCostProfileOverride(projectId, caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPut("{wellProjectId}/gas-producer-cost-profile-override/{costProfileId}")]
    public async Task<GasProducerCostProfileOverrideDto> UpdateGasProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateGasProducerCostProfileOverrideDto dto)
    {
        return await wellProjectTimeSeriesService.UpdateGasProducerCostProfileOverride(projectId, caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPut("{wellProjectId}/water-injector-cost-profile-override/{costProfileId}")]
    public async Task<WaterInjectorCostProfileOverrideDto> UpdateWaterInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateWaterInjectorCostProfileOverrideDto dto)
    {
        return await wellProjectTimeSeriesService.UpdateWaterInjectorCostProfileOverride(projectId, caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPut("{wellProjectId}/gas-injector-cost-profile-override/{costProfileId}")]
    public async Task<GasInjectorCostProfileOverrideDto> UpdateGasInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateGasInjectorCostProfileOverrideDto dto)
    {
        return await wellProjectTimeSeriesService.UpdateGasInjectorCostProfileOverride(projectId, caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPost("{wellProjectId}/oil-producer-cost-profile-override")]
    public async Task<OilProducerCostProfileOverrideDto> CreateOilProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateOilProducerCostProfileOverrideDto dto)
    {
        return await wellProjectTimeSeriesService.CreateOilProducerCostProfileOverride(projectId, caseId, wellProjectId, dto);
    }

    [HttpPost("{wellProjectId}/gas-producer-cost-profile-override")]
    public async Task<GasProducerCostProfileOverrideDto> CreateGasProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateGasProducerCostProfileOverrideDto dto)
    {
        return await wellProjectTimeSeriesService.CreateGasProducerCostProfileOverride(projectId, caseId, wellProjectId, dto);
    }

    [HttpPost("{wellProjectId}/water-injector-cost-profile-override")]
    public async Task<WaterInjectorCostProfileOverrideDto> CreateWaterInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateWaterInjectorCostProfileOverrideDto dto)
    {
        return await wellProjectTimeSeriesService.CreateWaterInjectorCostProfileOverride(projectId, caseId, wellProjectId, dto);
    }

    [HttpPost("{wellProjectId}/gas-injector-cost-profile-override")]
    public async Task<GasInjectorCostProfileOverrideDto> CreateGasInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateGasInjectorCostProfileOverrideDto dto)
    {
        return await wellProjectTimeSeriesService.CreateGasInjectorCostProfileOverride(projectId, caseId, wellProjectId, dto);
    }
}
