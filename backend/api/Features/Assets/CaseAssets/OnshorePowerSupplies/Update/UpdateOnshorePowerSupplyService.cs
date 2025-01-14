using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Profiles.Dtos;
using api.Features.Cases.Recalculation;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Update;

public class UpdateOnshorePowerSupplyService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task ResetOnshorePowerSupply(Guid projectId, Guid caseId, Guid onshorePowerSupplyId)
    {
        var existing = await context.OnshorePowerSupplies.SingleAsync(x => x.ProjectId == projectId && x.Id == onshorePowerSupplyId);

        existing.CostYear = 0;
        existing.DG3Date = null;
        existing.DG4Date = null;
        existing.ProspVersion = null;
        existing.Source = Source.ConceptApp;
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task<OnshorePowerSupplyDto> UpdateOnshorePowerSupply(Guid projectId, Guid caseId, Guid onshorePowerSupplyId, UpdateOnshorePowerSupplyDto dto)
    {
        var existing = await context.OnshorePowerSupplies.SingleAsync(x => x.ProjectId == projectId && x.Id == onshorePowerSupplyId);

        existing.CostYear = dto.CostYear;
        existing.DG3Date = dto.DG3Date;
        existing.DG4Date = dto.DG4Date;
        existing.Source = dto.Source;
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new OnshorePowerSupplyDto
        {
            Id = existing.Id,
            Name = existing.Name,
            ProjectId = existing.ProjectId,
            LastChangedDate = existing.LastChangedDate,
            CostYear = existing.CostYear,
            Source = existing.Source,
            ProspVersion = existing.ProspVersion,
            DG3Date = existing.DG3Date,
            DG4Date = existing.DG4Date
        };
    }
}
