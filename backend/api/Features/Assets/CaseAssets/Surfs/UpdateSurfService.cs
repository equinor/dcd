using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Surfs;

public class UpdateSurfService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task UpdateSurf(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        UpdateSurfDto updatedSurfDto)
    {
        var existingSurf = await context.Surfs.SingleAsync(x => x.ProjectId == projectId && x.Id == surfId);

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
        existingSurf.Currency = updatedSurfDto.Currency;
        existingSurf.CostYear = updatedSurfDto.CostYear;
        existingSurf.Source = updatedSurfDto.Source;
        existingSurf.ApprovedBy = updatedSurfDto.ApprovedBy;
        existingSurf.DG3Date = updatedSurfDto.DG3Date;
        existingSurf.DG4Date = updatedSurfDto.DG4Date;
        existingSurf.Maturity = updatedSurfDto.Maturity;
        existingSurf.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task UpdateSurf(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        ProspUpdateSurfDto updatedSurfDto)
    {
        var existingSurf = await context.Surfs.SingleAsync(x => x.ProjectId == projectId && x.Id == surfId);

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
        existingSurf.Currency = updatedSurfDto.Currency;
        existingSurf.CostYear = updatedSurfDto.CostYear;
        existingSurf.Source = updatedSurfDto.Source;
        existingSurf.ApprovedBy = updatedSurfDto.ApprovedBy;
        existingSurf.DG3Date = updatedSurfDto.DG3Date;
        existingSurf.DG4Date = updatedSurfDto.DG4Date;
        existingSurf.ProspVersion = updatedSurfDto.ProspVersion;
        existingSurf.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }
}
