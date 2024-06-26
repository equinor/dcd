using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/explorations")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class ExplorationsController : ControllerBase
{
    private readonly IExplorationService _explorationService;
    private readonly IExplorationTimeSeriesService _explorationTimeSeriesService;

    public ExplorationsController(
        IExplorationService explorationService,
        IExplorationTimeSeriesService explorationTimeSeriesService
    )
    {
        _explorationService = explorationService;
        _explorationTimeSeriesService = explorationTimeSeriesService;
    }

    [HttpPut("{explorationId}")]
    public async Task<ExplorationDto> UpdateExploration(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] UpdateExplorationDto dto)
    {
        return await _explorationService.UpdateExploration(caseId, explorationId, dto);
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
        return await _explorationService.UpdateExplorationWellDrillingSchedule(caseId, explorationId, wellId, drillingScheduleId, dto);
    }

    [HttpPost("{explorationId}/wells/{wellId}/drilling-schedule")]
    public async Task<DrillingScheduleDto> CreateExplorationWellDrillingSchedule(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid wellId,
        [FromBody] CreateDrillingScheduleDto dto)
    {
        return await _explorationService.CreateExplorationWellDrillingSchedule(caseId, explorationId, wellId, dto);
    }

    [HttpPut("{explorationId}/seismic-acquisition-and-processing/{costProfileId}")]
    public async Task<SeismicAcquisitionAndProcessingDto> UpdateSeismicAcquisitionAndProcessing(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateSeismicAcquisitionAndProcessingDto dto)
    {
        return await _explorationTimeSeriesService.UpdateSeismicAcquisitionAndProcessing(caseId, explorationId, costProfileId, dto);
    }

    [HttpPut("{explorationId}/g-and-g-and-admin-cost-override/{costProfileId}")]
    public async Task<GAndGAdminCostOverrideDto> UpdateGAndGAdminCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateGAndGAdminCostOverrideDto dto)
    {
        return await _explorationTimeSeriesService.UpdateGAndGAdminCostOverride(caseId, explorationId, costProfileId, dto);
    }

    [HttpPost("{explorationId}/g-and-g-and-admin-cost-override")]
    public async Task<GAndGAdminCostOverrideDto> CreateGAndGAdminCostOverride(
    [FromRoute] Guid projectId,
    [FromRoute] Guid caseId,
    [FromRoute] Guid explorationId,
    [FromBody] CreateGAndGAdminCostOverrideDto dto)
    {
        return await _explorationTimeSeriesService.CreateGAndGAdminCostOverride(caseId, explorationId, dto);
    }

    [HttpPost("{explorationId}/seismic-acquisition-and-processing")]
    public async Task<SeismicAcquisitionAndProcessingDto> CreateSeismicAcquisitionAndProcessing(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] CreateSeismicAcquisitionAndProcessingDto dto)
    {
        return await _explorationTimeSeriesService.CreateSeismicAcquisitionAndProcessing(caseId, explorationId, dto);
    }

    [HttpPut("{explorationId}/country-office-cost/{costProfileId}")]
    public async Task<CountryOfficeCostDto> UpdateCountryOfficeCost(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateCountryOfficeCostDto dto)
    {
        return await _explorationTimeSeriesService.UpdateCountryOfficeCost(caseId, explorationId, costProfileId, dto);
    }

    [HttpPost("{explorationId}/country-office-cost")]
    public async Task<CountryOfficeCostDto> CreateCountryOfficeCost(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] CreateCountryOfficeCostDto dto)
    {
        return await _explorationTimeSeriesService.CreateCountryOfficeCost(caseId, explorationId, dto);
    }
}

