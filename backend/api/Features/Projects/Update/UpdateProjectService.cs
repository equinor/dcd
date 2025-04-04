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
        const double tolerance = 0.000000001;

        return Math.Abs(existingProject.ExchangeRateUsdToNok - projectDto.ExchangeRateUsdToNok) > tolerance
               || Math.Abs(existingProject.DiscountRate - projectDto.DiscountRate) > tolerance
               || Math.Abs(existingProject.GasPriceNok - projectDto.GasPriceNok) > tolerance
               || Math.Abs(existingProject.NglPriceUsd - projectDto.NglPriceUsd) > tolerance
               || Math.Abs(existingProject.OilPriceUsd - projectDto.OilPriceUsd) > tolerance
               || existingProject.NpvYear != projectDto.NpvYear;
    }
}
