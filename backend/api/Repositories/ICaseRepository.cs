using api.Models;

namespace api.Repositories;


public interface ICaseRepository
{
    Task<Case?> GetCase(Guid caseId);
    Task<Case> UpdateCase(Case updatedCase);
    Task<CessationWellsCostOverride?> GetCessationWellsCostOverride(Guid costProfileId);
    Task<CessationWellsCostOverride> UpdateCessationWellsCostOverride(CessationWellsCostOverride costProfile);
    Task<CessationOffshoreFacilitiesCostOverride?> GetCessationOffshoreFacilitiesCostOverride(Guid costProfileId);
    Task<CessationOffshoreFacilitiesCostOverride> UpdateCessationOffshoreFacilitiesCostOverride(CessationOffshoreFacilitiesCostOverride costProfile);
    Task<TotalFeasibilityAndConceptStudiesOverride?> GetTotalFeasibilityAndConceptStudiesOverride(Guid costProfileId);
    Task<TotalFeasibilityAndConceptStudiesOverride> UpdateTotalFeasibilityAndConceptStudiesOverride(TotalFeasibilityAndConceptStudiesOverride costProfile);
    Task<TotalFEEDStudiesOverride?> GetTotalFEEDStudiesOverride(Guid costProfileId);
    Task<TotalFEEDStudiesOverride> UpdateTotalFEEDStudiesOverride(TotalFEEDStudiesOverride costProfile);
    Task<HistoricCostCostProfile?> GetHistoricCostCostProfile(Guid costProfileId);
    Task<HistoricCostCostProfile> UpdateHistoricCostCostProfile(HistoricCostCostProfile costProfile);
    Task<WellInterventionCostProfileOverride?> GetWellInterventionCostProfileOverride(Guid costProfileId);
    Task<WellInterventionCostProfileOverride> UpdateWellInterventionCostProfileOverride(WellInterventionCostProfileOverride costProfile);
    Task<OffshoreFacilitiesOperationsCostProfileOverride?> GetOffshoreFacilitiesOperationsCostProfileOverride(Guid costProfileId);
    Task<OffshoreFacilitiesOperationsCostProfileOverride> UpdateOffshoreFacilitiesOperationsCostProfileOverride(OffshoreFacilitiesOperationsCostProfileOverride costProfile);
    Task<OnshoreRelatedOPEXCostProfile?> GetOnshoreRelatedOPEXCostProfile(Guid costProfileId);
    Task<OnshoreRelatedOPEXCostProfile> UpdateOnshoreRelatedOPEXCostProfile(OnshoreRelatedOPEXCostProfile costProfile);
    Task<AdditionalOPEXCostProfile?> GetAdditionalOPEXCostProfile(Guid costProfileId);
    Task<AdditionalOPEXCostProfile> UpdateAdditionalOPEXCostProfile(AdditionalOPEXCostProfile costProfile);

    Task UpdateModifyTime(Guid caseId);
}
