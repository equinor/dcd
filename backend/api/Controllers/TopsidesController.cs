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
public class TopsidesController : ControllerBase
{
    private readonly ITopsideService _topsideService;

    public TopsidesController(ITopsideService topsideService)
    {
        _topsideService = topsideService;
    }

    [HttpPost(Name = "CreateTopside")]
    public async Task<ProjectDto> CreateTopside([FromQuery] Guid sourceCaseId, [FromBody] TopsideDto topsideDto)
    {
        return await _topsideService.CreateTopside(topsideDto, sourceCaseId);
    }

    [HttpDelete("{topsideId}", Name = "DeleteTopside")]
    public async Task<ProjectDto> DeleteTopside(Guid topsideId)
    {
        return await _topsideService.DeleteTopside(topsideId);
    }

    [HttpPut(Name = "UpdateTopside")]
    public async Task<ProjectDto> UpdateTopside([FromBody] TopsideDto topsideDto)
    {
        return await _topsideService.UpdateTopside(topsideDto);
    }

    [HttpPost("{topsideId}/copy", Name = "CopyTopside")]
    public async Task<TopsideDto> CopyTopside([FromQuery] Guid caseId, Guid topsideId)
    {
        return await _topsideService.CopyTopside(topsideId, caseId);
    }

    [HttpPut("new", Name = "NewUpdateTopside")]
    public async Task<TopsideDto> NewUpdateTopside([FromBody] TopsideDto topsideDto)
    {
        return await _topsideService.NewUpdateTopside(topsideDto);
    }
}
