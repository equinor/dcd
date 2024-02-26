using api.Dtos;
using api.Services;

using Api.Authorization;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class CasesController : ControllerBase
{
    private readonly ICaseService _caseService;
    private readonly IDuplicateCaseService _duplicateCaseService;

    public CasesController(ICaseService caseService, IDuplicateCaseService duplicateCaseService)
    {
        _caseService = caseService;
        _duplicateCaseService = duplicateCaseService;
    }

    [HttpPost]
    public async Task<ProjectDto> CreateCase([FromRoute] Guid projectId, [FromBody] CreateCaseDto caseDto)
    {
        return await _caseService.CreateCase(projectId, caseDto);
    }

    [HttpPost("copy", Name = "Duplicate")]
    public async Task<ProjectDto> DuplicateCase([FromQuery] Guid copyCaseId)
    {
        return await _duplicateCaseService.DuplicateCase(copyCaseId);
    }

    [HttpPut("{caseId}")]
    public async Task<ProjectDto> UpdateCase([FromRoute] Guid caseId, [FromBody] UpdateCaseDto caseDto)
    {
        return await _caseService.UpdateCase(caseId, caseDto);
    }

    [HttpDelete("{caseId}")]
    public async Task<ProjectDto> DeleteCase(Guid caseId)
    {
        return await _caseService.DeleteCase(caseId);
    }
}
