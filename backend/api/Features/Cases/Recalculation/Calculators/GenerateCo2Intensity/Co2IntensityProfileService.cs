using api.Features.Cases.Recalculation.Types.TotalProductionVolume;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Calculators.GenerateCo2Intensity;

public static class Co2IntensityProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var totalExportedVolumes = TotalExportedVolumesProfileService.GetTotalExportedVolumes(caseItem);
        var co2EmissionsProfile = GetCo2EmissionsProfile(caseItem);

        var co2IntensityValues = new List<double>();

        var yearDifference = co2EmissionsProfile.StartYear - totalExportedVolumes.StartYear;

        for (var i = 0; i < co2EmissionsProfile.Values.Length; i++)
        {
            var totalExportedVolumesIndex = i + yearDifference;

            if (totalExportedVolumesIndex < 0 || totalExportedVolumesIndex >= totalExportedVolumes.Values.Length)
            {
                continue;
            }

            if (totalExportedVolumes.Values[totalExportedVolumesIndex] == 0)
            {
                continue;
            }

            var totalExportedVolumesValue = totalExportedVolumes.Values[totalExportedVolumesIndex];
            var co2Intensity = co2EmissionsProfile.Values[i] / 1000 / (totalExportedVolumesValue / 1_000_000);
            co2IntensityValues.Add(co2Intensity);
        }

        var co2YearOffset = yearDifference < 0 ? yearDifference : 0;

        var co2IntensityProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.Co2Intensity);

        co2IntensityProfile.StartYear = co2EmissionsProfile.StartYear - co2YearOffset;
        co2IntensityProfile.Values = co2IntensityValues.ToArray();
    }

    public static TimeSeries GetOilProfile(Case caseItem)
    {
        const double million = 1E6;
        var oilValues = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.Values.Select(v => v / million).ToArray() ?? [];
        var additionalOilValues = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.Values.Select(v => v / million).ToArray() ?? [];

        var oilProfile = new TimeSeries();
        var additionalOilProfile = new TimeSeries();

        if (caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil) != null)
        {
            oilProfile = new TimeSeries
            {
                StartYear = caseItem.GetProfile(ProfileTypes.ProductionProfileOil).StartYear,
                Values = oilValues
            };
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil) != null)
        {
            additionalOilProfile = new TimeSeries
            {
                StartYear = caseItem.GetProfile(ProfileTypes.AdditionalProductionProfileOil).StartYear,
                Values = additionalOilValues
            };
        }

        return TimeSeriesMerger.MergeTimeSeries(oilProfile, additionalOilProfile);
    }

    public static TimeSeries GetCo2EmissionsProfile(Case caseItem) =>
        new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.Co2Emissions));

    public static TimeSeries GetCo2IntensityProfile(Case caseItem) =>
        new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.Co2Intensity));
}
