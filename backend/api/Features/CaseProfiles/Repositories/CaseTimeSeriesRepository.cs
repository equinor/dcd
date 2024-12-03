using api.Context;
using api.Models;

namespace api.Features.CaseProfiles.Repositories;

public class CaseTimeSeriesRepository(DcdDbContext context) : BaseRepository(context), ICaseTimeSeriesRepository
{
    public CessationWellsCostOverride CreateCessationWellsCostOverride(CessationWellsCostOverride profile)
    {
        Context.CessationWellsCostOverride.Add(profile);
        return profile;
    }

    public CessationOffshoreFacilitiesCostOverride CreateCessationOffshoreFacilitiesCostOverride(CessationOffshoreFacilitiesCostOverride profile)
    {
        Context.CessationOffshoreFacilitiesCostOverride.Add(profile);
        return profile;
    }

    public CessationOnshoreFacilitiesCostProfile CreateCessationOnshoreFacilitiesCostProfile(CessationOnshoreFacilitiesCostProfile profile)
    {
        Context.CessationOnshoreFacilitiesCostProfile.Add(profile);
        return profile;
    }

    public TotalFeasibilityAndConceptStudiesOverride CreateTotalFeasibilityAndConceptStudiesOverride(TotalFeasibilityAndConceptStudiesOverride profile)
    {
        Context.TotalFeasibilityAndConceptStudiesOverride.Add(profile);
        return profile;
    }

    public TotalFEEDStudiesOverride CreateTotalFEEDStudiesOverride(TotalFEEDStudiesOverride profile)
    {
        Context.TotalFEEDStudiesOverride.Add(profile);
        return profile;
    }

    public TotalOtherStudiesCostProfile CreateTotalOtherStudiesCostProfile(TotalOtherStudiesCostProfile profile)
    {
        Context.TotalOtherStudiesCostProfile.Add(profile);
        return profile;
    }

    public HistoricCostCostProfile CreateHistoricCostCostProfile(HistoricCostCostProfile profile)
    {
        Context.HistoricCostCostProfile.Add(profile);
        return profile;
    }

    public WellInterventionCostProfileOverride CreateWellInterventionCostProfileOverride(WellInterventionCostProfileOverride profile)
    {
        Context.WellInterventionCostProfileOverride.Add(profile);
        return profile;
    }

    public OffshoreFacilitiesOperationsCostProfileOverride CreateOffshoreFacilitiesOperationsCostProfileOverride(OffshoreFacilitiesOperationsCostProfileOverride profile)
    {
        Context.OffshoreFacilitiesOperationsCostProfileOverride.Add(profile);
        return profile;
    }

    public OnshoreRelatedOPEXCostProfile CreateOnshoreRelatedOPEXCostProfile(OnshoreRelatedOPEXCostProfile profile)
    {
        Context.OnshoreRelatedOPEXCostProfile.Add(profile);
        return profile;
    }

    public AdditionalOPEXCostProfile CreateAdditionalOPEXCostProfile(AdditionalOPEXCostProfile profile)
    {
        Context.AdditionalOPEXCostProfile.Add(profile);
        return profile;
    }

    public async Task<CessationWellsCostOverride?> GetCessationWellsCostOverride(Guid costProfileId)
    {
        return await GetWithIncludes<CessationWellsCostOverride>(costProfileId, c => c.Case);
    }

