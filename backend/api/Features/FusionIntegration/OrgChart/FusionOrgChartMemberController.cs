using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.FusionIntegration.OrgChart.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.FusionIntegration.OrgChart;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class FusionOrgChartMemberController(FusionOrgChartMemberService fusionPeopleService) : ControllerBase
{

    [HttpGet("projects/{projectId:guid}/members/context/{contextId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<List<FusionPersonV1>> GetOrgChartMembers([FromRoute] Guid projectId, [FromRoute] Guid contextId)
    {
        return await fusionPeopleService.GetAllPersonsOnProject(contextId, 100, 0);
    }
}
