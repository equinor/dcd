using api.Models;

namespace api.Repositories;


public interface ICaseRepository : IBaseRepository
{
    Task<Case?> GetCase(Guid caseId);
    Case UpdateCase(Case updatedCase);
    Task<CessationWellsCostOverride?> GetCessationWellsCostOverride(Guid costProfileId);
    CessationWellsCostOverride UpdateCessationWellsCostOverride(CessationWellsCostOverride costProfile);
    Task<CessationOffshoreFacilitiesCostOverride?> GetCessationOffshoreFacilitiesCostOverride(Guid costProfileId);
    CessationOffshoreFacilitiesCostOverride UpdateCessationOffshoreFacilitiesCostOverride(CessationOffshoreFacilitiesCostOverride costProfile);
    Task<TotalFeasibilityAndConceptStudiesOverride?> GetTotalFeasibilityAndConceptStudiesOverride(Guid costProfileId);
    TotalFeasibilityAndConceptStudiesOverride UpdateTotalFeasibilityAndConceptStudiesOverride(TotalFeasibilityAndConceptStudiesOverride costProfile);
    Task<TotalFEEDStudiesOverride?> GetTotalFEEDStudiesOverride(Guid costProfileId);
    TotalFEEDStudiesOverride UpdateTotalFEEDStudiesOverride(TotalFEEDStudiesOverride costProfile);
    Task<HistoricCostCostProfile?> GetHistoricCostCostProfile(Guid costProfileId);
    HistoricCostCostProfile UpdateHistoricCostCostProfile(HistoricCostCostProfile costProfile);
    Task<WellInterventionCostProfileOverride?> GetWellInterventionCostProfileOverride(Guid costProfileId);
    WellInterventionCostProfileOverride UpdateWellInterventionCostProfileOverride(WellInterventionCostProfileOverride costProfile);
    Task<OffshoreFacilitiesOperationsCostProfileOverride?> GetOffshoreFacilitiesOperationsCostProfileOverride(Guid costProfileId);
    OffshoreFacilitiesOperationsCostProfileOverride UpdateOffshoreFacilitiesOperationsCostProfileOverride(OffshoreFacilitiesOperationsCostProfileOverride costProfile);
    Task<OnshoreRelatedOPEXCostProfile?> GetOnshoreRelatedOPEXCostProfile(Guid costProfileId);
    OnshoreRelatedOPEXCostProfile UpdateOnshoreRelatedOPEXCostProfile(OnshoreRelatedOPEXCostProfile costProfile);
    Task<AdditionalOPEXCostProfile?> GetAdditionalOPEXCostProfile(Guid costProfileId);
    AdditionalOPEXCostProfile UpdateAdditionalOPEXCostProfile(AdditionalOPEXCostProfile costProfile);
    Task UpdateModifyTime(Guid caseId);
}
