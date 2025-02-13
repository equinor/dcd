using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Transports;

public class UpdateTransportService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateTransport(Guid projectId, Guid caseId, UpdateTransportDto updatedTransportDto)
    {
        var existing = await context.Transports.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId);

        existing.GasExportPipelineLength = updatedTransportDto.GasExportPipelineLength;
        existing.OilExportPipelineLength = updatedTransportDto.OilExportPipelineLength;
        existing.CostYear = updatedTransportDto.CostYear;
        existing.DG3Date = updatedTransportDto.DG3Date;
        existing.DG4Date = updatedTransportDto.DG4Date;
        existing.Source = updatedTransportDto.Source;
        existing.Maturity = updatedTransportDto.Maturity;
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
