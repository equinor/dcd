using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Calculators.GenerateCo2Intensity;

public static class Co2IntensityProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var totalExportedVolumes = GetTotalExportedVolumes(caseItem);
        var generateCo2EmissionsProfile = new TimeSeriesCost();

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

                var oilProduction = GetOilProfile(caseItem).Values[i + yearDifference];
                var co2Intensity = generateCo2EmissionsProfile.Values[i] / 1000 / (oilProduction * boeConversionFactor);
                co2IntensityValues.Add(co2Intensity);
            }
        }

        var co2YearOffset = yearDifference < 0 ? yearDifference : 0;

        var co2IntensityProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.Co2Intensity);

        co2IntensityProfile.StartYear = generateCo2EmissionsProfile.StartYear - co2YearOffset;
        co2IntensityProfile.Values = co2IntensityValues.ToArray();
    }

    private static TimeSeriesCost GetTotalExportedVolumes(Case caseItem)
    {
        var oilProfile = GetOilProfile(caseItem);

        return new TimeSeriesCost
        {
            StartYear = oilProfile.StartYear,
            Values = oilProfile.Values
        };
    }

    private static TimeSeriesCost GetOilProfile(Case caseItem)
    {
        var million = 1E6;
        var oilValues = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.Values.Select(v => v / million).ToArray() ?? [];
        var additionalOilValues = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.Values.Select(v => v / million).ToArray() ?? [];

        var oilProfile = new TimeSeriesCost();
        var additionalOilProfile = new TimeSeriesCost();

        if (caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil) != null)
        {
            oilProfile = new TimeSeriesCost
            {
                StartYear = caseItem.GetProfile(ProfileTypes.ProductionProfileOil).StartYear,
                Values = oilValues
            };
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil) != null)
        {
            additionalOilProfile = new TimeSeriesCost
            {
                StartYear = caseItem.GetProfile(ProfileTypes.AdditionalProductionProfileOil).StartYear,
                Values = additionalOilValues
            };
        }

        return TimeSeriesMerger.MergeTimeSeries(oilProfile, additionalOilProfile);
    }
}
