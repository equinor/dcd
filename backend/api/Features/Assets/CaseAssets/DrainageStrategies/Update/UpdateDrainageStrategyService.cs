using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.DrainageStrategies.Profiles.Dtos;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.DrainageStrategies.Update;

public class UpdateDrainageStrategyService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task<DrainageStrategyDto> UpdateDrainageStrategy(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        UpdateDrainageStrategyDto dto)
    {
        var existingDrainageStrategy = await context.DrainageStrategies.SingleAsync(x => x.ProjectId == projectId && x.Id == drainageStrategyId);

        existingDrainageStrategy.NGLYield = dto.NGLYield;
        existingDrainageStrategy.ProducerCount = dto.ProducerCount;
        existingDrainageStrategy.GasInjectorCount = dto.GasInjectorCount;
        existingDrainageStrategy.WaterInjectorCount = dto.WaterInjectorCount;
        existingDrainageStrategy.ArtificialLift = dto.ArtificialLift;
        existingDrainageStrategy.GasSolution = dto.GasSolution;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new DrainageStrategyDto
        {
            Id = existingDrainageStrategy.Id,
            ProjectId = existingDrainageStrategy.ProjectId,
            Name = existingDrainageStrategy.Name,
            Description = existingDrainageStrategy.Description,
            NGLYield = existingDrainageStrategy.NGLYield,
            ProducerCount = existingDrainageStrategy.ProducerCount,
            GasInjectorCount = existingDrainageStrategy.GasInjectorCount,
            WaterInjectorCount = existingDrainageStrategy.WaterInjectorCount,
            ArtificialLift = existingDrainageStrategy.ArtificialLift,
            GasSolution = existingDrainageStrategy.GasSolution
        };
    }
}
