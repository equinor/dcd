using api.Context;
using api.Context.Extensions;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.Projects.Update;
using api.Features.TechnicalInput.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.TechnicalInput;

public class UpdateProjectAndOperationalWellsCostService(DcdDbContext context)
{
    public async Task UpdateProjectAndOperationalWellsCosts(Guid projectId, UpdateTechnicalInputDto technicalInputDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var project = await context.Projects
            .Include(x => x.ExplorationOperationalWellCosts)
            .Include(x => x.DevelopmentOperationalWellCosts)
            .SingleAsync(x => x.Id == projectPk);

        UpdateProject(project, technicalInputDto.ProjectDto);
        UpdateDevelopmentOperationalWellCosts(project.DevelopmentOperationalWellCosts!, technicalInputDto.DevelopmentOperationalWellCostsDto);
        UpdateExplorationOperationalWellCosts(project.ExplorationOperationalWellCosts!, technicalInputDto.ExplorationOperationalWellCostsDto);

        await context.SaveChangesAsync();
    }

    private void UpdateProject(Project project, UpdateProjectDto dto)
    {
        project.Name = dto.Name;
        project.ReferenceCaseId = dto.ReferenceCaseId;
        project.Description = dto.Description;
        project.Country = dto.Country;
        project.Currency = dto.Currency;
        project.PhysicalUnit = dto.PhysicalUnit;
        project.Classification = dto.Classification;
        project.ProjectPhase = dto.ProjectPhase;
        project.InternalProjectPhase = dto.InternalProjectPhase;
        project.ProjectCategory = dto.ProjectCategory;
        project.SharepointSiteUrl = dto.SharepointSiteUrl;
        project.CO2RemovedFromGas = dto.CO2RemovedFromGas;
        project.CO2EmissionFromFuelGas = dto.CO2EmissionFromFuelGas;
        project.FlaredGasPerProducedVolume = dto.FlaredGasPerProducedVolume;
        project.CO2EmissionsFromFlaredGas = dto.CO2EmissionsFromFlaredGas;
        project.CO2Vented = dto.CO2Vented;
        project.DailyEmissionFromDrillingRig = dto.DailyEmissionFromDrillingRig;
        project.AverageDevelopmentDrillingDays = dto.AverageDevelopmentDrillingDays;
        project.OilPriceUSD = dto.OilPriceUSD;
        project.GasPriceNOK = dto.GasPriceNOK;
        project.DiscountRate = dto.DiscountRate;
        project.ExchangeRateUSDToNOK = dto.ExchangeRateUSDToNOK;
        project.ModifyTime = DateTime.UtcNow;
    }

    private void UpdateDevelopmentOperationalWellCosts(DevelopmentOperationalWellCosts item, UpdateDevelopmentOperationalWellCostsDto dto)
    {
        item.RigUpgrading = dto.RigUpgrading;
        item.RigMobDemob = dto.RigMobDemob;
        item.AnnualWellInterventionCostPerWell = dto.AnnualWellInterventionCostPerWell;
        item.PluggingAndAbandonment = dto.PluggingAndAbandonment;
    }

    private void UpdateExplorationOperationalWellCosts(ExplorationOperationalWellCosts item, UpdateExplorationOperationalWellCostsDto dto)
    {
        item.ExplorationRigUpgrading = dto.ExplorationRigUpgrading;
        item.ExplorationRigMobDemob = dto.ExplorationRigMobDemob;
        item.ExplorationProjectDrillingCosts = dto.ExplorationProjectDrillingCosts;
        item.AppraisalRigMobDemob = dto.AppraisalRigMobDemob;
        item.AppraisalProjectDrillingCosts = dto.AppraisalProjectDrillingCosts;
    }
}
