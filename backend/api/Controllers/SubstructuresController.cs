using api.Adapters;
using api.Dtos;
using api.Models;
using api.Services;

using Api.Authorization;
using Api.Services.FusionIntegration;

using AutoMapper;

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
        [FromBody] UpdateSubstructureCostProfileOverrideDto dto)
    {
        return await _substructureService.UpdateSubstructure(dto, substructureId);
    }


}
