using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects/{projectId}/cases/{caseId}/substructures")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.User
)]
[ActionType(ActionType.Edit)]
public class SubstructuresController : ControllerBase
{
    private readonly ISubstructureService _substructureService;
    private readonly ISubstructureTimeSeriesService _substructureTimeSeriesService;

    public SubstructuresController(
        ISubstructureService substructureService,
        ISubstructureTimeSeriesService substructureTimeSeriesService
    )
    {
        _substructureService = substructureService;
        _substructureTimeSeriesService = substructureTimeSeriesService;
    }

    [HttpPut("{substructureId}")]
    public async Task<SubstructureDto> UpdateSubstructure(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromBody] APIUpdateSubstructureDto dto)
    {
        return await _substructureService.UpdateSubstructure(projectId, caseId, substructureId, dto);
    }

    [HttpPost("{substructureId}/cost-profile-override")]
    public async Task<SubstructureCostProfileOverrideDto> CreateSubstructureCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromBody] CreateSubstructureCostProfileOverrideDto dto)
    {
        return await _substructureTimeSeriesService.CreateSubstructureCostProfileOverride(projectId, caseId, substructureId, dto);
    }

    [HttpPut("{substructureId}/cost-profile-override/{costProfileId}")]
    public async Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateSubstructureCostProfileOverrideDto dto)
    {
        return await _substructureTimeSeriesService.UpdateSubstructureCostProfileOverride(projectId, caseId, substructureId, costProfileId, dto);
    }
}
