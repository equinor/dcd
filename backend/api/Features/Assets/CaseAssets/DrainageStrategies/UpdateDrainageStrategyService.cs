using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.DrainageStrategies;

public class UpdateDrainageStrategyService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateDrainageStrategy(Guid projectId, Guid caseId, UpdateDrainageStrategyDto updatedDrainageStrategyDto)
    {
        var existingDrainageStrategy = await context.DrainageStrategies.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId);

        existingDrainageStrategy.NGLYield = updatedDrainageStrategyDto.NGLYield;
        existingDrainageStrategy.CondensateYield = updatedDrainageStrategyDto.CondensateYield;
        existingDrainageStrategy.GasShrinkageFactor = updatedDrainageStrategyDto.GasShrinkageFactor;
        existingDrainageStrategy.ProducerCount = updatedDrainageStrategyDto.ProducerCount;
        existingDrainageStrategy.GasInjectorCount = updatedDrainageStrategyDto.GasInjectorCount;
        existingDrainageStrategy.WaterInjectorCount = updatedDrainageStrategyDto.WaterInjectorCount;
        existingDrainageStrategy.ArtificialLift = updatedDrainageStrategyDto.ArtificialLift;
        existingDrainageStrategy.GasSolution = updatedDrainageStrategyDto.GasSolution;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
