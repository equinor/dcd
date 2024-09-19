using api.Context;
using api.Helpers;
using api.Models;

using AutoMapper;

namespace api.Services.GenerateCostProfiles;

public class FuelFlaringLossesProfileService : IFuelFlaringLossesProfileService
{
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IProjectService _projectService;
    private readonly ITopsideService _topsideService;

    public FuelFlaringLossesProfileService(
        ICaseService caseService,
        IProjectService projectService,
        ITopsideService topsideService,
        IDrainageStrategyService drainageStrategyService
    )
    {
        _caseService = caseService;
        _projectService = projectService;
        _topsideService = topsideService;
        _drainageStrategyService = drainageStrategyService;
    }

    public async Task Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCaseWithIncludes(caseId);
        var drainageStrategy = await _drainageStrategyService.GetDrainageStrategyWithIncludes(
            caseItem.DrainageStrategyLink,
            d => d.FuelFlaringAndLosses!,
            d => d.FuelFlaringAndLossesOverride!,
            d => d.ProductionProfileGas!,
            d => d.ProductionProfileOil!,
            d => d.AdditionalProductionProfileGas!,
            d => d.AdditionalProductionProfileOil!,
            d => d.ProductionProfileWaterInjection!
        );

        if (drainageStrategy.FuelFlaringAndLossesOverride?.Override == true)
        {
            return;
        }

        var topside = await _topsideService.GetTopsideWithIncludes(caseItem.TopsideLink);
        var project = await _projectService.GetProjectWithoutAssets(caseItem.ProjectId);

        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flaring = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);

        var total = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>?> { fuelConsumptions, flaring, losses });

        if (drainageStrategy.FuelFlaringAndLosses != null)
        {
            drainageStrategy.FuelFlaringAndLosses.StartYear = total.StartYear;
            drainageStrategy.FuelFlaringAndLosses.Values = total.Values;
        }
        else
        {
            drainageStrategy.FuelFlaringAndLosses = new FuelFlaringAndLosses
            {
                StartYear = total.StartYear,
                Values = total.Values
            };
        }
    }
}
