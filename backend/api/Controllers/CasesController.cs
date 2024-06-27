using api.Authorization;
using api.Dtos;
using api.Services;

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
    private readonly ICaseTimeSeriesService _caseTimeSeriesService;
    private readonly IDuplicateCaseService _duplicateCaseService;

    public CasesController(
        ICaseService caseService,
        ICaseTimeSeriesService caseTimeSeriesService,
        IDuplicateCaseService duplicateCaseService
        )
    {
        _caseService = caseService;
        _caseTimeSeriesService = caseTimeSeriesService;
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

    // TODO: Remove once autosave is properly implemented
    [HttpPut("{caseId}/update-case-and-profiles")]
    public async Task<ProjectDto> UpdateCaseAndProfiles([FromRoute] Guid caseId, [FromBody] APIUpdateCaseWithProfilesDto caseDto)
    {
        return await _caseService.UpdateCaseAndProfiles(caseId, caseDto);
    }

    [HttpPut("{caseId}")]
    public async Task<CaseDto> UpdateCase([FromRoute] Guid caseId, [FromBody] APIUpdateCaseDto caseDto)
    {
        return await _caseService.UpdateCase(caseId, caseDto);
    }


    [HttpDelete("{caseId}")]
    public async Task<ProjectDto> DeleteCase(Guid caseId)
    {
        return await _caseService.DeleteCase(caseId);
    }

    [HttpPut("{caseId}/cessation-wells-cost-override/{costProfileId}")]
    public async Task<CessationWellsCostOverrideDto> UpdateCessationWellsCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateCessationWellsCostOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.UpdateCessationWellsCostOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/cessation-offshore-facilities-cost-override/{costProfileId}")]
    public async Task<CessationOffshoreFacilitiesCostOverrideDto> UpdateCessationOffshoreFacilitiesCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateCessationOffshoreFacilitiesCostOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.UpdateCessationOffshoreFacilitiesCostOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPost("{caseId}/cessation-offshore-facilities-cost-override")]
    public async Task<CessationOffshoreFacilitiesCostOverrideDto> CreateCessationOffshoreFacilitiesCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateCessationOffshoreFacilitiesCostOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.CreateCessationOffshoreFacilitiesCostOverride(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/cessation-wells-cost-override")]
    public async Task<CessationWellsCostOverrideDto> CreateCessationWellsCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateCessationWellsCostOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.CreateCessationWellsCostOverride(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/total-feasibility-and-concept-studies-override")]
    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> CreateTotalFeasibilityAndConceptStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTotalFeasibilityAndConceptStudiesOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.CreateTotalFeasibilityAndConceptStudiesOverride(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/total-feed-studies-override")]
    public async Task<TotalFEEDStudiesOverrideDto> CreateTotalFEEDStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTotalFEEDStudiesOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.CreateTotalFEEDStudiesOverride(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/total-other-studies-cost-profile")]
    public async Task<TotalOtherStudiesCostProfileDto> CreateTotalOtherStudiesCostProfile(
    [FromRoute] Guid projectId,
    [FromRoute] Guid caseId,
    [FromBody] CreateTotalOtherStudiesCostProfileDto dto
)
    {
        return await _caseTimeSeriesService.CreateTotalOtherStudiesCostProfile(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/historic-cost-cost-profile")]
    public async Task<HistoricCostCostProfileDto> CreateHistoricCostCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateHistoricCostCostProfileDto dto
    )
    {
        return await _caseTimeSeriesService.CreateHistoricCostCostProfile(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/well-intervention-cost-profile-override")]
    public async Task<WellInterventionCostProfileOverrideDto> CreateWellInterventionCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateWellInterventionCostProfileOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.CreateWellInterventionCostProfileOverride(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/offshore-facilities-operations-cost-profile-override")]
    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> CreateOffshoreFacilitiesOperationsCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateOffshoreFacilitiesOperationsCostProfileOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.CreateOffshoreFacilitiesOperationsCostProfileOverride(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/onshore-related-opex-cost-profile")]
    public async Task<OnshoreRelatedOPEXCostProfileDto> CreateOnshoreRelatedOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateOnshoreRelatedOPEXCostProfileDto dto
    )
    {
        return await _caseTimeSeriesService.CreateOnshoreRelatedOPEXCostProfile(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/additional-opex-cost-profile")]
    public async Task<AdditionalOPEXCostProfileDto> CreateAdditionalOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateAdditionalOPEXCostProfileDto dto
    )
    {
        return await _caseTimeSeriesService.CreateAdditionalOPEXCostProfile(projectId, caseId, dto);
    }

    [HttpPut("{caseId}/total-feasibility-and-concept-studies-override/{costProfileId}")]
    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> UpdateTotalFeasibilityAndConceptStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTotalFeasibilityAndConceptStudiesOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.UpdateTotalFeasibilityAndConceptStudiesOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/total-feed-studies-override/{costProfileId}")]
    public async Task<TotalFEEDStudiesOverrideDto> UpdateTotalFEEDStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTotalFEEDStudiesOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.UpdateTotalFEEDStudiesOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/total-other-studies-cost-profile/{costProfileId}")]
    public async Task<TotalOtherStudiesCostProfileDto> UpdateTotalOtherStudiesCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTotalOtherStudiesCostProfileDto dto
    )
    {
        return await _caseTimeSeriesService.UpdateTotalOtherStudiesCostProfile(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/historic-cost-cost-profile/{costProfileId}")]
    public async Task<HistoricCostCostProfileDto> UpdateHistoricCostCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateHistoricCostCostProfileDto dto
    )
    {
        return await _caseTimeSeriesService.UpdateHistoricCostCostProfile(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/well-intervention-cost-profile-override/{costProfileId}")]
    public async Task<WellInterventionCostProfileOverrideDto> UpdateWellInterventionCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateWellInterventionCostProfileOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.UpdateWellInterventionCostProfileOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/offshore-facilities-operations-cost-profile-override/{costProfileId}")]
    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> UpdateOffshoreFacilitiesOperationsCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto dto
    )
    {
        return await _caseTimeSeriesService.UpdateOffshoreFacilitiesOperationsCostProfileOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/onshore-related-opex-cost-profile/{costProfileId}")]
    public async Task<OnshoreRelatedOPEXCostProfileDto> UpdateOnshoreRelatedOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOnshoreRelatedOPEXCostProfileDto dto
    )
    {
        return await _caseTimeSeriesService.UpdateOnshoreRelatedOPEXCostProfile(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/additional-opex-cost-profile/{costProfileId}")]
    public async Task<AdditionalOPEXCostProfileDto> UpdateAdditionalOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateAdditionalOPEXCostProfileDto dto
    )
    {
        return await _caseTimeSeriesService.UpdateAdditionalOPEXCostProfile(projectId, caseId, costProfileId, dto);
    }
}
