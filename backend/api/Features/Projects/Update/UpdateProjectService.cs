using api.AppInfrastructure.Authorization;
using api.Context;
using api.Context.Extensions;
using api.Models;
using api.Models.Infrastructure.ProjectRecalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Projects.Update;

public class UpdateProjectService(DcdDbContext context, CurrentUser? currentUser)
{
    public async Task UpdateProject(Guid projectId, UpdateProjectDto projectDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingProject = await context.Projects
            .Include(x => x.ProjectMembers)
            .SingleAsync(p => p.Id == projectPk);

        var shouldTriggerRecalculation = ShouldTriggerRecalculation(existingProject, projectDto);

        ProjectClassificationHelper.AddCurrentUserAsEditorIfClassificationBecomesMoreStrict(
            existingProject,
            projectDto.Classification,
            currentUser);

        existingProject.Name = projectDto.Name;
        existingProject.ReferenceCaseId = projectDto.ReferenceCaseId;
        existingProject.Description = projectDto.Description;
        existingProject.Country = projectDto.Country;
        existingProject.Currency = projectDto.Currency;
        existingProject.PhysicalUnit = projectDto.PhysicalUnit;
        existingProject.Classification = projectDto.Classification;
        existingProject.ProjectPhase = projectDto.ProjectPhase;
        existingProject.InternalProjectPhase = projectDto.InternalProjectPhase;
        existingProject.ProjectCategory = projectDto.ProjectCategory;
        existingProject.SharepointSiteUrl = projectDto.SharepointSiteUrl;
        existingProject.Co2RemovedFromGas = projectDto.Co2RemovedFromGas;
        existingProject.Co2EmissionFromFuelGas = projectDto.Co2EmissionFromFuelGas;
        existingProject.FlaredGasPerProducedVolume = projectDto.FlaredGasPerProducedVolume;
        existingProject.Co2EmissionsFromFlaredGas = projectDto.Co2EmissionsFromFlaredGas;
        existingProject.Co2Vented = projectDto.Co2Vented;
        existingProject.DailyEmissionFromDrillingRig = projectDto.DailyEmissionFromDrillingRig;
        existingProject.AverageDevelopmentDrillingDays = projectDto.AverageDevelopmentDrillingDays;
        existingProject.OilPriceUsd = projectDto.OilPriceUsd;
        existingProject.GasPriceNok = projectDto.GasPriceNok;
        existingProject.NglPriceUsd = projectDto.NglPriceUsd;
        existingProject.DiscountRate = projectDto.DiscountRate;
        existingProject.ExchangeRateUsdToNok = projectDto.ExchangeRateUsdToNok;
        existingProject.NpvYear = projectDto.NpvYear;

        if (shouldTriggerRecalculation)
        {
            context.PendingRecalculations.Add(new PendingRecalculation
            {
                ProjectId = existingProject.Id,
                CreatedUtc = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();
    }

    private static bool ShouldTriggerRecalculation(Project existingProject, UpdateProjectDto projectDto)
    {
        if (existingProject.ExchangeRateUsdToNok != projectDto.ExchangeRateUsdToNok)
        {
            return true;
        }

        if (existingProject.NpvYear != projectDto.NpvYear)
        {
            return true;
        }

        if (existingProject.DiscountRate != projectDto.DiscountRate)
        {
            return true;
        }

        if (existingProject.GasPriceNok != projectDto.GasPriceNok)
        {
            return true;
        }

        if (existingProject.NglPriceUsd != projectDto.NglPriceUsd)
        {
            return true;
        }

        if (existingProject.OilPriceUsd != projectDto.OilPriceUsd)
        {
            return true;
        }

        if (existingProject.AverageDevelopmentDrillingDays != projectDto.AverageDevelopmentDrillingDays)
        {
            return true;
        }

        if (existingProject.DailyEmissionFromDrillingRig != projectDto.DailyEmissionFromDrillingRig)
        {
            return true;
        }

        if (existingProject.Co2EmissionFromFuelGas != projectDto.Co2EmissionFromFuelGas)
        {
            return true;
        }

        if (existingProject.Co2EmissionsFromFlaredGas != projectDto.Co2EmissionsFromFlaredGas)
        {
            return true;
        }

        if (existingProject.Co2Vented != projectDto.Co2Vented)
        {
            return true;
        }

        return false;
    }
}
