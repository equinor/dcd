using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles.Dtos;
using api.Features.TimeSeriesCalculators;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.GeneratedProfiles.GenerateCo2DrillingFlaringFuelTotals;

public class Co2DrillingFlaringFuelTotalsService(DcdDbContext context)
{
    public async Task<Co2DrillingFlaringFuelTotalsDto> Generate(Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var profileTypes = new List<string>
        {
            ProfileTypes.ProductionProfileOil,
            ProfileTypes.AdditionalProductionProfileOil,
            ProfileTypes.ProductionProfileGas,
            ProfileTypes.AdditionalProductionProfileGas,
            ProfileTypes.ProductionProfileWaterInjection
        };

        var caseItem = await context.Cases
            .Include(x => x.Project)
            .Include(x => x.Topside)
            .SingleAsync(x => x.Project.Id == projectPk && x.Id == caseId);

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(y => profileTypes.Contains(y.ProfileType))
            .LoadAsync();

        var drillingSchedulesForWellProjectWell = await context.WellProjectWell
            .Where(w => w.WellProjectId == caseItem.WellProjectLink)
            .Select(x => x.DrillingSchedule)
            .ToListAsync();

        var fuelConsumptionsTotal = GetFuelConsumptionsProfileTotal(caseItem);
        var flaringsTotal = GetFlaringsProfileTotal(caseItem);
        var drillingEmissionsTotal = CalculateDrillingEmissionsTotal(caseItem.Project, drillingSchedulesForWellProjectWell);

        return new Co2DrillingFlaringFuelTotalsDto
        {
            Co2Drilling = drillingEmissionsTotal,
            Co2Flaring = flaringsTotal,
            Co2Fuel = fuelConsumptionsTotal
        };
    }

    private static double GetFlaringsProfileTotal(Case caseItem)
    {
        var flarings = EmissionCalculationHelper.CalculateFlaring(caseItem);

        var flaringsProfile = new TimeSeriesCost
        {
            StartYear = flarings.StartYear,
            Values = flarings.Values.Select(flare => flare * caseItem.Project.CO2EmissionsFromFlaredGas).ToArray()
        };

        return flaringsProfile.Values.Sum() / 1000;
    }

    private static double GetFuelConsumptionsProfileTotal(Case caseItem)
    {
        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem);

        var fuelConsumptionsProfile = new TimeSeriesCost
        {
            StartYear = fuelConsumptions.StartYear,
            Values = fuelConsumptions.Values.Select(fuel => fuel * caseItem.Project.CO2EmissionFromFuelGas).ToArray()
        };

        return fuelConsumptionsProfile.Values.Sum() / 1000;
    }

    private static double CalculateDrillingEmissionsTotal(Project project, List<DrillingSchedule?> drillingSchedulesForWellProjectWell)
    {
        var wellDrillingSchedules = new TimeSeriesCost();

        foreach (var drillingSchedule in drillingSchedulesForWellProjectWell)
        {
            if (drillingSchedule == null)
            {
                continue;
            }

            var timeSeries = new TimeSeriesCost
            {
                StartYear = drillingSchedule.StartYear,
                Values = drillingSchedule.Values.Select(v => (double)v).ToArray()
            };

            wellDrillingSchedules = TimeSeriesMerger.MergeTimeSeries(wellDrillingSchedules, timeSeries);
        }

        return wellDrillingSchedules.Values
            .Select(well => well * project.AverageDevelopmentDrillingDays * project.DailyEmissionFromDrillingRig)
            .Sum();
    }
}
