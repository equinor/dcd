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

    public WellProjectsController(
        IWellProjectService wellProjectService
    )
    {
        _wellProjectService = wellProjectService;
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

    [HttpPut("{wellProjectId}/oil-producer-cost-profile-override/{costProfileId}")]
    public async Task<OilProducerCostProfileOverrideDto> UpdateOilProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOilProducerCostProfileOverrideDto dto)
    {
        return await _wellProjectService.UpdateOilProducerCostProfileOverride(caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPut("{wellProjectId}/gas-producer-cost-profile-override/{costProfileId}")]
    public async Task<GasProducerCostProfileOverrideDto> UpdateGasProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateGasProducerCostProfileOverrideDto dto)
    {
        return await _wellProjectService.UpdateGasProducerCostProfileOverride(caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPut("{wellProjectId}/water-injector-cost-profile-override/{costProfileId}")]
    public async Task<WaterInjectorCostProfileOverrideDto> UpdateWaterInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateWaterInjectorCostProfileOverrideDto dto)
    {
        return await _wellProjectService.UpdateWaterInjectorCostProfileOverride(caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPut("{wellProjectId}/gas-injector-cost-profile-override/{costProfileId}")]
    public async Task<GasInjectorCostProfileOverrideDto> UpdateGasInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateGasInjectorCostProfileOverrideDto dto)
    {
        return await _wellProjectService.UpdateGasInjectorCostProfileOverride(caseId, wellProjectId, costProfileId, dto);
    }

    [HttpPut("{wellProjectId}/well/{wellId}")]
    public async Task<WellProjectWellDto> UpdateWellProjectWell(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid wellId,
        [FromBody] UpdateWellProjectWellDto dto)
    {
        return await _wellProjectService.UpdateWellProjectWell(caseId, wellProjectId, wellId, dto);
    }

    [HttpPost("{wellProjectId}/oil-producer-cost-profile-override")]
    public async Task<OilProducerCostProfileOverrideDto> CreateOilProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateOilProducerCostProfileOverrideDto dto)
    {
        return await _wellProjectService.CreateOilProducerCostProfileOverride(caseId, wellProjectId, dto);
    }

    [HttpPost("{wellProjectId}/gas-producer-cost-profile-override")]
    public async Task<GasProducerCostProfileOverrideDto> CreateGasProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateGasProducerCostProfileOverrideDto dto)
    {
        return await _wellProjectService.CreateGasProducerCostProfileOverride(caseId, wellProjectId, dto);
    }

    [HttpPost("{wellProjectId}/water-injector-cost-profile-override")]
    public async Task<WaterInjectorCostProfileOverrideDto> CreateWaterInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateWaterInjectorCostProfileOverrideDto dto)
    {
        return await _wellProjectService.CreateWaterInjectorCostProfileOverride(caseId, wellProjectId, dto);
    }

    [HttpPost("{wellProjectId}/gas-injector-cost-profile-override")]
    public async Task<GasInjectorCostProfileOverrideDto> CreateGasInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateGasInjectorCostProfileOverrideDto dto)
    {
        return await _wellProjectService.CreateGasInjectorCostProfileOverride(caseId, wellProjectId, dto);
    }
}
