using api.Context;
using api.Context.Extensions;
using api.Features.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.DrainageStrategies;

public class UpdateDrainageStrategyService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateDrainageStrategy(Guid projectId, Guid caseId, UpdateDrainageStrategyDto updatedDrainageStrategyDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existing = await context.DrainageStrategies.SingleAsync(x => x.Case.ProjectId == projectPk && x.CaseId == caseId);

        existing.NglYield = updatedDrainageStrategyDto.NglYield;
        existing.CondensateYield = updatedDrainageStrategyDto.CondensateYield;
        existing.GasShrinkageFactor = updatedDrainageStrategyDto.GasShrinkageFactor;
        existing.ProducerCount = updatedDrainageStrategyDto.ProducerCount;
        existing.GasInjectorCount = updatedDrainageStrategyDto.GasInjectorCount;
        existing.WaterInjectorCount = updatedDrainageStrategyDto.WaterInjectorCount;
        existing.ArtificialLift = updatedDrainageStrategyDto.ArtificialLift;
        existing.GasSolution = updatedDrainageStrategyDto.GasSolution;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
