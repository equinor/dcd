using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;
using api.Services.GenerateCostProfiles;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CompareCasesService
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;
    private readonly ILogger<CompareCasesService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly CaseService _caseService;
    private readonly ExplorationService _explorationService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly GenerateCessationCostProfile _generateCessationCostProfile;
    private readonly GenerateCo2EmissionsProfile _generateCo2EmissionsProfile;
    private readonly GenerateGAndGAdminCostProfile _generateGAndGAdminCostProfile;
    private readonly GenerateOpexCostProfile _generateOpexCostProfile;
    private readonly GenerateStudyCostProfile _generateStudyCostProfile;

    public CompareCasesService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _context = context;
        _projectService = projectService;
        _serviceProvider = serviceProvider;
        _logger = loggerFactory.CreateLogger<CompareCasesService>();
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _explorationService = serviceProvider.GetRequiredService<ExplorationService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();

        _generateGAndGAdminCostProfile = serviceProvider.GetRequiredService<GenerateGAndGAdminCostProfile>();
        _generateStudyCostProfile = serviceProvider.GetRequiredService<GenerateStudyCostProfile>();
        _generateOpexCostProfile = serviceProvider.GetRequiredService<GenerateOpexCostProfile>();
        _generateCessationCostProfile = serviceProvider.GetRequiredService<GenerateCessationCostProfile>();
        _generateCo2EmissionsProfile = serviceProvider.GetRequiredService<GenerateCo2EmissionsProfile>();
    }

    public IEnumerable<CompareCasesDto> Calculate(Guid projectId)
    {
        var project = _projectService.GetProject(projectId);
        var caseList = new List<CompareCasesDto>();
        if (project.Cases != null)
        {
            foreach (var caseItem in project.Cases)
            {
                var totalOilProduction = CalculateTotalOilProduction(caseItem.Id);
                var totalGasProduction = CalculateTotalGasProduction(caseItem.Id);
                var totalExportedVolumes = CalculateTotalExportedVolumes(caseItem.Id);
                var totalStudyCostsPlusOpex = CalculateTotalStudyCostsPlusOpex(caseItem.Id);
                var totalCessationCosts = CalculateTotalCessationCosts(caseItem.Id);
                var offshorePlusOnshoreFacilityCosts = CalculateOffshorePlusOnshoreFacilityCosts(caseItem.Id);
                var developmentCosts = CalculateDevelopmentWellCosts(caseItem.Id);
                var explorationCosts = CalculateExplorationWellCosts(caseItem.Id);
                var totalCo2Emissions = CalculateTotalCO2Emissions(caseItem.Id);
                var co2Intensity = CalculateCO2Intensity(caseItem.Id);

                var compareCases = new CompareCasesDto
                {
                    TotalOilProduction = totalOilProduction,
                    TotalGasProduction = totalGasProduction,
                    TotalExportedVolumes = totalExportedVolumes,
                    TotalStudyCostsPlusOpex = totalStudyCostsPlusOpex,
                    TotalCessationCosts = totalCessationCosts,
                    OffshorePlusOnshoreFacilityCosts = offshorePlusOnshoreFacilityCosts,
                    DevelopmentWellCosts = developmentCosts,
                    ExplorationWellCosts = explorationCosts,
                    TotalCo2Emissions = totalCo2Emissions,
                    Co2Intensity = co2Intensity,
                };
                caseList.Add(compareCases);
            }
        }
        return caseList;
    }

    public double CalculateTotalOilProduction(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var sumOilProduction = 0.0;

        DrainageStrategy drainageStrategy;
        try
        {
            drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
            if (drainageStrategy.ProductionProfileOil != null)
            {
                sumOilProduction += drainageStrategy.ProductionProfileOil.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
        }

        return sumOilProduction;
    }

    public double CalculateTotalGasProduction(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var sumGasProduction = 0.0;

        DrainageStrategy drainageStrategy;
        try
        {
            drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
            if (drainageStrategy.ProductionProfileGas != null)
            {
                sumGasProduction += drainageStrategy.ProductionProfileGas.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
        }

        return sumGasProduction;
    }

    public double CalculateTotalExportedVolumes(Guid caseId)
    {
        return CalculateTotalOilProduction(caseId) + CalculateTotalGasProduction(caseId);
    }

    public double CalculateTotalStudyCostsPlusOpex(Guid caseId)
    {
        var generateStudyProfile = _generateStudyCostProfile.Generate(caseId);
        var generateOpexProfile = _generateOpexCostProfile.Generate(caseId);
        var sumStudyValues = generateStudyProfile.Sum;
        var sumOpexValues = generateOpexProfile.Sum;

        return sumStudyValues + sumOpexValues;
    }

    public double CalculateTotalCessationCosts(Guid caseId)
    {
        var generateCessationProfile = _generateCessationCostProfile.Generate(caseId);
        return generateCessationProfile.Sum;
    }

    public double CalculateOffshorePlusOnshoreFacilityCosts(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        return _generateStudyCostProfile.SumAllCostFacility(caseItem);
    }

    public double CalculateDevelopmentWellCosts(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        return _generateStudyCostProfile.SumWellCost(caseItem);
    }

    public double CalculateExplorationWellCosts(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var sumExplorationWellCost = 0.0;
        var generateGAndGAdminProfile = _generateGAndGAdminCostProfile.Generate(caseId);
        sumExplorationWellCost += generateGAndGAdminProfile.Sum;

        Exploration exploration;
        try
        {
            exploration = _explorationService.GetExploration(caseItem.ExplorationLink);

            if (exploration?.CountryOfficeCost != null)
            {
                sumExplorationWellCost += exploration.CountryOfficeCost.Values.Sum();
            }
            if (exploration?.SeismicAcquisitionAndProcessing != null)
            {
                sumExplorationWellCost += exploration.SeismicAcquisitionAndProcessing.Values.Sum();
            }
            if (exploration?.ExplorationWellCostProfile != null)
            {
                sumExplorationWellCost += exploration.ExplorationWellCostProfile.Values.Sum();
            }
            if (exploration?.AppraisalWellCostProfile != null)
            {
                sumExplorationWellCost += exploration.AppraisalWellCostProfile.Values.Sum();
            }
            if (exploration?.SidetrackCostProfile != null)
            {
                sumExplorationWellCost += exploration.SidetrackCostProfile.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Exploration {0} not found.", caseItem.ExplorationLink);
        }

        return sumExplorationWellCost;
    }

    public double CalculateTotalCO2Emissions(Guid caseId)
    {
        var generateCo2EmissionsProfile = _generateCo2EmissionsProfile.Generate(caseId);
        return generateCo2EmissionsProfile.Sum;
    }

    public double CalculateCO2Intensity(Guid caseId)
    {
        var totalOilProductionInGasEquivalent = CalculateTotalOilProduction(caseId) * 1000;
        var totalCo2Emissions = CalculateTotalCO2Emissions(caseId);
        if (totalOilProductionInGasEquivalent != 0 && totalCo2Emissions != 0) {
            return totalCo2Emissions / totalOilProductionInGasEquivalent;
        }
        return 0;
    }
}