    public CessationWellsCostOverride UpdateCessationWellsCostOverride(CessationWellsCostOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<CessationOffshoreFacilitiesCostOverride?> GetCessationOffshoreFacilitiesCostOverride(Guid costProfileId)
    {
        return await GetWithIncludes<CessationOffshoreFacilitiesCostOverride>(costProfileId, c => c.Case);
    }

    public CessationOffshoreFacilitiesCostOverride UpdateCessationOffshoreFacilitiesCostOverride(CessationOffshoreFacilitiesCostOverride costProfile)
    {
        return Update(costProfile);
    }
    public async Task<CessationOnshoreFacilitiesCostProfile?> GetCessationOnshoreFacilitiesCostProfile(Guid costProfileId)
    {
        return await GetWithIncludes<CessationOnshoreFacilitiesCostProfile>(costProfileId, c => c.Case);
    }

    public CessationOnshoreFacilitiesCostProfile UpdateCessationOnshoreFacilitiesCostProfile(CessationOnshoreFacilitiesCostProfile costProfile)
    {
        return Update(costProfile);
    }

    public async Task<TotalFeasibilityAndConceptStudiesOverride?> GetTotalFeasibilityAndConceptStudiesOverride(Guid costProfileId)
    {
        return await GetWithIncludes<TotalFeasibilityAndConceptStudiesOverride>(costProfileId, c => c.Case);
    }

    public TotalFeasibilityAndConceptStudiesOverride UpdateTotalFeasibilityAndConceptStudiesOverride(TotalFeasibilityAndConceptStudiesOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<TotalFEEDStudiesOverride?> GetTotalFEEDStudiesOverride(Guid costProfileId)
    {
        return await GetWithIncludes<TotalFEEDStudiesOverride>(costProfileId, c => c.Case);
    }

    public TotalFEEDStudiesOverride UpdateTotalFEEDStudiesOverride(TotalFEEDStudiesOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<TotalOtherStudiesCostProfile?> GetTotalOtherStudiesCostProfile(Guid costProfileId)
    {
        return await GetWithIncludes<TotalOtherStudiesCostProfile>(costProfileId, c => c.Case);
    }

    public TotalOtherStudiesCostProfile UpdateTotalOtherStudiesCostProfile(TotalOtherStudiesCostProfile costProfile)
    {
        return Update(costProfile);
    }

    public async Task<HistoricCostCostProfile?> GetHistoricCostCostProfile(Guid costProfileId)
    {
        return await GetWithIncludes<HistoricCostCostProfile>(costProfileId, c => c.Case);
    }

    public HistoricCostCostProfile UpdateHistoricCostCostProfile(HistoricCostCostProfile costProfile)
    {
        return Update(costProfile);
    }

    public async Task<WellInterventionCostProfileOverride?> GetWellInterventionCostProfileOverride(Guid costProfileId)
    {
        return await GetWithIncludes<WellInterventionCostProfileOverride>(costProfileId, c => c.Case);
    }

    public WellInterventionCostProfileOverride UpdateWellInterventionCostProfileOverride(WellInterventionCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<OffshoreFacilitiesOperationsCostProfileOverride?> GetOffshoreFacilitiesOperationsCostProfileOverride(Guid costProfileId)
    {
        return await GetWithIncludes<OffshoreFacilitiesOperationsCostProfileOverride>(costProfileId, c => c.Case);
    }

    public OffshoreFacilitiesOperationsCostProfileOverride UpdateOffshoreFacilitiesOperationsCostProfileOverride(OffshoreFacilitiesOperationsCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<OnshoreRelatedOPEXCostProfile?> GetOnshoreRelatedOPEXCostProfile(Guid costProfileId)
    {
        return await GetWithIncludes<OnshoreRelatedOPEXCostProfile>(costProfileId, c => c.Case);
    }

    public OnshoreRelatedOPEXCostProfile UpdateOnshoreRelatedOPEXCostProfile(OnshoreRelatedOPEXCostProfile costProfile)
    {
        return Update(costProfile);
    }

    public async Task<AdditionalOPEXCostProfile?> GetAdditionalOPEXCostProfile(Guid costProfileId)
    {
        return await GetWithIncludes<AdditionalOPEXCostProfile>(costProfileId, c => c.Case);
    }

    public AdditionalOPEXCostProfile UpdateAdditionalOPEXCostProfile(AdditionalOPEXCostProfile costProfile)
    {
        return Update(costProfile);
    }
}
