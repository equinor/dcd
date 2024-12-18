using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Images.Get;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class GetImageController(GetImageService getImageService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/cases/{caseId:guid}/images")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<List<ImageDto>> GetImages(Guid projectId, Guid caseId)
    {
        return await getImageService.GetCaseImages(projectId, caseId);
    }

    [HttpGet("projects/{projectId:guid}/images")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<List<ImageDto>> GetProjectImages(Guid projectId)
    {
        return await getImageService.GetProjectImages(projectId);
    }
}
