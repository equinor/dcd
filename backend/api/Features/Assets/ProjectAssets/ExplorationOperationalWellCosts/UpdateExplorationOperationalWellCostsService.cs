using api.Context;
using api.Context.Extensions;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts;

public class UpdateExplorationOperationalWellCostsService(DcdDbContext context)
{
    public async Task<ExplorationOperationalWellCostsOverviewDto> UpdateExplorationOperationalWellCosts(
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

        await context.UpdateProjectUpdatedUtc(projectPk);
        await context.SaveChangesAsync();

        return new ExplorationOperationalWellCostsOverviewDto
        {
            ProjectId = existingExplorationOperationalWellCosts.ProjectId,
            ExplorationOperationalWellCostsId = existingExplorationOperationalWellCosts.Id,
            ExplorationRigUpgrading = existingExplorationOperationalWellCosts.ExplorationRigUpgrading,
            ExplorationRigMobDemob = existingExplorationOperationalWellCosts.ExplorationRigMobDemob,
            ExplorationProjectDrillingCosts = existingExplorationOperationalWellCosts.ExplorationProjectDrillingCosts,
            AppraisalRigMobDemob = existingExplorationOperationalWellCosts.AppraisalRigMobDemob,
            AppraisalProjectDrillingCosts = existingExplorationOperationalWellCosts.AppraisalProjectDrillingCosts
        };
    }
}
