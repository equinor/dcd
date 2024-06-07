using api.Context;
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

    public Case UpdateCase(Case updatedCase)
    {
        return Update(updatedCase);
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
