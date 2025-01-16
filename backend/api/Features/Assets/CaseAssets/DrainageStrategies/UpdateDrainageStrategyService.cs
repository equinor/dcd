using api.Context;
using api.Context.Extensions;
using api.Features.Cases.GetWithAssets;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.DrainageStrategies;

public class UpdateDrainageStrategyService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<DrainageStrategyDto> UpdateDrainageStrategy(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        UpdateDrainageStrategyDto updatedDrainageStrategyDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<DrainageStrategy>(projectId, drainageStrategyId);

        var existingDrainageStrategy = await context.DrainageStrategies.SingleAsync(x => x.Id == drainageStrategyId);

        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var project = await context.Projects.SingleAsync(p => p.Id == projectPk);

        conversionMapperService.MapToEntity(updatedDrainageStrategyDto, existingDrainageStrategy, drainageStrategyId, project.PhysicalUnit);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return conversionMapperService.MapToDto<DrainageStrategy, DrainageStrategyDto>(existingDrainageStrategy, drainageStrategyId, project.PhysicalUnit);
    }
}
