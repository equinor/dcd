using api.Context;
using api.Features.TimeSeriesCalculators;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.GeneratedProfiles.GenerateCo2Intensity;

public class Co2IntensityProfileService(DcdDbContext context)
{
    public async Task CalculateCo2IntensityProfile(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles)
            .SingleAsync(c => c.Id == caseId);

        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.Co2Intensity)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        CalculateCo2Intensity(caseItem, drainageStrategy);
    }

    public static void CalculateCo2Intensity(Case caseItem, DrainageStrategy drainageStrategy)
    {
        var totalExportedVolumes = GetTotalExportedVolumes(caseItem);
        TimeSeries<double> generateCo2EmissionsProfile = new();

        var co2EmissionsOverrideProfile = caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride);
        var co2EmissionsProfile = caseItem.GetProfileOrNull(ProfileTypes.Co2Emissions);

        if (co2EmissionsOverrideProfile?.Override == true)
        {
            generateCo2EmissionsProfile.StartYear = co2EmissionsOverrideProfile.StartYear;
            generateCo2EmissionsProfile.Values = co2EmissionsOverrideProfile.Values.Select(v => v).ToArray();
        }
        else
        {
            generateCo2EmissionsProfile.StartYear = co2EmissionsProfile?.StartYear ?? 0;
            generateCo2EmissionsProfile.Values = co2EmissionsProfile?.Values ?? [];
        }

        var co2IntensityValues = new List<double>();

        const int tonnesToKgFactor = 1000;
        const double boeConversionFactor = 6.29;
        var yearDifference = 0;
        if (generateCo2EmissionsProfile.StartYear != totalExportedVolumes.StartYear)
        {
            yearDifference = generateCo2EmissionsProfile.StartYear - totalExportedVolumes.StartYear;
        }
        for (var i = 0; i < generateCo2EmissionsProfile.Values.Length; i++)
        {
            if (yearDifference + i < 0) { continue; }

            if ((i + yearDifference < totalExportedVolumes.Values.Length) && totalExportedVolumes.Values[i + yearDifference] != 0)
            {
                var dividedProfiles = generateCo2EmissionsProfile.Values[i] / totalExportedVolumes.Values[i + yearDifference];
                var co2Intensity = dividedProfiles / 1E6 / boeConversionFactor * tonnesToKgFactor;
                co2IntensityValues.Add(co2Intensity);

            }
        }

        var co2YearOffset = yearDifference < 0 ? yearDifference : 0;

        drainageStrategy.Co2Intensity = new()
        {
            StartYear = generateCo2EmissionsProfile.StartYear - co2YearOffset,
            Values = co2IntensityValues.ToArray(),
        };
    }

    private static TimeSeries<double> GetTotalExportedVolumes(Case caseItem)
    {
        var oilProfile = GetOilProfile(caseItem);

        return new Co2Intensity
        {
            StartYear = oilProfile.StartYear,
            Values = oilProfile.Values
        };
    }

    private static TimeSeries<double> GetOilProfile(Case caseItem)
    {
        var million = 1E6;
        var oilValues = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.Values.Select(v => v / million).ToArray() ?? [];
        var additionalOilValues = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.Values.Select(v => v / million).ToArray() ?? [];

        TimeSeriesCost? oilProfile = null;
        TimeSeriesCost? additionalOilProfile = null;

        if (caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil) != null)
        {
            oilProfile = new TimeSeriesCost
            {
                StartYear = caseItem.GetProfile(ProfileTypes.ProductionProfileOil).StartYear,
                Values = oilValues,
            };
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil) != null)
        {
            additionalOilProfile = new TimeSeriesCost
            {
                StartYear = caseItem.GetProfile(ProfileTypes.AdditionalProductionProfileOil).StartYear,
                Values = additionalOilValues,
            };
        }

        var mergedProfiles = CostProfileMerger.MergeCostProfiles(
            oilProfile ?? new TimeSeriesCost { Values = [], StartYear = 0 },
            additionalOilProfile ?? new TimeSeriesCost { Values = [], StartYear = 0 }
        );

        return new TimeSeries<double>
        {
            Values = mergedProfiles.Values,
            StartYear = mergedProfiles.StartYear,
        };
    }
}
