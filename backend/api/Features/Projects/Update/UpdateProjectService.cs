using api.Context;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Projects.Update;

public class UpdateProjectService(DcdDbContext context)
{
    public async Task UpdateProject(Guid projectId, UpdateProjectDto projectDto)
    {
        var existingProject = await context.Projects.SingleAsync(p => p.Id == projectId);

        existingProject.ModifyTime = DateTime.UtcNow;
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
        existingProject.CO2RemovedFromGas = projectDto.CO2RemovedFromGas;
        existingProject.CO2EmissionFromFuelGas = projectDto.CO2EmissionFromFuelGas;
        existingProject.FlaredGasPerProducedVolume = projectDto.FlaredGasPerProducedVolume;
        existingProject.CO2EmissionsFromFlaredGas = projectDto.CO2EmissionsFromFlaredGas;
        existingProject.CO2Vented = projectDto.CO2Vented;
        existingProject.DailyEmissionFromDrillingRig = projectDto.DailyEmissionFromDrillingRig;
        existingProject.AverageDevelopmentDrillingDays = projectDto.AverageDevelopmentDrillingDays;
        existingProject.OilPriceUSD = projectDto.OilPriceUSD;
        existingProject.GasPriceNOK = projectDto.GasPriceNOK;
        existingProject.DiscountRate = projectDto.DiscountRate;
        existingProject.ExchangeRateUSDToNOK = projectDto.ExchangeRateUSDToNOK;

        await context.SaveChangesAsync();
    }
}
