using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Cases.Create;

public class CreateCaseController(CreateCaseService createCaseService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/cases")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ProjectDataDto> CreateCase(Guid projectId, [FromBody] CreateCaseDto caseDto)
    {
        CreateCaseDtoValidator.Validate(caseDto);

        await createCaseService.CreateCase(projectId, caseDto);

        return await getProjectDataService.GetProjectData(projectId);
    }
}
