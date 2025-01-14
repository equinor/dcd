using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Substructures.Update;

public class SubstructureService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task ResetSubstructure(Guid projectId, Guid caseId, Guid substructureId)
    {
        var existingSubstructure = await context.Substructures.SingleAsync(x =>  x.ProjectId == projectId && x.Id == substructureId);

        existingSubstructure.Source = Source.ConceptApp;
        existingSubstructure.Currency = Currency.NOK;
        existingSubstructure.Concept = Concept.NO_CONCEPT;
        existingSubstructure.DryWeight = 0;
        existingSubstructure.CostYear = 0;
        existingSubstructure.DG3Date = null;
        existingSubstructure.DG4Date = null;
        existingSubstructure.ProspVersion = null;
        existingSubstructure.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task UpdateSubstructureFromProsp(Guid projectId, Guid caseId, Guid substructureId, ProspUpdateSubstructureDto dto)
    {
        var existingSubstructure = await context.Substructures.SingleAsync(x =>  x.ProjectId == projectId && x.Id == substructureId);

        existingSubstructure.Source = dto.Source;
        existingSubstructure.DryWeight = dto.DryWeight;
        existingSubstructure.Currency = dto.Currency;
        existingSubstructure.CostYear = dto.CostYear;
        existingSubstructure.Concept = dto.Concept;
        existingSubstructure.DG3Date = dto.DG3Date;
        existingSubstructure.DG4Date = dto.DG4Date;
        existingSubstructure.ProspVersion = dto.ProspVersion;
        existingSubstructure.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task<SubstructureDto> UpdateSubstructure(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureDto dto)
    {
        var existingSubstructure = await context.Substructures.SingleAsync(x =>  x.ProjectId == projectId && x.Id == substructureId);

        existingSubstructure.DryWeight = dto.DryWeight;
        existingSubstructure.Currency = dto.Currency;
        existingSubstructure.CostYear = dto.CostYear;
        existingSubstructure.Source = dto.Source;
        existingSubstructure.Concept = dto.Concept;
        existingSubstructure.DG3Date = dto.DG3Date;
        existingSubstructure.DG4Date = dto.DG4Date;
        existingSubstructure.Maturity = dto.Maturity;
        existingSubstructure.ApprovedBy = dto.ApprovedBy;
        existingSubstructure.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new SubstructureDto
        {
            Id = existingSubstructure.Id,
            Name = existingSubstructure.Name,
            ProjectId = existingSubstructure.ProjectId,
            DryWeight = existingSubstructure.DryWeight,
            Maturity = existingSubstructure.Maturity,
            Currency = existingSubstructure.Currency,
            ApprovedBy = existingSubstructure.ApprovedBy,
            CostYear = existingSubstructure.CostYear,
            ProspVersion = existingSubstructure.ProspVersion,
            Source = existingSubstructure.Source,
            LastChangedDate = existingSubstructure.LastChangedDate,
            Concept = existingSubstructure.Concept,
            DG3Date = existingSubstructure.DG3Date,
            DG4Date = existingSubstructure.DG4Date
        };
    }
}
