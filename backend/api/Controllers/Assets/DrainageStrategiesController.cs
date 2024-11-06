using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects/{projectId}/cases/{caseId}/drainage-strategies")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.User
)]
[ActionType(ActionType.Edit)]
public class DrainageStrategiesController(
    IDrainageStrategyService drainageStrategyService,
    IDrainageStrategyTimeSeriesService drainageStrategyTimeSeriesService)
    : ControllerBase
{
    [HttpPut("{drainageStrategyId}")]
    public async Task<DrainageStrategyDto> UpdateDrainageStrategy(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] UpdateDrainageStrategyDto dto)
    {
        return await drainageStrategyService.UpdateDrainageStrategy(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPost("{drainageStrategyId}/production-profile-oil/")]
    public async Task<ProductionProfileOilDto> CreateProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateProductionProfileOilDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateProductionProfileOil(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/production-profile-oil/{profileId}")]
    public async Task<ProductionProfileOilDto> UpdateProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateProductionProfileOilDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateProductionProfileOil(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/additional-production-profile-oil/")]
    public async Task<AdditionalProductionProfileOilDto> CreateAdditionalProductionProfileOil(
    [FromRoute] Guid projectId,
    [FromRoute] Guid caseId,
    [FromRoute] Guid drainageStrategyId,
    [FromBody] CreateAdditionalProductionProfileOilDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateAdditionalProductionProfileOil(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/additional-production-profile-oil/{profileId}")]
    public async Task<AdditionalProductionProfileOilDto> UpdateAdditionalProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateAdditionalProductionProfileOilDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateAdditionalProductionProfileOil(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/production-profile-gas/")]
    public async Task<ProductionProfileGasDto> CreateProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateProductionProfileGasDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateProductionProfileGas(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/production-profile-gas/{profileId}")]
    public async Task<ProductionProfileGasDto> UpdateProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateProductionProfileGasDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateProductionProfileGas(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/additional-production-profile-gas/")]
    public async Task<AdditionalProductionProfileGasDto> CreateAdditionalProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateAdditionalProductionProfileGasDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateAdditionalProductionProfileGas(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/additional-production-profile-gas/{profileId}")]
    public async Task<AdditionalProductionProfileGasDto> UpdateAdditionalProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateAdditionalProductionProfileGasDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateAdditionalProductionProfileGas(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/production-profile-water/")]
    public async Task<ProductionProfileWaterDto> CreateProductionProfileWater(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateProductionProfileWaterDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateProductionProfileWater(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/production-profile-water/{profileId}")]
    public async Task<ProductionProfileWaterDto> UpdateProductionProfileWater(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateProductionProfileWaterDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateProductionProfileWater(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/production-profile-water-injection/")]
    public async Task<ProductionProfileWaterInjectionDto> CreateProductionProfileWaterInjection(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateProductionProfileWaterInjectionDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateProductionProfileWaterInjection(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/production-profile-water-injection/{profileId}")]
    public async Task<ProductionProfileWaterInjectionDto> UpdateProductionProfileWaterInjection(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateProductionProfileWaterInjectionDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateProductionProfileWaterInjection(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/fuel-flaring-and-losses-override/")]
    public async Task<FuelFlaringAndLossesOverrideDto> CreateFuelFlaringAndLossesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateFuelFlaringAndLossesOverrideDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateFuelFlaringAndLossesOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/fuel-flaring-and-losses-override/{profileId}")]
    public async Task<FuelFlaringAndLossesOverrideDto> UpdateFuelFlaringAndLossesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateFuelFlaringAndLossesOverrideDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateFuelFlaringAndLossesOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/net-sales-gas-override/")]
    public async Task<NetSalesGasOverrideDto> CreateNetSalesGasOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateNetSalesGasOverrideDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateNetSalesGasOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/net-sales-gas-override/{profileId}")]
    public async Task<NetSalesGasOverrideDto> UpdateNetSalesGasOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateNetSalesGasOverrideDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateNetSalesGasOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/co2-emissions-override/")]
    public async Task<Co2EmissionsOverrideDto> CreateCo2EmissionsOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateCo2EmissionsOverrideDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateCo2EmissionsOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/co2-emissions-override/{profileId}")]
    public async Task<Co2EmissionsOverrideDto> UpdateCo2EmissionsOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateCo2EmissionsOverrideDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateCo2EmissionsOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/imported-electricity-override/")]
    public async Task<ImportedElectricityOverrideDto> CreateImportedElectricityOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateImportedElectricityOverrideDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateImportedElectricityOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/imported-electricity-override/{profileId}")]
    public async Task<ImportedElectricityOverrideDto> UpdateImportedElectricityOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateImportedElectricityOverrideDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateImportedElectricityOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/deferred-oil-production/")]
    public async Task<DeferredOilProductionDto> CreateDeferredOilProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateDeferredOilProductionDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateDeferredOilProduction(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/deferred-oil-production/{profileId}")]
    public async Task<DeferredOilProductionDto> UpdateDeferredOilProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateDeferredOilProductionDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateDeferredOilProduction(projectId, caseId, drainageStrategyId, profileId, dto);
    }

    [HttpPost("{drainageStrategyId}/deferred-gas-production/")]
    public async Task<DeferredGasProductionDto> CreateDeferredGasProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateDeferredGasProductionDto dto)
    {
        return await drainageStrategyTimeSeriesService.CreateDeferredGasProduction(projectId, caseId, drainageStrategyId, dto);
    }

    [HttpPut("{drainageStrategyId}/deferred-gas-production/{profileId}")]
    public async Task<DeferredGasProductionDto> UpdateDeferredGasProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateDeferredGasProductionDto dto)
    {
        return await drainageStrategyTimeSeriesService.UpdateDeferredGasProduction(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
