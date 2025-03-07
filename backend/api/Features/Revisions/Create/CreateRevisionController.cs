using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Revisions.Create;

public class CreateRevisionController(CreateRevisionService createRevisionService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/revisions")]
    [AuthorizeActionType(ActionType.CreateRevision)]
    public async Task<RevisionDataDto> CreateRevision(Guid projectId, [FromBody] CreateRevisionDto createRevisionDto)
    {
        CreateRevisionDtoValidator.Validate(createRevisionDto);

        var revisionId = await createRevisionService.CreateRevision(projectId, createRevisionDto);

        return await getProjectDataService.GetRevisionData(projectId, revisionId);
    }
}
