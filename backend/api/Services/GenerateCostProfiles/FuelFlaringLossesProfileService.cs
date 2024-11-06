using api.Context;
using api.Helpers;
using api.Models;

using AutoMapper;

namespace api.Services.GenerateCostProfiles;

public class FuelFlaringLossesProfileService(
    ICaseService caseService,
    IProjectService projectService,
    ITopsideService topsideService,
    IDrainageStrategyService drainageStrategyService)
    : IFuelFlaringLossesProfileService
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await caseService.GetCaseWithIncludes(caseId);
        var drainageStrategy = await drainageStrategyService.GetDrainageStrategyWithIncludes(
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

        var topside = await topsideService.GetTopsideWithIncludes(caseItem.TopsideLink);
        var project = await projectService.GetProjectWithoutAssets(caseItem.ProjectId);

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
