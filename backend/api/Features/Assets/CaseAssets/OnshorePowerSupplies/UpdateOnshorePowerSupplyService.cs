using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies;

public class UpdateOnshorePowerSupplyService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateOnshorePowerSupply(Guid projectId, Guid caseId, UpdateOnshorePowerSupplyDto updatedOnshorePowerSupplyDto)
    {
        var existing = await context.OnshorePowerSupplies.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId);

        existing.CostYear = updatedOnshorePowerSupplyDto.CostYear;
        existing.DG3Date = updatedOnshorePowerSupplyDto.DG3Date;
        existing.DG4Date = updatedOnshorePowerSupplyDto.DG4Date;
        existing.Source = updatedOnshorePowerSupplyDto.Source;
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }

    public async Task UpdateOnshorePowerSupply(Guid projectId, Guid caseId, ProspUpdateOnshorePowerSupplyDto updatedOnshorePowerSupplyDto)
    {
        var existing = await context.OnshorePowerSupplies.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId);

        existing.CostYear = updatedOnshorePowerSupplyDto.CostYear;
        existing.DG3Date = updatedOnshorePowerSupplyDto.DG3Date;
        existing.DG4Date = updatedOnshorePowerSupplyDto.DG4Date;
        existing.Source = updatedOnshorePowerSupplyDto.Source;
        existing.ProspVersion = updatedOnshorePowerSupplyDto.ProspVersion;
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
