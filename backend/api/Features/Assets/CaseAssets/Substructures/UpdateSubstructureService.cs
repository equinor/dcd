using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Substructures;

public class UpdateSubstructureService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateSubstructure(Guid projectId, Guid caseId, UpdateSubstructureDto updatedSubstructureDto)
    {
        var existingSubstructure = await context.Substructures.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId);

        existingSubstructure.DryWeight = updatedSubstructureDto.DryWeight;
        existingSubstructure.CostYear = updatedSubstructureDto.CostYear;
        existingSubstructure.Source = updatedSubstructureDto.Source;
        existingSubstructure.Concept = updatedSubstructureDto.Concept;
        existingSubstructure.DG3Date = updatedSubstructureDto.DG3Date;
        existingSubstructure.DG4Date = updatedSubstructureDto.DG4Date;
        existingSubstructure.Maturity = updatedSubstructureDto.Maturity;
        existingSubstructure.ApprovedBy = updatedSubstructureDto.ApprovedBy;

        existingSubstructure.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
