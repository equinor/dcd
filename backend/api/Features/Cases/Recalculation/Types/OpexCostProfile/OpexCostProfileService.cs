using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.OpexCostProfile;

public static class OpexCostProfileService
{
    public static void RunCalculation(Case caseItem, List<CampaignWell> developmentWells)
    {
        var lastYearOfProduction = CalculationHelper.GetRelativeLastYearOfProduction(caseItem);
        var firstYearOfProduction = CalculationHelper.GetRelativeFirstYearOfProduction(caseItem);

        CalculateWellInterventionCostProfile(caseItem, developmentWells, firstYearOfProduction, lastYearOfProduction);
        CalculateOffshoreFacilitiesOperationsCostProfile(caseItem, firstYearOfProduction, lastYearOfProduction);
    }

    private static void CalculateWellInterventionCostProfile(Case caseItem, List<CampaignWell> developmentWells, int? firstYearOfProduction, int? lastYearOfProduction)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)?.Override == true)
        {
            return;
        }

        var firstYear = firstYearOfProduction ?? 0;
        var lastYear = lastYearOfProduction ?? 0;

        var initialYearsWithoutWellInterventionCost = caseItem.InitialYearsWithoutWellInterventionCost;
        var finalYearsWithoutWellInterventionCost = caseItem.FinalYearsWithoutWellInterventionCost;

        if (developmentWells.Count == 0)
        {
            CalculationHelper.ResetTimeSeries(caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile));

            return;
        }

        var wellInterventionCostsFromDrillingSchedules = developmentWells
            .Select(developmentWell => new TimeSeries
            {
                StartYear = developmentWell.StartYear,
                Values = developmentWell.Values.Select(v => (double)v).ToArray()
            })
            .ToList();

        var wellInterventionCostsFromDrillingSchedule = TimeSeriesMerger.MergeTimeSeries(wellInterventionCostsFromDrillingSchedules);

        var tempSeries = new TimeSeries
        {
            StartYear = firstYear,
            Values = wellInterventionCostsFromDrillingSchedule.Values
        };

        var cumulativeWellsDrilled = GetCumulativeWellsDrilled(tempSeries, lastYear);
        cumulativeWellsDrilled.StartYear = tempSeries.StartYear;

        var interventionCost = caseItem.Project.DevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell;
        var wellInterventionCostValues = cumulativeWellsDrilled.Values.Select(v => v * interventionCost).ToArray();

        var offset = Math.Abs(cumulativeWellsDrilled.StartYear - developmentWells.Min(w => w.StartYear));

        if (offset < wellInterventionCostValues.Length)
        {
            wellInterventionCostValues = wellInterventionCostValues.Skip(offset).ToArray();
        }

        for (var i = 0; i < wellInterventionCostValues.Length; i++)
        {
            wellInterventionCostValues[i] = i + offset < cumulativeWellsDrilled.Values.Length
                ? cumulativeWellsDrilled.Values[i + offset] * interventionCost
                : 0;
        }

        wellInterventionCostsFromDrillingSchedule.Values = wellInterventionCostValues;
        wellInterventionCostsFromDrillingSchedule.StartYear = cumulativeWellsDrilled.StartYear;

        var totalValuesCount = lastYear == 0
            ? wellInterventionCostsFromDrillingSchedule.Values.Length
            : lastYear - wellInterventionCostsFromDrillingSchedule.StartYear + 1;

        var additionalValuesCount = totalValuesCount - wellInterventionCostsFromDrillingSchedule.Values.Length;

        var additionalValues = new List<double>();

        for (var i = 0; i < additionalValuesCount; i++)
        {
            if (wellInterventionCostsFromDrillingSchedule.Values.Length > 0)
            {
                additionalValues.Add(wellInterventionCostsFromDrillingSchedule.Values.Last());
            }
        }

        var valuesList = wellInterventionCostsFromDrillingSchedule.Values.ToList();
        valuesList.AddRange(additionalValues);
        wellInterventionCostsFromDrillingSchedule.Values = valuesList.ToArray();

        if (initialYearsWithoutWellInterventionCost > 0 && wellInterventionCostsFromDrillingSchedule.Values.Length > initialYearsWithoutWellInterventionCost)
        {
            wellInterventionCostsFromDrillingSchedule.Values = wellInterventionCostsFromDrillingSchedule.Values.Skip((int)initialYearsWithoutWellInterventionCost).ToArray();
            wellInterventionCostsFromDrillingSchedule.StartYear += (int)initialYearsWithoutWellInterventionCost;
        }

        if (finalYearsWithoutWellInterventionCost > 0 && wellInterventionCostsFromDrillingSchedule.Values.Length > finalYearsWithoutWellInterventionCost)
        {
            wellInterventionCostsFromDrillingSchedule.Values = wellInterventionCostsFromDrillingSchedule.Values.Take(wellInterventionCostsFromDrillingSchedule.Values.Length - (int)finalYearsWithoutWellInterventionCost).ToArray();
        }

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

        var firstYear = firstYearOfProduction.Value;
        var lastYear = lastYearOfProduction.Value;

        var facilityOpex = caseItem.Topside.FacilityOpex;

        var values = new List<double>();

        values.AddRange(CalculateAnnualOpex(facilityOpex, firstYear, lastYear));

        const int preOpexCostYearOffset = 3;

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.OffshoreFacilitiesOperationsCostProfile);

        profile.Values = values.ToArray();
        profile.StartYear = firstYear - preOpexCostYearOffset;
    }

    private static List<double> CalculateAnnualOpex(double facilityOpex, int firstYear, int lastYear)
    {
        if (facilityOpex <= 0)
        {
            return Enumerable.Repeat(0.0, lastYear - firstYear + 3).ToList();
        }

        var values = new List<double>
        {
            (facilityOpex - 1) / 8,
            (facilityOpex - 1) / 4,
            (facilityOpex - 1) / 2
        };

        for (var i = firstYear; i < lastYear; i++)
        {
            values.Add(facilityOpex);
        }

        return values;
    }

    /*
    Calculates the cumulative number of wells drilled over time.
    Example:
    Input: [1, 2, 3, 4]
    Output: [1, 3, 6, 10]
    */
    private static TimeSeries GetCumulativeWellsDrilled(TimeSeries drillingSchedule, int lastYear)
    {
        var cumulativeSchedule = new TimeSeries
        {
            StartYear = drillingSchedule.StartYear
        };

        var values = new List<double>();
        var sum = 0.0;
        int currentYear = drillingSchedule.StartYear;

        var yearValueMap = drillingSchedule.Values
            .Select((value, index) => new { Year = drillingSchedule.StartYear + index, Value = value })
            .ToDictionary(x => x.Year, x => x.Value);

        for (var year = drillingSchedule.StartYear; year <= lastYear; year++)
        {
            if (yearValueMap.TryGetValue(year, out var value))
            {
                sum += value;
            }

            values.Add(sum);
        }

        cumulativeSchedule.Values = values.ToArray();

        return cumulativeSchedule;
    }
}
