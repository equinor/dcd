using api.Context;
using api.Exceptions;
using api.Features.BackgroundServices.ProjectMaster.Services.EnumConverters;
using api.Features.FusionIntegration.ProjectMaster;
using api.Models;
using api.Models.Enums;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Projects.Create;

public class CreateProjectService(DcdDbContext context, IFusionService fusionService)
{
    public async Task<Guid> CreateProject(Guid contextId)
    {
        var projectMaster = await fusionService.GetProjectMasterFromFusionContextId(contextId);

        if (projectMaster == null)
        {
            throw new NotFoundInDbException($"Project with context ID {contextId} not found in the external API.");
        }

        var existingProject = await context.Projects.FirstOrDefaultAsync(p => p.FusionProjectId == projectMaster.Identity);

        if (existingProject != null)
        {
            throw new ResourceAlreadyExistsException($"Project with externalId {projectMaster.Identity} already exists");
        }

        var project = new Project
        {
            Name = projectMaster.Name ?? projectMaster.Description ?? "",
            FusionProjectId = projectMaster.Identity,
            Description = projectMaster.Description ?? "",
            ProjectPhase = ProjectPhaseEnumConverter.ConvertPhase(projectMaster.Phase),
            Country = projectMaster.Country ?? "",
            ProjectCategory = ProjectCategoryEnumConverter.ConvertCategory(projectMaster.ProjectCategory),
            CommonLibraryName = "",
            ExplorationOperationalWellCosts = new ExplorationOperationalWellCosts
            {
                ExplorationRigUpgrading = 0,
                ExplorationRigMobDemob = 0,
                ExplorationProjectDrillingCosts = 0,
                AppraisalRigMobDemob = 0,
                AppraisalProjectDrillingCosts = 0
            },
            DevelopmentOperationalWellCosts = new DevelopmentOperationalWellCosts
            {
                RigUpgrading = 0,
                RigMobDemob = 0,
                AnnualWellInterventionCostPerWell = 0,
                PluggingAndAbandonment = 0
            },
            CO2EmissionFromFuelGas = 2.34,
            FlaredGasPerProducedVolume = 1.122765,
            CO2EmissionsFromFlaredGas = 3.73,
            CO2Vented = 1.96,
            DailyEmissionFromDrillingRig = 100,
            AverageDevelopmentDrillingDays = 50,
            Classification = ProjectClassification.Internal,
            DiscountRate = 8.0,
            OilPriceUsd = 75.0,
            GasPriceNok = 3.0,
            NglPriceUsd = 0.0,
            ExchangeRateUsdToNok = 10.0,
            NpvYear = DateTime.UtcNow.Year,
            Currency = Currency.Nok
        };

        context.Projects.Add(project);

        await context.SaveChangesAsync();

        return project.Id;
    }
}
