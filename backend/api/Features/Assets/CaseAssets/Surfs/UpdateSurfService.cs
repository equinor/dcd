using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Surfs;

public class UpdateSurfService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateSurf(Guid projectId, Guid caseId, UpdateSurfDto updatedSurfDto)
    {
        var existingSurf = await context.Surfs.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId);

        existingSurf.CessationCost = updatedSurfDto.CessationCost;
        existingSurf.InfieldPipelineSystemLength = updatedSurfDto.InfieldPipelineSystemLength;
        existingSurf.UmbilicalSystemLength = updatedSurfDto.UmbilicalSystemLength;
        existingSurf.ArtificialLift = updatedSurfDto.ArtificialLift;
        existingSurf.RiserCount = updatedSurfDto.RiserCount;
        existingSurf.TemplateCount = updatedSurfDto.TemplateCount;
        existingSurf.ProducerCount = updatedSurfDto.ProducerCount;
        existingSurf.GasInjectorCount = updatedSurfDto.GasInjectorCount;
        existingSurf.WaterInjectorCount = updatedSurfDto.WaterInjectorCount;
        existingSurf.ProductionFlowline = updatedSurfDto.ProductionFlowline;
        existingSurf.CostYear = updatedSurfDto.CostYear;
        existingSurf.Source = updatedSurfDto.Source;
        existingSurf.ApprovedBy = updatedSurfDto.ApprovedBy;
        existingSurf.Maturity = updatedSurfDto.Maturity;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
