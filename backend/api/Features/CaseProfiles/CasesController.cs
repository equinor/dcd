using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Create;
using api.Features.CaseProfiles.Dtos.Update;
using api.Features.CaseProfiles.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.CaseProfiles;

[ApiController]
[Route("projects/{projectId}/cases")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.User
)]
[ActionType(ActionType.Edit)]
public class CasesController(ICaseTimeSeriesService caseTimeSeriesService) : ControllerBase
{
    [HttpPut("{caseId}/cessation-wells-cost-override/{costProfileId}")]
    public async Task<CessationWellsCostOverrideDto> UpdateCessationWellsCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateCessationWellsCostOverrideDto dto
    )
    {
        return await caseTimeSeriesService.UpdateCessationWellsCostOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/cessation-offshore-facilities-cost-override/{costProfileId}")]
    public async Task<CessationOffshoreFacilitiesCostOverrideDto> UpdateCessationOffshoreFacilitiesCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateCessationOffshoreFacilitiesCostOverrideDto dto
    )
    {
        return await caseTimeSeriesService.UpdateCessationOffshoreFacilitiesCostOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPost("{caseId}/cessation-offshore-facilities-cost-override")]
    public async Task<CessationOffshoreFacilitiesCostOverrideDto> CreateCessationOffshoreFacilitiesCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateCessationOffshoreFacilitiesCostOverrideDto dto
    )
    {
        return await caseTimeSeriesService.CreateCessationOffshoreFacilitiesCostOverride(projectId, caseId, dto);
    }

    [HttpPut("{caseId}/cessation-onshore-facilities-cost-profile/{costProfileId}")]
    public async Task<CessationOnshoreFacilitiesCostProfileDto> UpdateCessationOnshoreFacilitiesCostProfile(
    [FromRoute] Guid projectId,
    [FromRoute] Guid caseId,
    [FromRoute] Guid costProfileId,
    [FromBody] UpdateCessationOnshoreFacilitiesCostProfileDto dto
)
    {
        return await caseTimeSeriesService.UpdateCessationOnshoreFacilitiesCostProfile(projectId, caseId, costProfileId, dto);
    }

    [HttpPost("{caseId}/cessation-onshore-facilities-cost-profile")]
    public async Task<CessationOnshoreFacilitiesCostProfileDto> CreateCessationOnshoreFacilitiesCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateCessationOnshoreFacilitiesCostProfileDto dto
    )
    {
        return await caseTimeSeriesService.CreateCessationOnshoreFacilitiesCostProfile(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/cessation-wells-cost-override")]
    public async Task<CessationWellsCostOverrideDto> CreateCessationWellsCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateCessationWellsCostOverrideDto dto
    )
    {
        return await caseTimeSeriesService.CreateCessationWellsCostOverride(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/total-feasibility-and-concept-studies-override")]
    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> CreateTotalFeasibilityAndConceptStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTotalFeasibilityAndConceptStudiesOverrideDto dto
    )
    {
        return await caseTimeSeriesService.CreateTotalFeasibilityAndConceptStudiesOverride(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/total-feed-studies-override")]
    public async Task<TotalFEEDStudiesOverrideDto> CreateTotalFeedStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTotalFEEDStudiesOverrideDto dto
    )
    {
        return await caseTimeSeriesService.CreateTotalFeedStudiesOverride(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/total-other-studies-cost-profile")]
    public async Task<TotalOtherStudiesCostProfileDto> CreateTotalOtherStudiesCostProfile(
    [FromRoute] Guid projectId,
    [FromRoute] Guid caseId,
    [FromBody] CreateTotalOtherStudiesCostProfileDto dto
    )
    {
        return await caseTimeSeriesService.CreateTotalOtherStudiesCostProfile(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/historic-cost-cost-profile")]
    public async Task<HistoricCostCostProfileDto> CreateHistoricCostCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateHistoricCostCostProfileDto dto
    )
    {
        return await caseTimeSeriesService.CreateHistoricCostCostProfile(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/well-intervention-cost-profile-override")]
    public async Task<WellInterventionCostProfileOverrideDto> CreateWellInterventionCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateWellInterventionCostProfileOverrideDto dto
    )
    {
        return await caseTimeSeriesService.CreateWellInterventionCostProfileOverride(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/offshore-facilities-operations-cost-profile-override")]
    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> CreateOffshoreFacilitiesOperationsCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateOffshoreFacilitiesOperationsCostProfileOverrideDto dto
    )
    {
        return await caseTimeSeriesService.CreateOffshoreFacilitiesOperationsCostProfileOverride(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/onshore-related-opex-cost-profile")]
    public async Task<OnshoreRelatedOPEXCostProfileDto> CreateOnshoreRelatedOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateOnshoreRelatedOPEXCostProfileDto dto
    )
    {
        return await caseTimeSeriesService.CreateOnshoreRelatedOPEXCostProfile(projectId, caseId, dto);
    }

    [HttpPost("{caseId}/additional-opex-cost-profile")]
    public async Task<AdditionalOPEXCostProfileDto> CreateAdditionalOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateAdditionalOPEXCostProfileDto dto
    )
    {
        return await caseTimeSeriesService.CreateAdditionalOpexCostProfile(projectId, caseId, dto);
    }

    [HttpPut("{caseId}/total-feasibility-and-concept-studies-override/{costProfileId}")]
    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> UpdateTotalFeasibilityAndConceptStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTotalFeasibilityAndConceptStudiesOverrideDto dto
    )
    {
        return await caseTimeSeriesService.UpdateTotalFeasibilityAndConceptStudiesOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/total-feed-studies-override/{costProfileId}")]
    public async Task<TotalFEEDStudiesOverrideDto> UpdateTotalFEEDStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTotalFEEDStudiesOverrideDto dto
    )
    {
        return await caseTimeSeriesService.UpdateTotalFeedStudiesOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/total-other-studies-cost-profile/{costProfileId}")]
    public async Task<TotalOtherStudiesCostProfileDto> UpdateTotalOtherStudiesCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTotalOtherStudiesCostProfileDto dto
    )
    {
        return await caseTimeSeriesService.UpdateTotalOtherStudiesCostProfile(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/historic-cost-cost-profile/{costProfileId}")]
    public async Task<HistoricCostCostProfileDto> UpdateHistoricCostCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateHistoricCostCostProfileDto dto
    )
    {
        return await caseTimeSeriesService.UpdateHistoricCostCostProfile(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/well-intervention-cost-profile-override/{costProfileId}")]
    public async Task<WellInterventionCostProfileOverrideDto> UpdateWellInterventionCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateWellInterventionCostProfileOverrideDto dto
    )
    {
        return await caseTimeSeriesService.UpdateWellInterventionCostProfileOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/offshore-facilities-operations-cost-profile-override/{costProfileId}")]
    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> UpdateOffshoreFacilitiesOperationsCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto dto
    )
    {
        return await caseTimeSeriesService.UpdateOffshoreFacilitiesOperationsCostProfileOverride(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/onshore-related-opex-cost-profile/{costProfileId}")]
    public async Task<OnshoreRelatedOPEXCostProfileDto> UpdateOnshoreRelatedOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOnshoreRelatedOPEXCostProfileDto dto
    )
    {
        return await caseTimeSeriesService.UpdateOnshoreRelatedOPEXCostProfile(projectId, caseId, costProfileId, dto);
    }

    [HttpPut("{caseId}/additional-opex-cost-profile/{costProfileId}")]
    public async Task<AdditionalOPEXCostProfileDto> UpdateAdditionalOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateAdditionalOPEXCostProfileDto dto
    )
    {
        return await caseTimeSeriesService.UpdateAdditionalOpexCostProfile(projectId, caseId, costProfileId, dto);
    }
}
