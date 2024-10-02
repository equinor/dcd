using api.Models;

using AutoMapper;

namespace api.Services;

public class CessationCostProfileService : ICessationCostProfileService
{
    private readonly ICaseService _caseService;
    private readonly ILogger<CessationCostProfileService> _logger;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IWellProjectService _wellProjectService;
    private readonly IWellProjectWellService _wellProjectWellService;
    private readonly ISurfService _surfService;
    private readonly IProjectService _projectService;

    public CessationCostProfileService(
        ILoggerFactory loggerFactory,
        ICaseService caseService,
        IDrainageStrategyService drainageStrategyService,
        IWellProjectService wellProjectService,
        IWellProjectWellService wellProjectWellService,
        ISurfService surfService,
        IProjectService projectService
        )
    {
        _logger = loggerFactory.CreateLogger<CessationCostProfileService>();
        _caseService = caseService;
        _drainageStrategyService = drainageStrategyService;
        _wellProjectService = wellProjectService;
        _wellProjectWellService = wellProjectWellService;
        _surfService = surfService;
        _projectService = projectService;
    }

    public async Task Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCaseWithIncludes(
            caseId,
            c => c.CessationWellsCostOverride!,
            c => c.CessationWellsCost!,
            c => c.CessationOffshoreFacilitiesCostOverride!,
            c => c.CessationOffshoreFacilitiesCost!
        );

        var drainageStrategy = await _drainageStrategyService.GetDrainageStrategyWithIncludes(
            caseItem.DrainageStrategyLink,
            d => d.ProductionProfileOil!,
            d => d.AdditionalProductionProfileOil!,
            d => d.ProductionProfileGas!,
            d => d.AdditionalProductionProfileGas!
        );

        var lastYearOfProduction = CalculationHelper.GetRelativeLastYearOfProduction(drainageStrategy);

        var project = await _projectService.GetProjectWithoutAssets(caseItem.ProjectId);

        await CalculateCessationWellsCost(caseItem, project, lastYearOfProduction);
        await GetCessationOffshoreFacilitiesCost(caseItem, lastYearOfProduction);
    }

    private async Task CalculateCessationWellsCost(Case caseItem, Project project, int? lastYear)
    {
        if (caseItem.CessationWellsCostOverride?.Override == true)
        {
            return;
        }

        if (!lastYear.HasValue)
        {
            CalculationHelper.ResetTimeSeries(caseItem.CessationWellsCost);
            return;
        }

        caseItem.CessationWellsCost = await GenerateCessationWellsCost(
            caseItem.WellProjectLink,
            project,
            lastYear.Value,
            caseItem.CessationWellsCost ?? new CessationWellsCost()
        );
    }

    private async Task GetCessationOffshoreFacilitiesCost(Case caseItem, int? lastYear)
    {
        if (caseItem.CessationOffshoreFacilitiesCostOverride?.Override == true)
        {
            return;
        }

        if (!lastYear.HasValue)
        {
            CalculationHelper.ResetTimeSeries(caseItem.CessationOffshoreFacilitiesCost);
            return;
        }

        var surf = await _surfService.GetSurfWithIncludes(caseItem.SurfLink);
        caseItem.CessationOffshoreFacilitiesCost = GenerateCessationOffshoreFacilitiesCost(
            surf,
            lastYear.Value,
            caseItem.CessationOffshoreFacilitiesCost ?? new CessationOffshoreFacilitiesCost()
        );
    }

    private async Task<CessationWellsCost> GenerateCessationWellsCost(Guid wellProjectId, Project project, int lastYear, CessationWellsCost cessationWells)
    {
        var linkedWells = await _wellProjectWellService.GetWellProjectWellsForWellProject(wellProjectId);
        if (linkedWells == null)
        {
            return cessationWells;
        }

        var pluggingAndAbandonment = project.DevelopmentOperationalWellCosts?.PluggingAndAbandonment ?? 0;

        var sumDrilledWells = linkedWells
            .Select(well => well.DrillingSchedule?.Values.Sum() ?? 0)
            .Sum();

        var totalCost = sumDrilledWells * (double)pluggingAndAbandonment;
        cessationWells.StartYear = lastYear;
        cessationWells.Values = [totalCost / 2, totalCost / 2];

        return cessationWells;
    }

    private static CessationOffshoreFacilitiesCost GenerateCessationOffshoreFacilitiesCost(Surf surf, int lastYear, CessationOffshoreFacilitiesCost cessationOffshoreFacilities)
    {
        var surfCessationCost = surf.CessationCost;

        cessationOffshoreFacilities.StartYear = lastYear + 1;
        cessationOffshoreFacilities.Values = [(double)surfCessationCost / 2, (double)surfCessationCost / 2];
        return cessationOffshoreFacilities;
    }
}
