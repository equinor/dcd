using api.Dtos;
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
public class CasesController : ControllerBase
{
    private readonly ICaseService _caseService;
    private readonly IDuplicateCaseService _duplicateCaseService;

    public CasesController(ICaseService caseService, IDuplicateCaseService duplicateCaseService)
    {
        _caseService = caseService;
        _duplicateCaseService = duplicateCaseService;
    }

    [HttpPost(Name = "CreateCase")]
    public async Task<ProjectDto> CreateCase([FromBody] CaseDto caseDto)
    {
        return await _caseService.CreateCase(caseDto);
    }

    [HttpPost("new", Name = "NewCreateCase")]
    public async Task<ProjectDto> NewCreateCase([FromBody] CaseDto caseDto)
    {
        return await _caseService.NewCreateCase(caseDto);
    }

    [HttpPost("copy", Name = "Duplicate")]
    public async Task<ProjectDto> DuplicateCase([FromQuery] Guid copyCaseId)
    {
        return await _duplicateCaseService.DuplicateCase(copyCaseId);
    }

    [HttpPut(Name = "UpdateCase")]
    public async Task<ProjectDto> UpdateCase([FromBody] CaseDto caseDto)
    {
        return await _caseService.UpdateCase(caseDto);
    }

    [HttpPut("new", Name = "NewUpdateCase")]
    public async Task<CaseDto> NewUpdateCase([FromBody] CaseDto caseDto)
    {
        return await _caseService.NewUpdateCase(caseDto);
    }

    [HttpDelete("{caseId}", Name = "DeleteCase")]
    public async Task<ProjectDto> DeleteTransport(Guid caseId)
    {
        return await _caseService.DeleteCase(caseId);
    }
}
