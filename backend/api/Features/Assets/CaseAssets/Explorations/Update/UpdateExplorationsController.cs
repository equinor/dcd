using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.Explorations.Profiles.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Update.Dtos;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Explorations.Update;

public class UpdateExplorationsController(UpdateExplorationService updateExplorationService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId}/cases/{caseId}/explorations/{explorationId}")]
    public async Task<ExplorationDto> UpdateExploration(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] UpdateExplorationDto dto)
    {
        return await updateExplorationService.UpdateExploration(projectId, caseId, explorationId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId}/cases/{caseId}/explorations/{explorationId}/wells/{wellId}/drilling-schedule/{drillingScheduleId}")]
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
    [HttpPost("projects/{projectId}/cases/{caseId}/explorations/{explorationId}/wells/{wellId}/drilling-schedule")]
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
