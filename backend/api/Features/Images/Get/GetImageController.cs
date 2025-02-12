using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Images.Get;

public class GetImageController(GetCaseImageService getCaseImageService, GetProjectImageService getProjectImageService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/cases/{caseId:guid}/images")]
    [AuthorizeActionType(ActionType.Read)]
    public async Task<List<ImageDto>> GetCaseImages(Guid projectId, Guid caseId)
    {
        return await getCaseImageService.GetImages(projectId, caseId);
    }

    [HttpGet("projects/{projectId:guid}/images")]
    [AuthorizeActionType(ActionType.Read)]
    public async Task<List<ImageDto>> GetProjectImages(Guid projectId)
    {
        return await getProjectImageService.GetImages(projectId);
    }
}
