using api.Context;
using api.Context.Extensions;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts;

public class UpdateDevelopmentOperationalWellCostsService(DcdDbContext context)
{
    public async Task<DevelopmentOperationalWellCostsOverviewDto> UpdateDevelopmentOperationalWellCosts(
        Guid projectId,
        Guid developmentOperationalWellCostsId,
        UpdateDevelopmentOperationalWellCostsDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingDevelopmentOperationalWellCosts = await context.DevelopmentOperationalWellCosts.SingleAsync(x => x.Id == developmentOperationalWellCostsId);

        existingDevelopmentOperationalWellCosts.RigUpgrading = dto.RigUpgrading;
        existingDevelopmentOperationalWellCosts.RigMobDemob = dto.RigMobDemob;
        existingDevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell = dto.AnnualWellInterventionCostPerWell;
        existingDevelopmentOperationalWellCosts.PluggingAndAbandonment = dto.PluggingAndAbandonment;

        var project = await context.Projects.SingleAsync(c => c.Id == projectPk);
        project.UpdatedUtc = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return new DevelopmentOperationalWellCostsOverviewDto
        {
            DevelopmentOperationalWellCostsId = existingDevelopmentOperationalWellCosts.Id,
            RigUpgrading = existingDevelopmentOperationalWellCosts.RigUpgrading,
            RigMobDemob = existingDevelopmentOperationalWellCosts.RigMobDemob,
            AnnualWellInterventionCostPerWell = existingDevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell,
            PluggingAndAbandonment = existingDevelopmentOperationalWellCosts.PluggingAndAbandonment
        };
    }
}
