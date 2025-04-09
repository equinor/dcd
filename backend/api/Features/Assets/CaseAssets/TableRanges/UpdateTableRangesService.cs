using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.TableRanges;

public class UpdateTableRangesService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateTableRanges(Guid projectId, Guid caseId, UpdateTableRangesDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existing = await context.Cases.SingleAsync(x => x.ProjectId == projectPk && x.Id == caseId);

        existing.Co2EmissionsYears = dto.Co2EmissionsYears;
        existing.DrillingScheduleYears = dto.DrillingScheduleYears;
        existing.CaseCostYears = dto.CaseCostYears;
        existing.ProductionProfilesYears = dto.ProductionProfilesYears;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
