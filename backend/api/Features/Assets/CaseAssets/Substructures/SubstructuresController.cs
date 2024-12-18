using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Create;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Update;
using api.Features.Assets.CaseAssets.Substructures.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Assets.CaseAssets.Substructures;

[ApiController]
[Route("projects/{projectId}/cases/{caseId}/substructures")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.User
)]
[ActionType(ActionType.Edit)]
public class SubstructuresController(
    ISubstructureService substructureService,
    ISubstructureTimeSeriesService substructureTimeSeriesService)
    : ControllerBase
{
    [HttpPut("{substructureId}")]
    public async Task<SubstructureDto> UpdateSubstructure(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromBody] APIUpdateSubstructureDto dto)
    {
        return await substructureService.UpdateSubstructure(projectId, caseId, substructureId, dto);
    }

    [HttpPost("{substructureId}/cost-profile-override")]
    public async Task<SubstructureCostProfileOverrideDto> CreateSubstructureCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromBody] CreateSubstructureCostProfileOverrideDto dto)
    {
        return await substructureTimeSeriesService.CreateSubstructureCostProfileOverride(projectId, caseId, substructureId, dto);
    }

    [HttpPut("{substructureId}/cost-profile-override/{costProfileId}")]
    public async Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateSubstructureCostProfileOverrideDto dto)
    {
        return await substructureTimeSeriesService.UpdateSubstructureCostProfileOverride(projectId, caseId, substructureId, costProfileId, dto);
    }
}
