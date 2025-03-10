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
            StartYear = Math.Max(firstYear, wellInterventionCostsFromDrillingSchedule.StartYear),
            Values = wellInterventionCostsFromDrillingSchedule.Values
        };

        var cumulativWellsDrilled = GetCumulativeWellsDrilled(tempSeries, lastYear);
        cumulativWellsDrilled.StartYear = tempSeries.StartYear;

        var interventionCost = caseItem.Project.DevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell;


        var wellInterventionCostValues = cumulativWellsDrilled.Values.Select(v => v * interventionCost).ToArray();

        var offset = Math.Abs(developmentWells.First().StartYear) + (firstYearOfProduction ?? 0);

        if (offset < wellInterventionCostValues.Length)
        {
            wellInterventionCostValues = wellInterventionCostValues.Skip(offset).ToArray();
        }

        for (int i = 0; i < wellInterventionCostValues.Length; i++)
        {
            int currentYear = firstYear + i;

            if (i + offset < cumulativWellsDrilled.Values.Length)
            {
                wellInterventionCostValues[i] = cumulativWellsDrilled.Values[i + offset] * interventionCost;
            }
            else
            {
                wellInterventionCostValues[i] = 0;
            }
        }

        wellInterventionCostsFromDrillingSchedule.Values = wellInterventionCostValues;
        wellInterventionCostsFromDrillingSchedule.StartYear = cumulativWellsDrilled.StartYear;

        var totalValuesCount = lastYear == 0 ? wellInterventionCostsFromDrillingSchedule.Values.Length : lastYear - wellInterventionCostsFromDrillingSchedule.StartYear + 1;
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

        // Convert Values array to dictionary for easier year-based lookup
        var yearValueMap = drillingSchedule.Values
            .Select((value, index) => new { Year = drillingSchedule.StartYear + index, Value = value })
            .ToDictionary(x => x.Year, x => x.Value);

        // Fill values from StartYear to lastYear
        for (int year = drillingSchedule.StartYear; year <= lastYear; year++)
        {
            if (yearValueMap.TryGetValue(year, out double value))
            {
                sum += value;
            }

            values.Add(sum); // Store cumulative sum
        }

        cumulativeSchedule.Values = values.ToArray();
        return cumulativeSchedule;
    }

}
