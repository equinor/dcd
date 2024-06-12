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

    [HttpPost("{drainageStrategyId}/production-profile-oil/")]
    public async Task<ProductionProfileOilDto> CreateProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateProductionProfileOilDto dto)
    {
        return await _drainageStrategyService.CreateProductionProfileOil(projectId, caseId, drainageStrategyId, dto);
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

    [HttpPost("{drainageStrategyId}/production-profile-gas/")]
    public async Task<ProductionProfileGasDto> CreateProductionProfileGas(
    [FromRoute] Guid projectId,
    [FromRoute] Guid caseId,
    [FromRoute] Guid drainageStrategyId,
    [FromBody] CreateProductionProfileGasDto dto)
    {
        return await _drainageStrategyService.CreateProductionProfileGas(projectId, caseId, drainageStrategyId, dto);
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

    [HttpPost("{drainageStrategyId}/production-profile-water/")]
    public async Task<ProductionProfileWaterDto> CreateProductionProfileWater(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateProductionProfileWaterDto dto)
    {
        return await _drainageStrategyService.CreateProductionProfileWater(projectId, caseId, drainageStrategyId, dto);
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

    [HttpPost("{drainageStrategyId}/production-profile-water-injection/")]
    public async Task<ProductionProfileWaterInjectionDto> CreateProductionProfileWaterInjection(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateProductionProfileWaterInjectionDto dto)
    {
        return await _drainageStrategyService.CreateProductionProfileWaterInjection(projectId, caseId, drainageStrategyId, dto);
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

    [HttpPost("{drainageStrategyId}/fuel-flaring-and-losses-override/")]
    public async Task<FuelFlaringAndLossesOverrideDto> CreateFuelFlaringAndLossesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateFuelFlaringAndLossesOverrideDto dto)
    {
        return await _drainageStrategyService.CreateFuelFlaringAndLossesOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/fuel-flaring-and-losses-override/{profileId}")]
    public async Task<FuelFlaringAndLossesOverrideDto> UpdateFuelFlaringAndLossesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateFuelFlaringAndLossesOverrideDto dto)
    {
        return await _drainageStrategyService.UpdateFuelFlaringAndLossesOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/net-sales-gas-override/")]
    public async Task<NetSalesGasOverrideDto> CreateNetSalesGasOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateNetSalesGasOverrideDto dto)
    {
        return await _drainageStrategyService.CreateNetSalesGasOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/net-sales-gas-override/{profileId}")]
    public async Task<NetSalesGasOverrideDto> UpdateNetSalesGasOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateNetSalesGasOverrideDto dto)
    {
        return await _drainageStrategyService.UpdateNetSalesGasOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/co2-emissions-override/")]
    public async Task<Co2EmissionsOverrideDto> CreateCo2EmissionsOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateCo2EmissionsOverrideDto dto)
    {
        return await _drainageStrategyService.CreateCo2EmissionsOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/co2-emissions-override/{profileId}")]
    public async Task<Co2EmissionsOverrideDto> UpdateCo2EmissionsOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateCo2EmissionsOverrideDto dto)
    {
        return await _drainageStrategyService.UpdateCo2EmissionsOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/imported-electricity-override/")]
    public async Task<ImportedElectricityOverrideDto> CreateImportedElectricityOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateImportedElectricityOverrideDto dto)
    {
        return await _drainageStrategyService.CreateImportedElectricityOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/imported-electricity-override/{profileId}")]
    public async Task<ImportedElectricityOverrideDto> UpdateImportedElectricityOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateImportedElectricityOverrideDto dto)
    {
        return await _drainageStrategyService.UpdateImportedElectricityOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/deferred-oil-production/")]
    public async Task<DeferredOilProductionDto> CreateDeferredOilProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateDeferredOilProductionDto dto)
    {
        return await _drainageStrategyService.CreateDeferredOilProduction(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/deferred-oil-production/{profileId}")]
    public async Task<DeferredOilProductionDto> UpdateDeferredOilProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateDeferredOilProductionDto dto)
    {
        return await _drainageStrategyService.UpdateDeferredOilProduction(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/deferred-gas-production/")]
    public async Task<DeferredGasProductionDto> CreateDeferredGasProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateDeferredGasProductionDto dto)
    {
        return await _drainageStrategyService.CreateDeferredGasProduction(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/deferred-gas-production/{profileId}")]
    public async Task<DeferredGasProductionDto> UpdateDeferredGasProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateDeferredGasProductionDto dto)
    {
        return await _drainageStrategyService.UpdateDeferredGasProduction(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
