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
        var co2EmissionsProfile = GetCo2EmissionsProfile(caseItem);

        var co2IntensityValues = new List<double>();

        const double boeConversionFactor = 6.29;

        var yearDifference = co2EmissionsProfile.StartYear - totalExportedVolumes.StartYear;

        for (var i = 0; i < co2EmissionsProfile.Values.Length; i++)
        {
            var totalExportedVolumesIndex = i + yearDifference;

            if (totalExportedVolumesIndex < 0 || totalExportedVolumesIndex >= totalExportedVolumes.Values.Length)
            {
                continue;
            }

            if (totalExportedVolumes.Values[totalExportedVolumesIndex] != 0)
            {
                var totalExportedVolumesValue = totalExportedVolumes.Values[totalExportedVolumesIndex];
                var co2Intensity = co2EmissionsProfile.Values[i] / 1000 / (totalExportedVolumesValue * boeConversionFactor);
                co2IntensityValues.Add(co2Intensity);
            }
        }

        var co2YearOffset = yearDifference < 0 ? yearDifference : 0;

        var co2IntensityProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.Co2Intensity);

        co2IntensityProfile.StartYear = co2EmissionsProfile.StartYear - co2YearOffset;
        co2IntensityProfile.Values = co2IntensityValues.ToArray();
    }

    private static TimeSeriesCost GetTotalExportedVolumes(Case caseItem)
    {
        var oilProfile = GetOilProfile(caseItem);
        var netSalesGas = caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas)?.Values ?? [];
        var netSalesGasValues = new TimeSeriesCost { StartYear = oilProfile.StartYear, Values = netSalesGas.Select(v => v / 1E9).ToArray() };
        var totalExportedVolumes = TimeSeriesMerger.MergeTimeSeries(oilProfile, netSalesGasValues);
        return totalExportedVolumes;
    }

    public static TimeSeriesCost GetOilProfile(Case caseItem)
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

    public static TimeSeriesCost GetCo2EmissionsProfile(Case caseItem)
    {
        var co2EmissionsOverrideProfile = caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride);
        var co2EmissionsProfile = caseItem.GetProfileOrNull(ProfileTypes.Co2Emissions);

        if (co2EmissionsOverrideProfile?.Override == true)
        {
            return new TimeSeriesCost
            {
                StartYear = co2EmissionsOverrideProfile.StartYear,
                Values = co2EmissionsOverrideProfile.Values.Select(v => v).ToArray()
            };
        }

        return new TimeSeriesCost
        {
            StartYear = co2EmissionsProfile?.StartYear ?? 0,
            Values = co2EmissionsProfile?.Values ?? []
        };
    }
}
