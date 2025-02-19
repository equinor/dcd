using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.Co2EmissionsProfile;

public static class Co2EmissionsProfileService
{
    public static void RunCalculation(Case caseItem, List<DevelopmentWell> developmentWells)
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

        var newProfile = new TimeSeries
        {
            StartYear = tempProfile.StartYear,
            Values = convertedValues.ToArray()
        };

        var drillingEmissionsProfile = CalculateDrillingEmissions(caseItem.Project, developmentWells);

        var totalProfile = TimeSeriesMerger.MergeTimeSeries(newProfile, drillingEmissionsProfile);

        var co2Emissions = caseItem.CreateProfileIfNotExists(ProfileTypes.Co2Emissions);

        co2Emissions.Values = totalProfile.Values;
        co2Emissions.StartYear = totalProfile.StartYear;
    }

    private static TimeSeries GetLossesProfile(Case caseItem)
    {
        var losses = EmissionCalculationHelper.CalculateLosses(caseItem);

        return new TimeSeries
        {
            StartYear = losses.StartYear,
            Values = losses.Values.Select(loss => loss * caseItem.Project.CO2Vented).ToArray()
        };
    }

    private static TimeSeries GetFlaringsProfile(Case caseItem)
    {
        var flarings = EmissionCalculationHelper.CalculateFlaring(caseItem);

        return new TimeSeries
        {
            StartYear = flarings.StartYear,
            Values = flarings.Values.Select(flare => flare * caseItem.Project.CO2EmissionsFromFlaredGas).ToArray()
        };
    }

    private static TimeSeries GetFuelConsumptionsProfile(Case caseItem)
    {
        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem);

        return new TimeSeries
        {
            StartYear = fuelConsumptions.StartYear,
            Values = fuelConsumptions.Values.Select(fuel => fuel * caseItem.Project.CO2EmissionFromFuelGas).ToArray(),
        };
    }

    private static TimeSeries CalculateDrillingEmissions(Project project, List<DevelopmentWell> developmentWells)
    {
        var wellDrillingSchedules = new TimeSeries();

        foreach (var developmentWell in developmentWells)
        {
            var timeSeries = new TimeSeries
            {
                StartYear = developmentWell.StartYear,
                Values = developmentWell.Values.Select(v => (double)v).ToArray()
            };

            wellDrillingSchedules = TimeSeriesMerger.MergeTimeSeries(wellDrillingSchedules, timeSeries);
        }

        return new TimeSeries
        {
            StartYear = wellDrillingSchedules.StartYear,
            Values = wellDrillingSchedules.Values
                .Select(well => well * project.AverageDevelopmentDrillingDays * project.DailyEmissionFromDrillingRig)
                .ToArray()
        };
    }
}
