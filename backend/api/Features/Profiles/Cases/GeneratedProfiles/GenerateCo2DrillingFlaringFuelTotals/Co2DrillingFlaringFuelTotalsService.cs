using api.Context;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.GeneratedProfiles.GenerateCo2DrillingFlaringFuelTotals;

public class Co2DrillingFlaringFuelTotalsService(DcdDbContext context)
{
    public async Task<Co2DrillingFlaringFuelTotalsDto> Generate(Guid caseId)
    {
        var caseItem = await context.Cases.SingleAsync(x => x.Id == caseId);

        var topside = await context.Topsides.SingleAsync(x => x.Id == caseItem.TopsideLink);

        var project = await context.Projects
            .Include(p => p.Cases)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .SingleAsync(p => p.Id == caseItem.ProjectId);

        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.ProductionProfileOil)
            .Include(d => d.AdditionalProductionProfileOil)
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileGas)
            .Include(d => d.ProductionProfileWaterInjection)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        var fuelConsumptionsTotal = GetFuelConsumptionsProfileTotal(project, caseItem, topside, drainageStrategy);
        var flaringsTotal = GetFlaringsProfileTotal(project, drainageStrategy);
        var drillingEmissionsTotal = await CalculateDrillingEmissionsTotal(project, caseItem.WellProjectLink);

        return new Co2DrillingFlaringFuelTotalsDto
        {
            Co2Drilling = drillingEmissionsTotal,
            Co2Flaring = flaringsTotal,
            Co2Fuel = fuelConsumptionsTotal
        };
    }

    private static double GetFlaringsProfileTotal(Project project, DrainageStrategy drainageStrategy)
    {
        var flarings = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);

        var flaringsProfile = new TimeSeriesVolume
        {
            StartYear = flarings.StartYear,
            Values = flarings.Values.Select(flare => flare * project.CO2EmissionsFromFlaredGas).ToArray(),
        };

        return flaringsProfile.Values.Sum() / 1000;
    }

    private static double GetFuelConsumptionsProfileTotal(
        Project project,
        Case caseItem,
        Topside topside,
        DrainageStrategy drainageStrategy
    )
    {
        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);

        var fuelConsumptionsProfile = new TimeSeriesVolume
        {
            StartYear = fuelConsumptions.StartYear,
            Values = fuelConsumptions.Values.Select(fuel => fuel * project.CO2EmissionFromFuelGas).ToArray(),
        };

        return fuelConsumptionsProfile.Values.Sum() / 1000;
    }

    private async Task<double> CalculateDrillingEmissionsTotal(Project project, Guid wellProjectId)
    {
        var linkedWells = await context.WellProjectWell
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.WellProjectId == wellProjectId).ToListAsync();

        var wellDrillingSchedules = new TimeSeries<double>();
        foreach (var linkedWell in linkedWells)
        {
            if (linkedWell.DrillingSchedule == null)
            {
                continue;
            }

            var timeSeries = new TimeSeries<double>
            {
                StartYear = linkedWell.DrillingSchedule.StartYear,
                Values = linkedWell.DrillingSchedule.Values.Select(v => (double)v).ToArray(),
            };
            wellDrillingSchedules = TimeSeriesCost.MergeCostProfiles(wellDrillingSchedules, timeSeries);
        }

        var drillingEmission = new ProductionProfileGas
        {
            StartYear = wellDrillingSchedules.StartYear,
            Values = wellDrillingSchedules.Values
                .Select(well => well * project.AverageDevelopmentDrillingDays * project.DailyEmissionFromDrillingRig)
                .ToArray(),
        };

        return drillingEmission.Values.Sum();
    }
}
