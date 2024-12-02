using api.AppInfrastructure.Authorization;
using api.Dtos;
using api.Features.TechnicalInput.Dtos;
using api.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects/{projectId}/cases/{caseId}/explorations")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.User
)]
[ActionType(ActionType.Edit)]
public class ExplorationsController(
    IExplorationService explorationService,
    IExplorationTimeSeriesService explorationTimeSeriesService)
    : ControllerBase
{
    [HttpPut("{explorationId}")]
    public async Task<ExplorationDto> UpdateExploration(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] UpdateExplorationDto dto)
    {
        return await explorationService.UpdateExploration(projectId, caseId, explorationId, dto);
    }

    [HttpPut("{explorationId}/wells/{wellId}/drilling-schedule/{drillingScheduleId}")]
    public async Task<DrillingScheduleDto> UpdateExplorationWellDrillingSchedule(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid wellId,
        [FromRoute] Guid drillingScheduleId,
        [FromBody] UpdateDrillingScheduleDto dto)
    {
        return await explorationService.UpdateExplorationWellDrillingSchedule(projectId, caseId, explorationId, wellId, drillingScheduleId, dto);
    }

    [HttpPost("{explorationId}/wells/{wellId}/drilling-schedule")]
    public async Task<DrillingScheduleDto> CreateExplorationWellDrillingSchedule(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid wellId,
        [FromBody] CreateDrillingScheduleDto dto)
    {
        return await explorationService.CreateExplorationWellDrillingSchedule(projectId, caseId, explorationId, wellId, dto);
    }



    [HttpPut("{explorationId}/g-and-g-and-admin-cost-override/{costProfileId}")]
    public async Task<GAndGAdminCostOverrideDto> UpdateGAndGAdminCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateGAndGAdminCostOverrideDto dto)
    {
        return await explorationTimeSeriesService.UpdateGAndGAdminCostOverride(projectId, caseId, explorationId, costProfileId, dto);
    }

    [HttpPost("{explorationId}/g-and-g-and-admin-cost-override")]
    public async Task<GAndGAdminCostOverrideDto> CreateGAndGAdminCostOverride(
    [FromRoute] Guid projectId,
    [FromRoute] Guid caseId,
    [FromRoute] Guid explorationId,
    [FromBody] CreateGAndGAdminCostOverrideDto dto)
    {
        return await explorationTimeSeriesService.CreateGAndGAdminCostOverride(projectId, caseId, explorationId, dto);
    }

    [HttpPut("{explorationId}/seismic-acquisition-and-processing/{costProfileId}")]
    public async Task<SeismicAcquisitionAndProcessingDto> UpdateSeismicAcquisitionAndProcessing(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateSeismicAcquisitionAndProcessingDto dto)
    {
        return await explorationTimeSeriesService.UpdateSeismicAcquisitionAndProcessing(projectId, caseId, explorationId, costProfileId, dto);
    }

    [HttpPost("{explorationId}/seismic-acquisition-and-processing")]
    public async Task<SeismicAcquisitionAndProcessingDto> CreateSeismicAcquisitionAndProcessing(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] CreateSeismicAcquisitionAndProcessingDto dto)
    {
        return await explorationTimeSeriesService.CreateSeismicAcquisitionAndProcessing(projectId, caseId, explorationId, dto);
    }

    [HttpPut("{explorationId}/country-office-cost/{costProfileId}")]
    public async Task<CountryOfficeCostDto> UpdateCountryOfficeCost(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateCountryOfficeCostDto dto)
    {
        return await explorationTimeSeriesService.UpdateCountryOfficeCost(projectId, caseId, explorationId, costProfileId, dto);
    }

    [HttpPost("{explorationId}/country-office-cost")]
    public async Task<CountryOfficeCostDto> CreateCountryOfficeCost(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] CreateCountryOfficeCostDto dto)
    {
        return await explorationTimeSeriesService.CreateCountryOfficeCost(projectId, caseId, explorationId, dto);
    }
}
