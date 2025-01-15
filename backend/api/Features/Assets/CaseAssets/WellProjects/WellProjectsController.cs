using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Services;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.WellProjects;

[Route("projects/{projectId}/cases/{caseId}/well-projects")]
[AuthorizeActionType(ActionType.Edit)]
public class WellProjectsController(WellProjectService wellProjectService) : ControllerBase
{
    [HttpPut("{wellProjectId}")]
    public async Task<WellProjectDto> UpdateWellProject(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] UpdateWellProjectDto dto)
    {
        return await wellProjectService.UpdateWellProject(projectId, caseId, wellProjectId, dto);
    }

    [HttpPut("{wellProjectId}/wells/{wellId}/drilling-schedule/{drillingScheduleId}")]
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

    [HttpPost("{wellProjectId}/wells/{wellId}/drilling-schedule")]
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
