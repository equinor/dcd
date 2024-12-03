using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.CaseProfiles.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Wells.GetAffectedCases;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class GetCasesAffectedByWellController(GetAffectedCasesService getAffectedCasesService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/wells/{wellId:guid}/affected-cases")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<List<CaseDto>> GetAffectedCases([FromRoute] Guid projectId, [FromRoute] Guid wellId)
    {
        return await getAffectedCasesService.GetAffectedCases(projectId, wellId);
    }
}
