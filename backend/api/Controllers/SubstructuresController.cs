using api.Adapters;
using api.Dtos;
using api.Models;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
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
}
