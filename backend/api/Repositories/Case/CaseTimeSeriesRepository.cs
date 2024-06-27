using System.Linq.Expressions;

using api.Context;
using api.Enums;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class CaseTimeSeriesRepository : BaseRepository, ICaseTimeSeriesRepository
{
    private readonly ILogger<CaseRepository> _logger;

    public CaseTimeSeriesRepository(
        DcdDbContext context,
        ILogger<CaseRepository> logger
        ) : base(context)
    {
        _logger = logger;
    }

    public CessationWellsCostOverride CreateCessationWellsCostOverride(CessationWellsCostOverride profile)
    {
        _context.CessationWellsCostOverride.Add(profile);
        return profile;
    }

    public CessationOffshoreFacilitiesCostOverride CreateCessationOffshoreFacilitiesCostOverride(CessationOffshoreFacilitiesCostOverride profile)
    {
        _context.CessationOffshoreFacilitiesCostOverride.Add(profile);
        return profile;
    }

    public CessationOnshoreFacilitiesCostProfile CreateCessationOnshoreFacilitiesCostProfile(CessationOnshoreFacilitiesCostProfile profile)
    {
        _context.CessationOnshoreFacilitiesCostProfile.Add(profile);
        return profile;
    }

    public TotalFeasibilityAndConceptStudiesOverride CreateTotalFeasibilityAndConceptStudiesOverride(TotalFeasibilityAndConceptStudiesOverride profile)
    {
        _context.TotalFeasibilityAndConceptStudiesOverride.Add(profile);
        return profile;
    }

    public TotalFEEDStudiesOverride CreateTotalFEEDStudiesOverride(TotalFEEDStudiesOverride profile)
    {
        _context.TotalFEEDStudiesOverride.Add(profile);
        return profile;
    }

    public TotalOtherStudiesCostProfile CreateTotalOtherStudiesCostProfile(TotalOtherStudiesCostProfile profile)
    {
        _context.TotalOtherStudiesCostProfile.Add(profile);
        return profile;
    }

    public HistoricCostCostProfile CreateHistoricCostCostProfile(HistoricCostCostProfile profile)
    {
        _context.HistoricCostCostProfile.Add(profile);
        return profile;
    }

    public WellInterventionCostProfileOverride CreateWellInterventionCostProfileOverride(WellInterventionCostProfileOverride profile)
    {
        _context.WellInterventionCostProfileOverride.Add(profile);
        return profile;
    }

    public OffshoreFacilitiesOperationsCostProfileOverride CreateOffshoreFacilitiesOperationsCostProfileOverride(OffshoreFacilitiesOperationsCostProfileOverride profile)
    {
        _context.OffshoreFacilitiesOperationsCostProfileOverride.Add(profile);
        return profile;
    }

    public OnshoreRelatedOPEXCostProfile CreateOnshoreRelatedOPEXCostProfile(OnshoreRelatedOPEXCostProfile profile)
    {
        _context.OnshoreRelatedOPEXCostProfile.Add(profile);
        return profile;
    }

    public AdditionalOPEXCostProfile CreateAdditionalOPEXCostProfile(AdditionalOPEXCostProfile profile)
    {
        _context.AdditionalOPEXCostProfile.Add(profile);
        return profile;
    }

    public async Task<CessationWellsCostOverride?> GetCessationWellsCostOverride(Guid costProfileId)
    {
        return await Get<CessationWellsCostOverride>(costProfileId);
    }

    public CessationWellsCostOverride UpdateCessationWellsCostOverride(CessationWellsCostOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<CessationOffshoreFacilitiesCostOverride?> GetCessationOffshoreFacilitiesCostOverride(Guid costProfileId)
    {
        return await Get<CessationOffshoreFacilitiesCostOverride>(costProfileId);
    }

    public CessationOffshoreFacilitiesCostOverride UpdateCessationOffshoreFacilitiesCostOverride(CessationOffshoreFacilitiesCostOverride costProfile)
    {
        return Update(costProfile);
    }
    public async Task<CessationOnshoreFacilitiesCostProfile?> GetCessationOnshoreFacilitiesCostProfile(Guid costProfileId)
    {
        return await Get<CessationOnshoreFacilitiesCostProfile>(costProfileId);
    }

    public CessationOnshoreFacilitiesCostProfile UpdateCessationOnshoreFacilitiesCostProfile(CessationOnshoreFacilitiesCostProfile costProfile)
    {
        return Update(costProfile);
    }

    public async Task<TotalFeasibilityAndConceptStudiesOverride?> GetTotalFeasibilityAndConceptStudiesOverride(Guid costProfileId)
    {
        return await Get<TotalFeasibilityAndConceptStudiesOverride>(costProfileId);
    }

    public TotalFeasibilityAndConceptStudiesOverride UpdateTotalFeasibilityAndConceptStudiesOverride(TotalFeasibilityAndConceptStudiesOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<TotalFEEDStudiesOverride?> GetTotalFEEDStudiesOverride(Guid costProfileId)
    {
        return await Get<TotalFEEDStudiesOverride>(costProfileId);
    }

    public TotalFEEDStudiesOverride UpdateTotalFEEDStudiesOverride(TotalFEEDStudiesOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<TotalOtherStudiesCostProfile?> GetTotalOtherStudiesCostProfile(Guid costProfileId)
    {
        return await Get<TotalOtherStudiesCostProfile>(costProfileId);
    }

    public TotalOtherStudiesCostProfile UpdateTotalOtherStudiesCostProfile(TotalOtherStudiesCostProfile costProfile)
    {
        return Update(costProfile);
    }

    public async Task<HistoricCostCostProfile?> GetHistoricCostCostProfile(Guid costProfileId)
    {
        return await Get<HistoricCostCostProfile>(costProfileId);
    }

    public HistoricCostCostProfile UpdateHistoricCostCostProfile(HistoricCostCostProfile costProfile)
    {
        return Update(costProfile);
    }

    public async Task<WellInterventionCostProfileOverride?> GetWellInterventionCostProfileOverride(Guid costProfileId)
    {
        return await Get<WellInterventionCostProfileOverride>(costProfileId);
    }

    public WellInterventionCostProfileOverride UpdateWellInterventionCostProfileOverride(WellInterventionCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<OffshoreFacilitiesOperationsCostProfileOverride?> GetOffshoreFacilitiesOperationsCostProfileOverride(Guid costProfileId)
    {
        return await Get<OffshoreFacilitiesOperationsCostProfileOverride>(costProfileId);
    }

    public OffshoreFacilitiesOperationsCostProfileOverride UpdateOffshoreFacilitiesOperationsCostProfileOverride(OffshoreFacilitiesOperationsCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<OnshoreRelatedOPEXCostProfile?> GetOnshoreRelatedOPEXCostProfile(Guid costProfileId)
    {
        return await Get<OnshoreRelatedOPEXCostProfile>(costProfileId);
    }

    public OnshoreRelatedOPEXCostProfile UpdateOnshoreRelatedOPEXCostProfile(OnshoreRelatedOPEXCostProfile costProfile)
    {
        return Update(costProfile);
    }

    public async Task<AdditionalOPEXCostProfile?> GetAdditionalOPEXCostProfile(Guid costProfileId)
    {
        return await Get<AdditionalOPEXCostProfile>(costProfileId);
    }

    public AdditionalOPEXCostProfile UpdateAdditionalOPEXCostProfile(AdditionalOPEXCostProfile costProfile)
    {
        return Update(costProfile);
    }
}
