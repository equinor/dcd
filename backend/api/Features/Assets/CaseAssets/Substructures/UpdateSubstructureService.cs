using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Substructures;

public class UpdateSubstructureService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task UpdateSubstructure(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureDto updatedSubstructureDto)
    {
        var existingSubstructure = await context.Substructures.SingleAsync(x => x.ProjectId == projectId && x.Id == substructureId);

        existingSubstructure.DryWeight = updatedSubstructureDto.DryWeight;
        existingSubstructure.Currency = updatedSubstructureDto.Currency;
        existingSubstructure.CostYear = updatedSubstructureDto.CostYear;
        existingSubstructure.Source = updatedSubstructureDto.Source;
        existingSubstructure.Concept = updatedSubstructureDto.Concept;
        existingSubstructure.DG3Date = updatedSubstructureDto.DG3Date;
        existingSubstructure.DG4Date = updatedSubstructureDto.DG4Date;
        existingSubstructure.Maturity = updatedSubstructureDto.Maturity;
        existingSubstructure.ApprovedBy = updatedSubstructureDto.ApprovedBy;

        existingSubstructure.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task UpdateSubstructure(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        ProspUpdateSubstructureDto updatedSubstructureDto)
    {
        var existingSubstructure = await context.Substructures.SingleAsync(x => x.ProjectId == projectId && x.Id == substructureId);

        existingSubstructure.DryWeight = updatedSubstructureDto.DryWeight;
        existingSubstructure.Currency = updatedSubstructureDto.Currency;
        existingSubstructure.CostYear = updatedSubstructureDto.CostYear;
        existingSubstructure.Source = updatedSubstructureDto.Source;
        existingSubstructure.Concept = updatedSubstructureDto.Concept;
        existingSubstructure.DG3Date = updatedSubstructureDto.DG3Date;
        existingSubstructure.DG4Date = updatedSubstructureDto.DG4Date;
        existingSubstructure.ProspVersion = updatedSubstructureDto.ProspVersion;

        existingSubstructure.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }
}
