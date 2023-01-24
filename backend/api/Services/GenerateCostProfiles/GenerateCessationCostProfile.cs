using System.Reflection.Metadata.Ecma335;

using api.Adapters;
using api.Dtos;
using api.Models;

namespace api.Services;

public class GenerateCessationCostProfile : IGenerateCessationCostProfile
{
    private readonly ICaseService _caseService;
    private readonly ILogger<CaseService> _logger;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IWellProjectService _wellProjectService;
    private readonly ISurfService _surfService;
    private readonly IProjectService _projectService;

    public GenerateCessationCostProfile(ILoggerFactory loggerFactory, ICaseService caseService, IDrainageStrategyService drainageStrategyService,
        IWellProjectService wellProjectService, ISurfService surfService, IProjectService projectService)
    {
        _logger = loggerFactory.CreateLogger<CaseService>();
        _caseService = caseService;
        _drainageStrategyService = drainageStrategyService;
        _wellProjectService = wellProjectService;
        _surfService = surfService;
        _projectService = projectService;
    }

    public CessationCostWrapperDto Generate(Guid caseId)
    {
        var result = new CessationCostWrapperDto();
        var caseItem = _caseService.GetCase(caseId);
        var project = _projectService.GetProjectWithoutAssets(caseItem.ProjectId);

        var cessationWellsCost = new CessationWellsCost();
        var cessationOffshoreFacilitiesCost = new CessationOffshoreFacilitiesCost();

        var lastYear = GetRelativeLastYearOfProduction(caseItem);
        if (lastYear == null) { return new CessationCostWrapperDto(); }

        WellProject wellProject;
        try
        {
            wellProject = _wellProjectService.GetWellProject(caseItem.WellProjectLink);
            cessationWellsCost = GenerateCessationWellsCost(wellProject, project, (int)lastYear);
            var cessationWellsDto = CaseDtoAdapter.Convert<CessationWellsCostDto, CessationWellsCost>(cessationWellsCost);
            result.CessationWellsCostDto = cessationWellsDto;
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("WellProject {0} not found.", caseItem.WellProjectLink);
        }

        Surf surf;
        try
        {
            surf = _surfService.GetSurf(caseItem.SurfLink);
            cessationOffshoreFacilitiesCost = GenerateCessationOffshoreFacilitiesCost(surf, (int)lastYear);
            var cessationOffshoreFacilitiesCostDto = CaseDtoAdapter.Convert<CessationOffshoreFacilitiesCostDto, CessationOffshoreFacilitiesCost>(cessationOffshoreFacilitiesCost);
            result.CessationOffshoreFacilitiesCostDto = cessationOffshoreFacilitiesCostDto;
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Surf {0} not found.", caseItem.SurfLink);
        }

        var cessationTimeSeries = TimeSeriesCost.MergeCostProfiles(cessationWellsCost, cessationOffshoreFacilitiesCost);
        var cessation = new CessationCost
        {
            StartYear = cessationTimeSeries.StartYear,
            Values = cessationTimeSeries.Values
        };
        var cessationDto = CaseDtoAdapter.Convert<CessationCostDto, CessationCost>(cessation);
        result.CessationCostDto = cessationDto;
        return result;
    }

    private static CessationWellsCost GenerateCessationWellsCost(WellProject wellProject, Project project, int lastYear)
    {
        var cessationWells = new CessationWellsCost();
        var linkedWells = wellProject.WellProjectWells?.Where(ew => Well.IsWellProjectWell(ew.Well.WellCategory)).ToList();
        if (linkedWells != null)
        {
            var pluggingAndAbandonment = project.DevelopmentOperationalWellCosts?.PluggingAndAbandonment ?? 0;

            var sumDrilledWells = 0;
            foreach (var well in linkedWells)
            {
                sumDrilledWells += well.DrillingSchedule?.Values.Sum() ?? 0;
            }
            var totalCost = sumDrilledWells * (double)pluggingAndAbandonment;
            cessationWells.StartYear = lastYear;
            var cessationWellsValues = new double[] { totalCost / 2, totalCost / 2 };
            cessationWells.Values = cessationWellsValues;
        }
        return cessationWells;
    }

    private static CessationOffshoreFacilitiesCost GenerateCessationOffshoreFacilitiesCost(Surf surf, int lastYear)
    {
        var cessationOffshoreFacilities = new CessationOffshoreFacilitiesCost();

        var surfCessationCost = surf.CessationCost;

        cessationOffshoreFacilities.StartYear = lastYear + 1;
        var cessationOffshoreFacilitiesValues = new double[] { (double)surfCessationCost / 2, (double)surfCessationCost / 2 };
        cessationOffshoreFacilities.Values = cessationOffshoreFacilitiesValues;
        return cessationOffshoreFacilities;
    }

    private int? GetRelativeLastYearOfProduction(Case caseItem)
    {
        var drainageStrategy = new DrainageStrategy();
        try
        {
            drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
            return null;
        }
        if (drainageStrategy.ProductionProfileOil == null) { return null; }
        var lastYear = drainageStrategy.ProductionProfileOil.StartYear + drainageStrategy.ProductionProfileOil.Values.Length - 1;
        return lastYear;
    }
}
