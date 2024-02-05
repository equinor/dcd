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
public class SubstructuresController : ControllerBase
{
    private readonly ISubstructureService _substructureService;

    public SubstructuresController(ISubstructureService substructureService)
    {
        _substructureService = substructureService;
    }

    [HttpPost(Name = "CreateSubstructure")]
    public async Task<ProjectDto> CreateSubstructure([FromQuery] Guid sourceCaseId, [FromBody] SubstructureDto substructureDto)
    {
        var substructure = SubstructureAdapter.Convert(substructureDto);
        return await _substructureService.CreateSubstructure(substructure, sourceCaseId);
    }

    [HttpDelete("{substructureId}", Name = "DeleteSubstructure")]
    public async Task<ProjectDto> DeleteSubstructure(Guid substructureId)
    {
        return await _substructureService.DeleteSubstructure(substructureId);
    }

    [HttpPut(Name = "UpdateSubstructure")]
    public async Task<ProjectDto> UpdateSubstructure([FromBody] SubstructureDto substructureDto)
    {
        return await _substructureService.UpdateSubstructure(substructureDto);
    }

    [HttpPost("{substructureId}/copy", Name = "CopySubstructure")]
    public async Task<SubstructureDto> CopySubstructure([FromQuery] Guid caseId, Guid substructureId)
    {
        return await _substructureService.CopySubstructure(substructureId, caseId);
    }

    [HttpPut("new", Name = "NewUpdateSubstructure")]
    public async Task<SubstructureDto> NewUpdateSubstructure([FromBody] SubstructureDto substructureDto)
    {
        return await _substructureService.NewUpdateSubstructure(substructureDto);
    }
}
