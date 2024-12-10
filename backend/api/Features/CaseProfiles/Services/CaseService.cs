using System.Linq.Expressions;

using api.Context;
using api.Exceptions;
using api.Features.CaseProfiles.Repositories;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services;

public class CaseService(
    DcdDbContext context,
    ICaseRepository repository)
    : ICaseService
{
    public async Task<Project> GetProject(Guid id)
    {
        return await repository.GetProject(id);
    }

    public async Task<Case> GetCase(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
            .Include(c => c.TotalOtherStudiesCostProfile)
            .Include(c => c.HistoricCostCostProfile)
            .Include(c => c.WellInterventionCostProfile)
            .Include(c => c.WellInterventionCostProfileOverride)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(c => c.AdditionalOPEXCostProfile)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(c => c.CessationOnshoreFacilitiesCostProfile)
            .Include(c => c.CalculatedTotalIncomeCostProfile)
            .Include(c => c.CalculatedTotalCostCostProfile)
            .SingleOrDefaultAsync(c => c.Id == caseId)
        ?? throw new NotFoundInDBException($"Case {caseId} not found.");

        return caseItem;
    }

    public async Task<Case> GetCaseWithIncludes(Guid caseId, params Expression<Func<Case, object>>[] includes)
    {
        return await repository.GetCaseWithIncludes(caseId, includes)
            ?? throw new NotFoundInDBException($"Case with id {caseId} not found.");
    }

    // TODO: Delete this method
    public async Task<IEnumerable<Case>> GetAll()
    {
        return await context.Cases.ToListAsync();
    }
}
