using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class CaseRepository : ICaseRepository
{
    private readonly DcdDbContext _context;
    private readonly ILogger<CaseRepository> _logger;

    public CaseRepository(
        DcdDbContext context,
        ILogger<CaseRepository> logger
        )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Case?> GetCase(Guid caseId)
    {
        return await _context.Cases.FindAsync(caseId);
    }

    public async Task<Case> UpdateCase(Case updatedCase)
    {
        _context.Cases.Update(updatedCase);
        await _context.SaveChangesAsync();
        return updatedCase;
    }

    public async Task<CessationWellsCostOverride?> GetCessationWellsCostOverride(Guid costProfileId)
    {
        return await _context.CessationWellsCostOverride.FindAsync(costProfileId);
    }

    public async Task<CessationWellsCostOverride> UpdateCessationWellsCostOverride(CessationWellsCostOverride costProfile)
    {
        _context.CessationWellsCostOverride.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }

    public async Task<CessationOffshoreFacilitiesCostOverride?> GetCessationOffshoreFacilitiesCostOverride(Guid costProfileId)
    {
        return await _context.CessationOffshoreFacilitiesCostOverride.FindAsync(costProfileId);
    }

    public async Task<CessationOffshoreFacilitiesCostOverride> UpdateCessationOffshoreFacilitiesCostOverride(CessationOffshoreFacilitiesCostOverride costProfile)
    {
        _context.CessationOffshoreFacilitiesCostOverride.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }

    public async Task<TotalFeasibilityAndConceptStudiesOverride?> GetTotalFeasibilityAndConceptStudiesOverride(Guid costProfileId)
    {
        return await _context.TotalFeasibilityAndConceptStudiesOverride.FindAsync(costProfileId);
    }

    public async Task<TotalFeasibilityAndConceptStudiesOverride> UpdateTotalFeasibilityAndConceptStudiesOverride(TotalFeasibilityAndConceptStudiesOverride costProfile)
    {
        _context.TotalFeasibilityAndConceptStudiesOverride.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }

    public async Task<TotalFEEDStudiesOverride?> GetTotalFEEDStudiesOverride(Guid costProfileId)
    {
        return await _context.TotalFEEDStudiesOverride.FindAsync(costProfileId);
    }

    public async Task<TotalFEEDStudiesOverride> UpdateTotalFEEDStudiesOverride(TotalFEEDStudiesOverride costProfile)
    {
        _context.TotalFEEDStudiesOverride.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }

    public async Task<HistoricCostCostProfile?> GetHistoricCostCostProfile(Guid costProfileId)
    {
        return await _context.HistoricCostCostProfile.FindAsync(costProfileId);
    }

    public async Task<HistoricCostCostProfile> UpdateHistoricCostCostProfile(HistoricCostCostProfile costProfile)
    {
        _context.HistoricCostCostProfile.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }

    public async Task<WellInterventionCostProfileOverride?> GetWellInterventionCostProfileOverride(Guid costProfileId)
    {
        return await _context.WellInterventionCostProfileOverride.FindAsync(costProfileId);
    }

    public async Task<WellInterventionCostProfileOverride> UpdateWellInterventionCostProfileOverride(WellInterventionCostProfileOverride costProfile)
    {
        _context.WellInterventionCostProfileOverride.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }

    public async Task<OffshoreFacilitiesOperationsCostProfileOverride?> GetOffshoreFacilitiesOperationsCostProfileOverride(Guid costProfileId)
    {
        return await _context.OffshoreFacilitiesOperationsCostProfileOverride.FindAsync(costProfileId);
    }

    public async Task<OffshoreFacilitiesOperationsCostProfileOverride> UpdateOffshoreFacilitiesOperationsCostProfileOverride(OffshoreFacilitiesOperationsCostProfileOverride costProfile)
    {
        _context.OffshoreFacilitiesOperationsCostProfileOverride.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }

    public async Task<OnshoreRelatedOPEXCostProfile?> GetOnshoreRelatedOPEXCostProfile(Guid costProfileId)
    {
        return await _context.OnshoreRelatedOPEXCostProfile.FindAsync(costProfileId);
    }

    public async Task<OnshoreRelatedOPEXCostProfile> UpdateOnshoreRelatedOPEXCostProfile(OnshoreRelatedOPEXCostProfile costProfile)
    {
        _context.OnshoreRelatedOPEXCostProfile.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }

    public async Task<AdditionalOPEXCostProfile?> GetAdditionalOPEXCostProfile(Guid costProfileId)
    {
        return await _context.AdditionalOPEXCostProfile.FindAsync(costProfileId);
    }

    public async Task<AdditionalOPEXCostProfile> UpdateAdditionalOPEXCostProfile(AdditionalOPEXCostProfile costProfile)
    {
        _context.AdditionalOPEXCostProfile.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
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
