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
    private readonly SubstructureService _substructureService;

    public SubstructuresController(SubstructureService substructureService)
    {
        _substructureService = substructureService;
    }

    [HttpPost(Name = "CreateSubstructure")]
    public ProjectDto CreateSubstructure([FromQuery] Guid sourceCaseId, [FromBody] SubstructureDto substructureDto)
    {
        var substructure = SubstructureAdapter.Convert(substructureDto);
        return _substructureService.CreateSubstructure(substructure, sourceCaseId);
    }

    [HttpDelete("{substructureId}", Name = "DeleteSubstructure")]
    public ProjectDto DeleteSubstructure(Guid substructureId)
    {
        return _substructureService.DeleteSubstructure(substructureId);
    }

    [HttpPut(Name = "UpdateSubstructure")]
    public ProjectDto UpdateSubstructure([FromBody] SubstructureDto substructureDto)
    {
        return _substructureService.UpdateSubstructure(substructureDto);
    }

    [HttpPost("{substructureId}/copy", Name = "CopySubstructure")]
    public SubstructureDto CopySubstructure([FromQuery] Guid caseId, Guid substructureId)
    {
        return _substructureService.CopySubstructure(substructureId, caseId);
    }

    [HttpPut("new", Name = "NewUpdateSubstructure")]
    public SubstructureDto NewUpdateSubstructure([FromBody] SubstructureDto substructureDto)
    {
        return _substructureService.NewUpdateSubstructure(substructureDto);
    }
}
