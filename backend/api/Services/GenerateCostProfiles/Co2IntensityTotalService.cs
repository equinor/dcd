using api.Dtos;
using api.Models;
using api.Repositories;

namespace api.Services.GenerateCostProfiles;

public class Co2IntensityTotalService(
    IProjectWithAssetsRepository projectWithAssetsRepository,
    ILogger<Co2IntensityTotalService> logger,
    ICaseService caseService,
    IDrainageStrategyService drainageStrategyService)
    : ICo2IntensityTotalService
{
    public async Task<double> Calculate(Guid caseId)
    {
        var caseItem = await caseService.GetCase(caseId);
        var project = await projectWithAssetsRepository.GetProjectWithCasesAndAssets(caseItem.ProjectId);
        var drainageStrategy = await drainageStrategyService.GetDrainageStrategyWithIncludes(
            caseItem.DrainageStrategyLink,
            d => d.Co2Emissions!,
            d => d.Co2EmissionsOverride!,
            d => d.ProductionProfileOil!,
            d => d.AdditionalProductionProfileOil!,
            d => d.ProductionProfileGas!,
            d => d.AdditionalProductionProfileGas!
        );

        var generateCo2EmissionsProfile = new Co2EmissionsDto();
        if (drainageStrategy.Co2EmissionsOverride?.Override == true)
        {
            generateCo2EmissionsProfile.Values = drainageStrategy.Co2EmissionsOverride.Values;
            generateCo2EmissionsProfile.StartYear = drainageStrategy.Co2EmissionsOverride.StartYear;
        }
        else
        {
            generateCo2EmissionsProfile.Values = drainageStrategy.Co2Emissions?.Values ?? [];
            generateCo2EmissionsProfile.StartYear = drainageStrategy.Co2Emissions?.StartYear ?? 0;
        }
        double co2Intensity = CalculateCO2Intensity(caseItem, project, drainageStrategy, generateCo2EmissionsProfile);
        return co2Intensity;
    }

    private double CalculateTotalOilProduction(Case caseItem, Project project, DrainageStrategy drainageStrategy, Boolean excludeOilFieldConversion)
    {
        var sumOilProduction = 0.0;
        var million = 1E6;
        var bblConversionFactor = 6.29;
        try
        {
            if (drainageStrategy.ProductionProfileOil != null)
            {
                sumOilProduction += drainageStrategy.ProductionProfileOil.Values.Sum();
            }
            if (drainageStrategy.AdditionalProductionProfileOil != null)
            {
                sumOilProduction += drainageStrategy.AdditionalProductionProfileOil.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
        }

        if (project.PhysicalUnit != 0 && !excludeOilFieldConversion)
        {
            return sumOilProduction * bblConversionFactor / million;
        }

        return sumOilProduction / million;
    }

    private double CalculateTotalGasProduction(Case caseItem, Project project, DrainageStrategy drainageStrategy, Boolean excludeOilFieldConversion)
    {
        var sumGasProduction = 0.0;
        var billion = 1E9;
        var scfConversionFactor = 35.315;
        try
        {
            if (drainageStrategy.ProductionProfileGas != null)
            {
                sumGasProduction += drainageStrategy.ProductionProfileGas.Values.Sum();
            }
            if (drainageStrategy.AdditionalProductionProfileGas != null)
            {
                sumGasProduction += drainageStrategy.AdditionalProductionProfileGas.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
        }

        if (project.PhysicalUnit != 0 && !excludeOilFieldConversion)
        {
            return sumGasProduction * scfConversionFactor / billion;
        }

        return sumGasProduction / billion;
    }

    private double CalculateTotalExportedVolumes(Case caseItem, Project project, DrainageStrategy drainageStrategy, Boolean excludeOilFieldConversion)
    {
        var oilEquivalentFactor = 5.61;
        if (project.PhysicalUnit != 0 && !excludeOilFieldConversion)
        {
            return CalculateTotalOilProduction(caseItem, project, drainageStrategy, false) + CalculateTotalGasProduction(caseItem, project, drainageStrategy, false) / oilEquivalentFactor;
        }
        return CalculateTotalOilProduction(caseItem, project, drainageStrategy, true) + CalculateTotalGasProduction(caseItem, project, drainageStrategy, true);
    }

    private double CalculateTotalCO2Emissions(Co2EmissionsDto generateCo2EmissionsProfile)
    {
        return generateCo2EmissionsProfile.Sum;
    }

    private double CalculateCO2Intensity(Case caseItem, Project project, DrainageStrategy drainageStrategy, Co2EmissionsDto generateCo2EmissionsProfile)
    {
        var tonnesToKgFactor = 1000;
        var boeConversionFactor = 6.29;
        var totalExportedVolumes = CalculateTotalExportedVolumes(caseItem, project, drainageStrategy, true);
        var totalCo2Emissions = CalculateTotalCO2Emissions(generateCo2EmissionsProfile);
        if (totalExportedVolumes > 0 && totalCo2Emissions > 0)
        {
            return (totalCo2Emissions / totalExportedVolumes) / boeConversionFactor * tonnesToKgFactor;
        }
        return 0;
    }
}
