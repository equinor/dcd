using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.OpexCostProfile;

public static class OpexCostProfileService
{
    public static void RunCalculation(Case caseItem, List<DevelopmentWell> developmentWells)
    {
        var lastYearOfProduction = CalculationHelper.GetRelativeLastYearOfProduction(caseItem);
        var firstYearOfProduction = CalculationHelper.GetRelativeFirstYearOfProduction(caseItem);

        CalculateWellInterventionCostProfile(caseItem, developmentWells, lastYearOfProduction);
        CalculateOffshoreFacilitiesOperationsCostProfile(caseItem, firstYearOfProduction, lastYearOfProduction);
    }

    private static void CalculateWellInterventionCostProfile(Case caseItem, List<DevelopmentWell> developmentWells, int? lastYearOfProduction)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)?.Override == true)
        {
            return;
        }

        var lastYear = lastYearOfProduction ?? 0;

        if (developmentWells.Count == 0)
        {
            CalculationHelper.ResetTimeSeries(caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile));
            return;
        }

        var wellInterventionCostsFromDrillingSchedule = new TimeSeriesCost();

        foreach (var developmentWell in developmentWells)
        {
            var timeSeries = new TimeSeriesCost
            {
                StartYear = developmentWell.StartYear,
                Values = developmentWell.Values.Select(v => (double)v).ToArray()
            };

            wellInterventionCostsFromDrillingSchedule = TimeSeriesMerger.MergeTimeSeries(wellInterventionCostsFromDrillingSchedule, timeSeries);
        }

        var tempSeries = new TimeSeriesCost
        {
            StartYear = wellInterventionCostsFromDrillingSchedule.StartYear,
            Values = wellInterventionCostsFromDrillingSchedule.Values
        };

        var cumulativeDrillingSchedule = GetCumulativeDrillingSchedule(tempSeries);
        cumulativeDrillingSchedule.StartYear = tempSeries.StartYear;

        var interventionCost = caseItem.Project.DevelopmentOperationalWellCosts?.AnnualWellInterventionCostPerWell ?? 0;

        var wellInterventionCostValues = cumulativeDrillingSchedule.Values.Select(v => v * interventionCost).ToArray();

        wellInterventionCostsFromDrillingSchedule.Values = wellInterventionCostValues;
        wellInterventionCostsFromDrillingSchedule.StartYear = cumulativeDrillingSchedule.StartYear;

        var totalValuesCount = lastYear == 0 ? wellInterventionCostsFromDrillingSchedule.Values.Length : lastYear - wellInterventionCostsFromDrillingSchedule.StartYear;
        var additionalValuesCount = totalValuesCount - wellInterventionCostsFromDrillingSchedule.Values.Length;

        var additionalValues = new List<double>();
        for (int i = 0; i < additionalValuesCount; i++)
        {
            if (wellInterventionCostsFromDrillingSchedule.Values.Length > 0)
            {
                additionalValues.Add(wellInterventionCostsFromDrillingSchedule.Values.Last());
            }
        }

        var valuesList = wellInterventionCostsFromDrillingSchedule.Values.ToList();
        valuesList.AddRange(additionalValues);

        wellInterventionCostsFromDrillingSchedule.Values = valuesList.ToArray();

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.WellInterventionCostProfile);

        profile.Values = wellInterventionCostsFromDrillingSchedule.Values;
        profile.StartYear = wellInterventionCostsFromDrillingSchedule.StartYear;
    }

    private static void CalculateOffshoreFacilitiesOperationsCostProfile(Case caseItem, int? firstYearOfProduction, int? lastYearOfProduction)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)?.Override == true)
        {
            return;
        }

        if (!firstYearOfProduction.HasValue || !lastYearOfProduction.HasValue)
        {
            CalculationHelper.ResetTimeSeries(caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile));
            return;
        }

        int firstYear = firstYearOfProduction.Value;
        int lastYear = lastYearOfProduction.Value;

        var facilityOpex = caseItem.Topside!.FacilityOpex;

        var values = new List<double>();

        if (facilityOpex > 0)
        {
            values.Add((facilityOpex - 1) / 8);
            values.Add((facilityOpex - 1) / 4);
            values.Add((facilityOpex - 1) / 2);

            for (int i = firstYear; i < lastYear; i++)
            {
                values.Add(facilityOpex);
            }
        }
        else
        {
            values.AddRange(Enumerable.Repeat(0.0, lastYear - firstYear + 3));
        }

        const int preOpexCostYearOffset = 3;

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.OffshoreFacilitiesOperationsCostProfile);

        profile.Values = values.ToArray();
        profile.StartYear = firstYear - preOpexCostYearOffset;
    }

    /*
    Calculates the cumulative number of wells drilled over time.
    Example:
    Input: [1, 2, 3, 4]
    Output: [1, 3, 6, 10]
    */
    private static TimeSeriesCost GetCumulativeDrillingSchedule(TimeSeriesCost drillingSchedule)
    {
        var cumulativeSchedule = new TimeSeriesCost
        {
            StartYear = drillingSchedule.StartYear
        };
        var values = new List<double>();
        var sum = 0.0;

        for (int i = 0; i < drillingSchedule.Values.Length; i++)
        {
            sum += drillingSchedule.Values[i];
            values.Add(sum);
        }

        cumulativeSchedule.Values = values.ToArray();

        return cumulativeSchedule;
    }
}
