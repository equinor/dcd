using api.AppInfrastructure.ControllerAttributes;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Explorations;

public class UpdateExplorationController(UpdateExplorationService updateExplorationService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/explorations/{explorationId:guid}")]
    public async Task<NoContentResult> UpdateExploration(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] UpdateExplorationDto dto)
    {
        await updateExplorationService.UpdateExploration(projectId, caseId, explorationId, dto);
        return NoContent();
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/explorations/{explorationId:guid}/wells/{wellId:guid}/drilling-schedule/{drillingScheduleId:guid}")]
    public async Task<DrillingScheduleDto> UpdateExplorationWellDrillingSchedule(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid wellId,
        [FromRoute] Guid drillingScheduleId,
        [FromBody] UpdateDrillingScheduleDto dto)
    {
        return await updateExplorationService.UpdateExplorationWellDrillingSchedule(projectId, caseId, explorationId, wellId, drillingScheduleId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/explorations/{explorationId:guid}/wells/{wellId:guid}/drilling-schedule")]
    public async Task<DrillingScheduleDto> CreateExplorationWellDrillingSchedule(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid wellId,
        [FromBody] CreateDrillingScheduleDto dto)
    {
        return await updateExplorationService.CreateExplorationWellDrillingSchedule(projectId, caseId, explorationId, wellId, dto);
    }
}
