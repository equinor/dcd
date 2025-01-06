using api.Context;
using api.Context.Extensions;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts;

public class UpdateExplorationOperationalWellCostsService(DcdDbContext context)
{
    public async Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts(
        Guid projectId,
        Guid explorationOperationalWellCostsId,
        UpdateExplorationOperationalWellCostsDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingExplorationOperationalWellCosts = await context.ExplorationOperationalWellCosts.SingleAsync(x => x.Id == explorationOperationalWellCostsId);

        existingExplorationOperationalWellCosts.ExplorationRigUpgrading = dto.ExplorationRigUpgrading;
        existingExplorationOperationalWellCosts.ExplorationRigMobDemob = dto.ExplorationRigMobDemob;
        existingExplorationOperationalWellCosts.ExplorationProjectDrillingCosts = dto.ExplorationProjectDrillingCosts;
        existingExplorationOperationalWellCosts.AppraisalRigMobDemob = dto.AppraisalRigMobDemob;
        existingExplorationOperationalWellCosts.AppraisalProjectDrillingCosts = dto.AppraisalProjectDrillingCosts;

        var project = await context.Projects.SingleAsync(c => c.Id == projectPk);
        project.ModifyTime = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return new ExplorationOperationalWellCostsDto
        {
            Id = existingExplorationOperationalWellCosts.Id,
            ProjectId = existingExplorationOperationalWellCosts.ProjectId,
            ExplorationRigUpgrading = existingExplorationOperationalWellCosts.ExplorationRigUpgrading,
            ExplorationRigMobDemob = existingExplorationOperationalWellCosts.ExplorationRigMobDemob,
            ExplorationProjectDrillingCosts = existingExplorationOperationalWellCosts.ExplorationProjectDrillingCosts,
            AppraisalRigMobDemob = existingExplorationOperationalWellCosts.AppraisalRigMobDemob,
            AppraisalProjectDrillingCosts = existingExplorationOperationalWellCosts.AppraisalProjectDrillingCosts
        };
    }
}
