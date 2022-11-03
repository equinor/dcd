using api.Adapters;
using api.Dtos;
using api.Models;
using api.Services;

using Api.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User

    )]
public class WellProjectsController : ControllerBase
{
    private readonly WellProjectService _wellProjectService;

    public WellProjectsController(WellProjectService wellProjectService)
    {
        _wellProjectService = wellProjectService;
    }

    [HttpPost(Name = "CreateWellProject")]
    public ProjectDto CreateWellProject([FromQuery] Guid sourceCaseId, [FromBody] WellProjectDto wellProjectDto)
    {
        var wellProject = WellProjectAdapter.Convert(wellProjectDto);
        return _wellProjectService.CreateWellProject(wellProject, sourceCaseId);
    }

    [HttpDelete("{wellProjectId}", Name = "DeleteWellProject")]
    public ProjectDto DeleteWellProject(Guid wellProjectId)
    {
        return _wellProjectService.DeleteWellProject(wellProjectId);
    }

    [HttpPut(Name = "UpdateWellProject")]
    public ProjectDto UpdateWellProject([FromBody] WellProjectDto wellProjectDto)
    {
        return _wellProjectService.UpdateWellProject(wellProjectDto);
    }

    [HttpPut("new", Name = "NewUpdateWellProject")]
    public WellProjectDto NewUpdateWellProject([FromBody] WellProjectDto wellProjectDto)
    {
        return _wellProjectService.NewUpdateWellProject(wellProjectDto);
    }

    [HttpPost("{wellProjectId}/copy", Name = "CopyWellProject")]
    public WellProjectDto CopyWellProject([FromQuery] Guid caseId, Guid wellProjectId)
    {
        return _wellProjectService.CopyWellProject(wellProjectId, caseId);
    }
}
