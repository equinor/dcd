using api.Context;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.TimeSeriesCalculators;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.Co2EmissionsProfile;

public class Co2EmissionsProfileService(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var profileTypes = new List<string>
        {
            ProfileTypes.Co2Emissions,
            ProfileTypes.Co2EmissionsOverride,
            ProfileTypes.ProductionProfileOil,
            ProfileTypes.AdditionalProductionProfileOil,
            ProfileTypes.ProductionProfileGas,
            ProfileTypes.AdditionalProductionProfileGas,
            ProfileTypes.ProductionProfileWaterInjection
        };

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => profileTypes.Contains(y.ProfileType)))
            .SingleAsync(x => x.Id == caseId);

        var drainageStrategy = await context.DrainageStrategies
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        if (caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride)?.Override == true)
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

        var linkedWells = await context.WellProjectWell
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.WellProjectId == caseItem.WellProjectLink)
            .ToListAsync();

        var fuelConsumptionsProfile = GetFuelConsumptionsProfile(project, caseItem, topside);
        var flaringsProfile = GetFlaringsProfile(project, caseItem);
        var lossesProfile = GetLossesProfile(project, caseItem);
        
        var tempProfile = TimeSeriesMerger.MergeTimeSeries(fuelConsumptionsProfile, flaringsProfile, lossesProfile);

        var convertedValues = tempProfile.Values.Select(v => v / 1000);

        var newProfile = new TimeSeriesCost
        {
            StartYear = tempProfile.StartYear,
            Values = convertedValues.ToArray()
        };

        var drillingEmissionsProfile = await CalculateDrillingEmissions(project, caseItem.WellProjectLink);

        var totalProfile = TimeSeriesMerger.MergeTimeSeries(newProfile, drillingEmissionsProfile);

        var co2Emissions = caseItem.CreateProfileIfNotExists(ProfileTypes.Co2Emissions);

        co2Emissions.Values = totalProfile.Values;
        co2Emissions.StartYear = totalProfile.StartYear;
    }

    private static TimeSeriesCost GetLossesProfile(Project project, Case caseItem)
    {
        var losses = EmissionCalculationHelper.CalculateLosses(project, caseItem);

        var lossesProfile = new TimeSeriesCost
        {
            StartYear = losses.StartYear,
            Values = losses.Values.Select(loss => loss * project.CO2Vented).ToArray()
        };

        return lossesProfile;
    }

    private static TimeSeriesCost GetFlaringsProfile(Project project, Case caseItem)
    {
        var flarings = EmissionCalculationHelper.CalculateFlaring(project, caseItem);

        var flaringsProfile = new TimeSeriesCost
        {
            StartYear = flarings.StartYear,
            Values = flarings.Values.Select(flare => flare * project.CO2EmissionsFromFlaredGas).ToArray()
        };
        return flaringsProfile;
    }

    private static TimeSeriesCost GetFuelConsumptionsProfile(Project project, Case caseItem, Topside topside)
    {
        var fuelConsumptions =
            EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside);

        var fuelConsumptionsProfile = new TimeSeriesCost
        {
            StartYear = fuelConsumptions.StartYear,
            Values = fuelConsumptions.Values.Select(fuel => fuel * project.CO2EmissionFromFuelGas).ToArray(),
        };
        return fuelConsumptionsProfile;
    }

    private async Task<TimeSeriesCost> CalculateDrillingEmissions(Project project, Guid wellProjectId)
    {
        var linkedWells = await context.WellProjectWell
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.WellProjectId == wellProjectId).ToListAsync();

        var wellDrillingSchedules = new TimeSeriesCost();
        foreach (var linkedWell in linkedWells)
        {
            if (linkedWell.DrillingSchedule == null)
            {
                continue;
            }

            var timeSeries = new TimeSeriesCost
            {
                StartYear = linkedWell.DrillingSchedule.StartYear,
                Values = linkedWell.DrillingSchedule.Values.Select(v => (double)v).ToArray()
            };
            wellDrillingSchedules = TimeSeriesMerger.MergeTimeSeries(wellDrillingSchedules, timeSeries);
        }

        var drillingEmission = new TimeSeriesCost
        {
            StartYear = wellDrillingSchedules.StartYear,
            Values = wellDrillingSchedules.Values
                .Select(well => well * project.AverageDevelopmentDrillingDays * project.DailyEmissionFromDrillingRig)
                .ToArray()
        };

        return drillingEmission;
    }
}
