using api.Context;
using api.Context.Extensions;
using api.Features.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Surfs;

public class UpdateSurfService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateSurf(Guid projectId, Guid caseId, UpdateSurfDto updatedSurfDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existing = await context.Surfs.SingleAsync(x => x.Case.ProjectId == projectPk && x.CaseId == caseId);

        existing.CessationCost = updatedSurfDto.CessationCost;
        existing.InfieldPipelineSystemLength = updatedSurfDto.InfieldPipelineSystemLength;
        existing.UmbilicalSystemLength = updatedSurfDto.UmbilicalSystemLength;
        existing.ArtificialLift = updatedSurfDto.ArtificialLift;
        existing.RiserCount = updatedSurfDto.RiserCount;
        existing.TemplateCount = updatedSurfDto.TemplateCount;
        existing.ProducerCount = updatedSurfDto.ProducerCount;
        existing.GasInjectorCount = updatedSurfDto.GasInjectorCount;
        existing.WaterInjectorCount = updatedSurfDto.WaterInjectorCount;
        existing.ProductionFlowline = updatedSurfDto.ProductionFlowline;
        existing.CostYear = updatedSurfDto.CostYear;
        existing.Source = updatedSurfDto.Source;
        existing.ApprovedBy = updatedSurfDto.ApprovedBy;
        existing.Maturity = updatedSurfDto.Maturity;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
