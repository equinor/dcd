using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Explorations;

public class UpdateExplorationService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateExploration(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        UpdateExplorationDto updatedExplorationDto)
    {
        var existingExploration = await context.Explorations.SingleAsync(x => x.Case.ProjectId == projectId && x.Id == explorationId);

        existingExploration.RigMobDemob = updatedExplorationDto.RigMobDemob;
        existingExploration.Currency = updatedExplorationDto.Currency;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
