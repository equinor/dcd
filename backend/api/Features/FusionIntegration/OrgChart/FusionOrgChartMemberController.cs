using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Features.FusionIntegration.OrgChart.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.FusionIntegration.OrgChart;

[ApiController]
[Route("projects/{projectId:guid}/members")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class FusionOrgChartMemberController(FusionOrgChartMemberService fusionPeopleService) : ControllerBase
{

    [HttpGet("/context/{contextId:guid}")]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [ActionType(ActionType.Edit)]
    public async Task<List<FusionPersonV1>> GetOrgChartMembers([FromRoute] Guid projectId, [FromRoute] Guid contextId)
    {
        return await fusionPeopleService.GetAllPersonsOnProject(contextId, 100, 0);
    }
}
