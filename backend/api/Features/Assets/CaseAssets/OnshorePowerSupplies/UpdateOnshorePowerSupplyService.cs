using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies;

public class UpdateOnshorePowerSupplyService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task UpdateOnshorePowerSupply(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        UpdateOnshorePowerSupplyDto updatedOnshorePowerSupplyDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<OnshorePowerSupply>(projectId, onshorePowerSupplyId);

        var existing = await context.OnshorePowerSupplies.SingleAsync(x => x.Id == onshorePowerSupplyId);

        mapperService.MapToEntity(updatedOnshorePowerSupplyDto, existing, onshorePowerSupplyId);
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task UpdateOnshorePowerSupply(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        ProspUpdateOnshorePowerSupplyDto updatedOnshorePowerSupplyDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<OnshorePowerSupply>(projectId, onshorePowerSupplyId);

        var existing = await context.OnshorePowerSupplies.SingleAsync(x => x.Id == onshorePowerSupplyId);

        mapperService.MapToEntity(updatedOnshorePowerSupplyDto, existing, onshorePowerSupplyId);
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }
}
