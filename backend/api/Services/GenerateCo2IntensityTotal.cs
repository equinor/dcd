using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;
using api.Services.GenerateCostProfiles;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class GenerateCo2IntensityTotal
{
    private readonly CaseService _caseService;
    private readonly ProjectService _projectService;
    private readonly ILogger<GenerateCo2IntensityTotal> _logger;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly GenerateCo2EmissionsProfile _generateCo2EmissionsProfile;

    public GenerateCo2IntensityTotal(ProjectService projectService, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<GenerateCo2IntensityTotal>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
        _generateCo2EmissionsProfile = serviceProvider.GetRequiredService<GenerateCo2EmissionsProfile>();
    }

    public Co2IntensityTotalDto Calculate(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var project = _projectService.GetProject(caseItem.ProjectId);
        var co2Intensity = 0.0;

        DrainageStrategy drainageStrategy;
        drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);

        var generateCo2EmissionsProfile = _generateCo2EmissionsProfile.Generate(caseItem.Id);
        var totalOilProduction = CalculateTotalOilProduction(caseItem, project, drainageStrategy, false);
        var totalGasProduction = CalculateTotalGasProduction(caseItem, project, drainageStrategy, false);
        var totalCo2Emissions = CalculateTotalCO2Emissions(caseItem, generateCo2EmissionsProfile);
        co2Intensity = CalculateCO2Intensity(caseItem, project, drainageStrategy, generateCo2EmissionsProfile);

        var co2IntensityTotal = new Co2IntensityTotalDto
        {
            Co2IntensityTotal = co2Intensity,
        };
        return co2IntensityTotal;
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
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
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
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
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

    private double CalculateTotalCO2Emissions(Case caseItem, Co2EmissionsDto generateCo2EmissionsProfile)
    {
        return generateCo2EmissionsProfile.Sum;
    }

    private double CalculateCO2Intensity(Case caseItem, Project project, DrainageStrategy drainageStrategy, Co2EmissionsDto generateCo2EmissionsProfile)
    {
        var tonnesToKgFactor = 1000;
        var boeConversionFactor = 6.29;
        var totalExportedVolumes = CalculateTotalExportedVolumes(caseItem, project, drainageStrategy, true);
        var totalCo2Emissions = CalculateTotalCO2Emissions(caseItem, generateCo2EmissionsProfile);
        if (totalExportedVolumes != 0 && totalCo2Emissions != 0)
        {
            return (totalCo2Emissions / totalExportedVolumes) / boeConversionFactor * tonnesToKgFactor;
        }
        return 0;
    }
}
