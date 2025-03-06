using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies;

public class UpdateOnshorePowerSupplyService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateOnshorePowerSupply(Guid projectId, Guid caseId, UpdateOnshorePowerSupplyDto updatedOnshorePowerSupplyDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existing = await context.OnshorePowerSupplies.SingleAsync(x => x.Case.ProjectId == projectPk && x.CaseId == caseId);

        existing.CostYear = updatedOnshorePowerSupplyDto.CostYear;
        existing.Source = updatedOnshorePowerSupplyDto.Source;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
