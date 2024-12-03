using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.CaseProfiles.Dtos;
using api.Features.FeatureToggles;
using api.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Cases.Update;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateCaseController(UpdateCaseService updateCaseService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<CaseDto> UpdateCase([FromRoute] Guid projectId, [FromRoute] Guid caseId, [FromBody] UpdateCaseDto caseDto)
    {
        if (FeatureToggleService.RevisionEnabled)
        {
            UpdateCaseDtoValidator.Validate(caseDto);
        }

        return await updateCaseService.UpdateCase(projectId, caseId, caseDto);
    }
}
