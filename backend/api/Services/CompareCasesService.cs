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

    public double CalculateTotalOilProduction(Case caseItem)
    {
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

    public double CalculateTotalGasProduction(Case caseItem)
    {
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

    public double CalculateTotalExportedVolumes(Case caseItem)
    {
        return CalculateTotalOilProduction(caseItem) + CalculateTotalGasProduction(caseItem);
    }

    public double CalculateTotalStudyCostsPlusOpex(Guid caseId)
    {
        var generateStudyProfile = _generateStudyCostProfile.Generate(caseId);
        var generateOpexProfile = _generateOpexCostProfile.Generate(caseId);

        var sumStudyValues = generateStudyProfile.Values.Sum();
        var sumOpexValues = generateOpexProfile.Values.Sum();

        return sumStudyValues + sumOpexValues;
    }

    public double CalculateTotalCessationCosts(Guid caseId)
    {
        var generateCessationProfile = _generateCessationCostProfile.Generate(caseId);
        return generateCessationProfile.Values.Sum();
    }

    public double CalculateOffshorePlusOnshoreFacilityCosts(Case caseItem)
    {
        return _generateStudyCostProfile.SumAllCostFacility(caseItem);

    }

    public double CalculateDevelopmentWellCosts(Case caseItem)
    {
        return _generateStudyCostProfile.SumWellCost(caseItem);
    }

    public double CalculateExplorationWellCosts(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var sumExplorationWellCost = 0.0;
        var generateGAndGAdminProfile = _generateGAndGAdminCostProfile.Generate(caseId);
        sumExplorationWellCost += generateGAndGAdminProfile.Values.Sum();

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
        return generateCo2EmissionsProfile.Values.Sum();
    }

    public double CalculateCO2Intensity(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var totalOilProductionInGasEquivalent = CalculateTotalOilProduction(caseItem) * 1000;
        return CalculateTotalCO2Emissions(caseId) / totalOilProductionInGasEquivalent;
    }
}