using api.Dtos;
using api.Models;

namespace api.Services;

public interface ICaseService
{
    Task<ProjectDto> CreateCase(Guid projectId, CreateCaseDto createCaseDto);
    Task<ProjectDto> UpdateCaseAndProfiles<TDto>(Guid caseId, TDto updatedCaseDto) where TDto : BaseUpdateCaseDto;
    Task<ProjectDto> DeleteCase(Guid caseId);
    Task<Case> GetCase(Guid caseId);
    Task<IEnumerable<Case>> GetAll();
    Task<CaseDto> UpdateCase<TDto>(Guid caseId, TDto updatedCaseDto) where TDto : BaseUpdateCaseDto;
    Task<CessationWellsCostOverrideDto> UpdateCessationWellsCostOverride(Guid projectId, Guid caseId, Guid costProfileId, UpdateCessationWellsCostOverrideDto updatedCostProfileDto);
    Task<CessationOffshoreFacilitiesCostOverrideDto> UpdateCessationOffshoreFacilitiesCostOverride(Guid projectId, Guid caseId, Guid costProfileId, UpdateCessationOffshoreFacilitiesCostOverrideDto updatedCostProfileDto);
    Task<TotalFeasibilityAndConceptStudiesOverrideDto> UpdateTotalFeasibilityAndConceptStudiesOverride(Guid projectId, Guid caseId, Guid costProfileId, UpdateTotalFeasibilityAndConceptStudiesOverrideDto updatedCostProfileDto);
    Task<TotalFEEDStudiesOverrideDto> UpdateTotalFEEDStudiesOverride(Guid projectId, Guid caseId, Guid costProfileId, UpdateTotalFEEDStudiesOverrideDto updatedCostProfileDto);
    Task<HistoricCostCostProfileDto> UpdateHistoricCostCostProfile(Guid projectId, Guid caseId, Guid costProfileId, UpdateHistoricCostCostProfileDto updatedCostProfileDto);
    Task<WellInterventionCostProfileOverrideDto> UpdateWellInterventionCostProfileOverride(Guid projectId, Guid caseId, Guid costProfileId, UpdateWellInterventionCostProfileOverrideDto updatedCostProfileDto);
    Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> UpdateOffshoreFacilitiesOperationsCostProfileOverride(Guid projectId, Guid caseId, Guid costProfileId, UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto updatedCostProfileDto);
    Task<OnshoreRelatedOPEXCostProfileDto> UpdateOnshoreRelatedOPEXCostProfile(Guid projectId, Guid caseId, Guid costProfileId, UpdateOnshoreRelatedOPEXCostProfileDto updatedCostProfileDto);
    Task<AdditionalOPEXCostProfileDto> UpdateAdditionalOPEXCostProfile(Guid projectId, Guid caseId, Guid costProfileId, UpdateAdditionalOPEXCostProfileDto updatedCostProfileDto);
    Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> CreateOffshoreFacilitiesOperationsCostProfileOverride(Guid projectId, Guid caseId, CreateOffshoreFacilitiesOperationsCostProfileOverrideDto createProfileDto);
    Task<CessationWellsCostOverrideDto> CreateCessationWellsCostOverride(Guid projectId, Guid caseId, CreateCessationWellsCostOverrideDto createProfileDto);
    Task<CessationOffshoreFacilitiesCostOverrideDto> CreateCessationOffshoreFacilitiesCostOverride(Guid projectId, Guid caseId, CreateCessationOffshoreFacilitiesCostOverrideDto createProfileDto);
    Task<TotalFeasibilityAndConceptStudiesOverrideDto> CreateTotalFeasibilityAndConceptStudiesOverride(Guid projectId, Guid caseId, CreateTotalFeasibilityAndConceptStudiesOverrideDto createProfileDto);
    Task<TotalFEEDStudiesOverrideDto> CreateTotalFEEDStudiesOverride(Guid projectId, Guid caseId, CreateTotalFEEDStudiesOverrideDto createProfileDto);
    Task<HistoricCostCostProfileDto> CreateHistoricCostCostProfile(Guid projectId, Guid caseId, CreateHistoricCostCostProfileDto createProfileDto);
    Task<WellInterventionCostProfileOverrideDto> CreateWellInterventionCostProfileOverride(Guid projectId, Guid caseId, CreateWellInterventionCostProfileOverrideDto createProfileDto);
    Task<OnshoreRelatedOPEXCostProfileDto> CreateOnshoreRelatedOPEXCostProfile(Guid projectId, Guid caseId, CreateOnshoreRelatedOPEXCostProfileDto createProfileDto);
    Task<AdditionalOPEXCostProfileDto> CreateAdditionalOPEXCostProfile(Guid projectId, Guid caseId, CreateAdditionalOPEXCostProfileDto createProfileDto);
}
