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
            .Include(x => x.Project)
            .Include(x => x.Topside)
            .SingleAsync(x => x.Id == caseId);

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(y => profileTypes.Contains(y.ProfileType))
            .LoadAsync();

        var drillingSchedulesForWellProjectWell = await context.WellProjectWell
            .Where(w => w.WellProjectId == caseItem.WellProjectLink)
            .Select(x => x.DrillingSchedule)
            .Where(x => x != null)
            .Select(x => x!)
            .ToListAsync();

        RunCalculation(caseItem, drillingSchedulesForWellProjectWell);
    }

    public static void RunCalculation(Case caseItem, List<DrillingSchedule> drillingSchedulesForWellProjectWell)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride)?.Override == true)
        {
            return;
        }

        var fuelConsumptionsProfile = GetFuelConsumptionsProfile(caseItem);
        var flaringsProfile = GetFlaringsProfile(caseItem);
        var lossesProfile = GetLossesProfile(caseItem);

        var tempProfile = TimeSeriesMerger.MergeTimeSeries(fuelConsumptionsProfile, flaringsProfile, lossesProfile);

        var convertedValues = tempProfile.Values.Select(v => v / 1000);

        var newProfile = new TimeSeriesCost
        {
            StartYear = tempProfile.StartYear,
            Values = convertedValues.ToArray()
        };

        var drillingEmissionsProfile = CalculateDrillingEmissions(caseItem.Project, drillingSchedulesForWellProjectWell);

        var totalProfile = TimeSeriesMerger.MergeTimeSeries(newProfile, drillingEmissionsProfile);

        var co2Emissions = caseItem.CreateProfileIfNotExists(ProfileTypes.Co2Emissions);

        co2Emissions.Values = totalProfile.Values;
        co2Emissions.StartYear = totalProfile.StartYear;
    }

    private static TimeSeriesCost GetLossesProfile(Case caseItem)
    {
        var losses = EmissionCalculationHelper.CalculateLosses(caseItem);

        return new TimeSeriesCost
        {
            StartYear = losses.StartYear,
            Values = losses.Values.Select(loss => loss * caseItem.Project.CO2Vented).ToArray()
        };
    }

    private static TimeSeriesCost GetFlaringsProfile(Case caseItem)
    {
        var flarings = EmissionCalculationHelper.CalculateFlaring(caseItem);

        return new TimeSeriesCost
        {
            StartYear = flarings.StartYear,
            Values = flarings.Values.Select(flare => flare * caseItem.Project.CO2EmissionsFromFlaredGas).ToArray()
        };
    }

    private static TimeSeriesCost GetFuelConsumptionsProfile(Case caseItem)
    {
        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem);

        return new TimeSeriesCost
        {
            StartYear = fuelConsumptions.StartYear,
            Values = fuelConsumptions.Values.Select(fuel => fuel * caseItem.Project.CO2EmissionFromFuelGas).ToArray(),
        };
    }

    private static TimeSeriesCost CalculateDrillingEmissions(Project project, List<DrillingSchedule> drillingSchedulesForWellProjectWell)
    {
        var wellDrillingSchedules = new TimeSeriesCost();

        foreach (var drillingSchedule in drillingSchedulesForWellProjectWell)
        {
            var timeSeries = new TimeSeriesCost
            {
                StartYear = drillingSchedule.StartYear,
                Values = drillingSchedule.Values.Select(v => (double)v).ToArray()
            };

            wellDrillingSchedules = TimeSeriesMerger.MergeTimeSeries(wellDrillingSchedules, timeSeries);
        }

        return new TimeSeriesCost
        {
            StartYear = wellDrillingSchedules.StartYear,
            Values = wellDrillingSchedules.Values
                .Select(well => well * project.AverageDevelopmentDrillingDays * project.DailyEmissionFromDrillingRig)
                .ToArray()
        };
    }
}
