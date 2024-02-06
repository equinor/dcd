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
    private readonly IWellProjectService _wellProjectService;

    public WellProjectsController(IWellProjectService wellProjectService)
    {
        _wellProjectService = wellProjectService;
    }

    [HttpPost(Name = "CreateWellProject")]
    public async Task<ProjectDto> CreateWellProject([FromQuery] Guid sourceCaseId, [FromBody] WellProjectDto wellProjectDto)
    {
        var wellProject = WellProjectAdapter.Convert(wellProjectDto);
        return await _wellProjectService.CreateWellProject(wellProject, sourceCaseId);
    }

    [HttpDelete("{wellProjectId}", Name = "DeleteWellProject")]
    public async Task<ProjectDto> DeleteWellProject(Guid wellProjectId)
    {
        return await _wellProjectService.DeleteWellProject(wellProjectId);
    }

    [HttpPut(Name = "UpdateWellProject")]
    public async Task<ProjectDto> UpdateWellProject([FromBody] WellProjectDto wellProjectDto)
    {
        return await _wellProjectService.UpdateWellProject(wellProjectDto);
    }

    [HttpPut("new", Name = "NewUpdateWellProject")]
    public async Task<WellProjectDto> NewUpdateWellProject([FromBody] WellProjectDto wellProjectDto)
    {
        return await _wellProjectService.NewUpdateWellProject(wellProjectDto);
    }

    [HttpPost("{wellProjectId}/copy", Name = "CopyWellProject")]
    public async Task<WellProjectDto> CopyWellProject([FromQuery] Guid caseId, Guid wellProjectId)
    {
        return await _wellProjectService.CopyWellProject(wellProjectId, caseId);
    }
}
