using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Cases.Delete;

public class DeleteCaseController(DeleteCaseService deleteCaseService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpDelete("projects/{projectId:guid}/cases/{caseId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    [DisableLazyLoading]
    public async Task<ProjectDataDto> DeleteCase([FromRoute] Guid projectId, [FromRoute] Guid caseId)
    {
        await deleteCaseService.DeleteCase(projectId, caseId);

        return await getProjectDataService.GetProjectData(projectId);
    }
}
