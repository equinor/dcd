using api.Context;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.FuelFlaringLossesProfile;

public class FuelFlaringLossesProfileService(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await context.Cases.SingleAsync(x => x.Id == caseId);
        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.FuelFlaringAndLosses)
            .Include(d => d.FuelFlaringAndLossesOverride)
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.ProductionProfileOil)
            .Include(d => d.AdditionalProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileOil)
            .Include(d => d.ProductionProfileWaterInjection)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        if (drainageStrategy.FuelFlaringAndLossesOverride?.Override == true)
        {
            return;
        }

        var topside = await context.Topsides.SingleAsync(x => x.Id == caseItem.TopsideLink);
        var project = await context.Projects
            .Include(p => p.Cases)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .SingleAsync(p => p.Id == caseItem.ProjectId);

        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flaring = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);

        var total = TimeSeriesCost.MergeCostProfilesList([fuelConsumptions, flaring, losses]);

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
