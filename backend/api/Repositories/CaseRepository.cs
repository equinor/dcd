using System.Linq.Expressions;

using api.Context;
using api.Enums;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class CaseRepository : BaseRepository, ICaseRepository
{
    private readonly ILogger<CaseRepository> _logger;

    public CaseRepository(
        DcdDbContext context,
        ILogger<CaseRepository> logger
        ) : base(context)
    {
        _logger = logger;
    }

    public async Task<Case?> GetCase(Guid caseId)
    {
        return await Get<Case>(caseId);
    }

    public async Task<bool> CaseHasProfile(Guid caseId, CaseProfileNames profileType)
    {
        Expression<Func<Case, bool>> profileExistsExpression = profileType switch
        {
            CaseProfileNames.CessationWellsCostOverride => d => d.CessationWellsCostOverride != null,
            CaseProfileNames.CessationOffshoreFacilitiesCostOverride => d => d.CessationOffshoreFacilitiesCostOverride != null,
            CaseProfileNames.TotalFeasibilityAndConceptStudiesOverride => d => d.TotalFeasibilityAndConceptStudiesOverride != null,
            CaseProfileNames.TotalFEEDStudiesOverride => d => d.TotalFEEDStudiesOverride != null,
            CaseProfileNames.HistoricCostCostProfile => d => d.HistoricCostCostProfile != null,
            CaseProfileNames.WellInterventionCostProfileOverride => d => d.WellInterventionCostProfileOverride != null,
            CaseProfileNames.OffshoreFacilitiesOperationsCostProfileOverride => d => d.OffshoreFacilitiesOperationsCostProfileOverride != null,
            CaseProfileNames.OnshoreRelatedOPEXCostProfile => d => d.OnshoreRelatedOPEXCostProfile != null,
            CaseProfileNames.AdditionalOPEXCostProfile => d => d.AdditionalOPEXCostProfile != null,
        };

        bool hasProfile = await _context.Cases
            .Where(d => d.Id == caseId)
            .AnyAsync(profileExistsExpression);

        return hasProfile;
    }

    public Case UpdateCase(Case updatedCase)
    {
        return Update(updatedCase);
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

    public async Task UpdateModifyTime(Guid caseId)
    {
        if (caseId == Guid.Empty)
        {
            throw new ArgumentException("The case id cannot be empty.", nameof(caseId));
        }

        var caseItem = await _context.Cases.SingleOrDefaultAsync(c => c.Id == caseId)
            ?? throw new KeyNotFoundException($"Case with id {caseId} not found.");

        caseItem.ModifyTime = DateTimeOffset.UtcNow;

        // try
        // {
        //     await _context.SaveChangesAsync();
        // }
        // catch (DbUpdateConcurrencyException ex)
        // {
        //     _logger.LogWarning(ex, "Failed to update ModifyDate for Case with id {caseId}.", caseId);
        // }
        // catch (DbUpdateException ex)
        // {
        //     _logger.LogWarning(ex, "An error occurred while updating ModifyDate for the Case with id {caseId}.", caseId);
        // }
        // catch (Exception ex)
        // {
        //     _logger.LogWarning(ex, "Failed to update modify time for case id {CaseId}, but operation continues.", caseId);
        // }
    }
}
