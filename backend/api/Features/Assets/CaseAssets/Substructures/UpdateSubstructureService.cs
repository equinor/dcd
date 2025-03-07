using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Substructures;

public class UpdateSubstructureService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateSubstructure(Guid projectId, Guid caseId, UpdateSubstructureDto updatedSubstructureDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existing = await context.Substructures.SingleAsync(x => x.Case.ProjectId == projectPk && x.CaseId == caseId);

        existing.DryWeight = updatedSubstructureDto.DryWeight;
        existing.CostYear = updatedSubstructureDto.CostYear;
        existing.Source = updatedSubstructureDto.Source;
        existing.Concept = updatedSubstructureDto.Concept;
        existing.Maturity = updatedSubstructureDto.Maturity;
        existing.ApprovedBy = updatedSubstructureDto.ApprovedBy;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
