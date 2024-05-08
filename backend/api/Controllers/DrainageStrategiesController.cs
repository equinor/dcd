using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/drainage-strategies")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class DrainageStrategiesController : ControllerBase
{
    private readonly IDrainageStrategyService _drainageStrategyService;

    public DrainageStrategiesController(
        IDrainageStrategyService drainageStrategyService
    )
    {
        _drainageStrategyService = drainageStrategyService;
    }

    [HttpPut("{drainageStrategyId}")]
    public async Task<DrainageStrategyDto> UpdateDrainageStrategy(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] UpdateDrainageStrategyDto dto)
    {
        return await _drainageStrategyService.UpdateDrainageStrategy(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/production-profile-oil/{profileId}")]
    public async Task<ProductionProfileOilDto> UpdateProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateProductionProfileOilDto dto)
    {
        return await _drainageStrategyService.UpdateProductionProfileOil(projectId, caseId, drainageStrategyId, profileId, dto);
    }

        [HttpPut("{drainageStrategyId}/production-profile-gas/{profileId}")]
    public async Task<ProductionProfileGasDto> UpdateProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateProductionProfileGasDto dto)
    {
        return await _drainageStrategyService.UpdateProductionProfileGas(projectId, caseId, drainageStrategyId, profileId, dto);
    }


    [HttpPut("{drainageStrategyId}/production-profile-water/{profileId}")]
    public async Task<ProductionProfileWaterDto> UpdateProductionProfileWater(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateProductionProfileWaterDto dto)
    {
        return await _drainageStrategyService.UpdateProductionProfileWater(projectId, caseId, drainageStrategyId, profileId, dto);
    }


    [HttpPut("{drainageStrategyId}/production-profile-water-injection/{profileId}")]
    public async Task<ProductionProfileWaterInjectionDto> UpdateProductionProfileWaterInjection(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateProductionProfileWaterInjectionDto dto)
    {
        return await _drainageStrategyService.UpdateProductionProfileWaterInjection(projectId, caseId, drainageStrategyId, profileId, dto);
    }



}
