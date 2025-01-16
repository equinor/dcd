using api.AppInfrastructure.ControllerAttributes;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Features.Cases.GetWithAssets;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.WellProjects;

public class UpdateWellProjectController(WellProjectService wellProjectService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}")]
    public async Task<WellProjectDto> UpdateWellProject(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] UpdateWellProjectDto dto)
    {
        return await wellProjectService.UpdateWellProject(projectId, caseId, wellProjectId, dto);
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
        return await wellProjectService.UpdateWellProjectWellDrillingSchedule(projectId, caseId, wellProjectId, wellId, drillingScheduleId, dto);
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
        return await wellProjectService.CreateWellProjectWellDrillingSchedule(projectId, caseId, wellProjectId, wellId, dto);
    }
}
