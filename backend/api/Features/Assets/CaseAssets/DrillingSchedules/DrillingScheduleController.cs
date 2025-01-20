using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.DrillingSchedules.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.DrillingSchedules;

public class DrillingScheduleController(DrillingScheduleService drillingScheduleService) : ControllerBase
{
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
        return await drillingScheduleService.UpdateExplorationWellDrillingSchedule(projectId, caseId, explorationId, wellId, drillingScheduleId, dto);
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
        return await drillingScheduleService.CreateExplorationWellDrillingSchedule(projectId, caseId, explorationId, wellId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}/wells/{wellId:guid}/drilling-schedule/{drillingScheduleId:guid}")]
    public async Task<DrillingScheduleDto> UpdateWellProjectWellDrillingSchedule(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid wellId,
        [FromRoute] Guid drillingScheduleId,
        [FromBody] UpdateDrillingScheduleDto dto)
    {
        return await drillingScheduleService.UpdateWellProjectWellDrillingSchedule(projectId, caseId, wellProjectId, wellId, drillingScheduleId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}/wells/{wellId:guid}/drilling-schedule")]
    public async Task<DrillingScheduleDto> CreateWellProjectWellDrillingSchedule(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid wellId,
        [FromBody] CreateDrillingScheduleDto dto)
    {
        return await drillingScheduleService.CreateWellProjectWellDrillingSchedule(projectId, caseId, wellProjectId, wellId, dto);
    }
}
