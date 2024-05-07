using api.Dtos;
using api.Services;

using api.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/substructures")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class SubstructuresController : ControllerBase
{
    private readonly ISubstructureService _substructureService;

    public SubstructuresController(
        ISubstructureService substructureService
    )
    {
        _substructureService = substructureService;
    }

    [HttpPut("{substructureId}")]
    public async Task<SubstructureDto> UpdateSubstructure(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromBody] APIUpdateSubstructureDto dto)
    {
        return await _substructureService.UpdateSubstructure(caseId, substructureId, dto);
    }

    [HttpPut("{substructureId}/cost-profile-override/{costProfileId}")]
    public async Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateSubstructureCostProfileOverrideDto dto)
    {
        return await _substructureService.UpdateSubstructureCostProfileOverride(caseId, substructureId, costProfileId, dto);
    }


}
