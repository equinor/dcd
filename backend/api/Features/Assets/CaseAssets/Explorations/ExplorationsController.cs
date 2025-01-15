using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Services;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Explorations;

[Route("projects/{projectId}/cases/{caseId}/explorations")]
[AuthorizeActionType(ActionType.Edit)]
public class ExplorationsController(
    ExplorationService explorationService)
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
}
