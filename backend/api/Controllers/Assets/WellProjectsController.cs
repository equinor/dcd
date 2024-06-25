using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/well-projects")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class WellProjectsController : ControllerBase
{
    private readonly IWellProjectService _wellProjectService;
    private readonly IWellProjectTimeSeriesService _wellProjectTimeSeriesService;

    public WellProjectsController(
        IWellProjectService wellProjectService,
        IWellProjectTimeSeriesService wellProjectTimeSeriesService
    )
    {
        _wellProjectService = wellProjectService;
        _wellProjectTimeSeriesService = wellProjectTimeSeriesService;
    }

    [HttpPut("{wellProjectId}")]
    public async Task<WellProjectDto> UpdateWellProject(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] UpdateWellProjectDto dto)
    {
        return await _wellProjectService.UpdateWellProject(caseId, wellProjectId, dto);
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
        return await _wellProjectService.UpdateWellProjectWellDrillingSchedule(caseId, wellProjectId, wellId, drillingScheduleId, dto);
    }

    [HttpPost("{wellProjectId}/wells/{wellId}/drilling-schedule")]
    public async Task<DrillingScheduleDto> CreateWellProjectWellDrillingSchedule(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid wellId,
        [FromBody] CreateDrillingScheduleDto dto)
    {
        return await _wellProjectService.CreateWellProjectWellDrillingSchedule(caseId, wellProjectId, wellId, dto);
    }

    [HttpPut("{wellProjectId}/oil-producer-cost-profile-override/{costProfileId}")]
    public async Task<OilProducerCostProfileOverrideDto> UpdateOilProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOilProducerCostProfileOverrideDto dto)
    {
        return await _wellProjectTimeSeriesService.UpdateOilProducerCostProfileOverride(caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPut("{wellProjectId}/gas-producer-cost-profile-override/{costProfileId}")]
    public async Task<GasProducerCostProfileOverrideDto> UpdateGasProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateGasProducerCostProfileOverrideDto dto)
    {
        return await _wellProjectTimeSeriesService.UpdateGasProducerCostProfileOverride(caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPut("{wellProjectId}/water-injector-cost-profile-override/{costProfileId}")]
    public async Task<WaterInjectorCostProfileOverrideDto> UpdateWaterInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateWaterInjectorCostProfileOverrideDto dto)
    {
        return await _wellProjectTimeSeriesService.UpdateWaterInjectorCostProfileOverride(caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPut("{wellProjectId}/gas-injector-cost-profile-override/{costProfileId}")]
    public async Task<GasInjectorCostProfileOverrideDto> UpdateGasInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateGasInjectorCostProfileOverrideDto dto)
    {
        return await _wellProjectTimeSeriesService.UpdateGasInjectorCostProfileOverride(caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPost("{wellProjectId}/oil-producer-cost-profile-override")]
    public async Task<OilProducerCostProfileOverrideDto> CreateOilProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateOilProducerCostProfileOverrideDto dto)
    {
        return await _wellProjectTimeSeriesService.CreateOilProducerCostProfileOverride(caseId, wellProjectId, dto);
    }

    [HttpPost("{wellProjectId}/gas-producer-cost-profile-override")]
    public async Task<GasProducerCostProfileOverrideDto> CreateGasProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateGasProducerCostProfileOverrideDto dto)
    {
        return await _wellProjectTimeSeriesService.CreateGasProducerCostProfileOverride(caseId, wellProjectId, dto);
    }

    [HttpPost("{wellProjectId}/water-injector-cost-profile-override")]
    public async Task<WaterInjectorCostProfileOverrideDto> CreateWaterInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateWaterInjectorCostProfileOverrideDto dto)
    {
        return await _wellProjectTimeSeriesService.CreateWaterInjectorCostProfileOverride(caseId, wellProjectId, dto);
    }

    [HttpPost("{wellProjectId}/gas-injector-cost-profile-override")]
    public async Task<GasInjectorCostProfileOverrideDto> CreateGasInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateGasInjectorCostProfileOverrideDto dto)
    {
        return await _wellProjectTimeSeriesService.CreateGasInjectorCostProfileOverride(caseId, wellProjectId, dto);
    }
}
