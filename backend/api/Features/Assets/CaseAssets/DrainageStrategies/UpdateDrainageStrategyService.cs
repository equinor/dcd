using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.DrainageStrategies;

public class UpdateDrainageStrategyService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task UpdateDrainageStrategy(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        UpdateDrainageStrategyDto updatedDrainageStrategyDto)
    {
        var existingDrainageStrategy = await context.DrainageStrategies.SingleAsync(x => x.ProjectId == projectId && x.Id == drainageStrategyId);

        existingDrainageStrategy.NGLYield = updatedDrainageStrategyDto.NGLYield;
        existingDrainageStrategy.ProducerCount = updatedDrainageStrategyDto.ProducerCount;
        existingDrainageStrategy.GasInjectorCount = updatedDrainageStrategyDto.GasInjectorCount;
        existingDrainageStrategy.WaterInjectorCount = updatedDrainageStrategyDto.WaterInjectorCount;
        existingDrainageStrategy.ArtificialLift = updatedDrainageStrategyDto.ArtificialLift;
        existingDrainageStrategy.GasSolution = updatedDrainageStrategyDto.GasSolution;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }
}
