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

    public static void CalculateWellInterventionCostProfile(Case caseItem, List<CampaignWell> developmentWells, int? firstYearOfProduction, int? lastYearOfProduction)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)?.Override == true)
        {
            return;
        }

        if (developmentWells.Count == 0)
        {
            CalculationHelper.ResetTimeSeries(caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile));

            return;
        }

        var lastYear = lastYearOfProduction ?? 0;

        var initialYearsWithoutWellInterventionCost = caseItem.InitialYearsWithoutWellInterventionCost;
        var finalYearsWithoutWellInterventionCost = caseItem.FinalYearsWithoutWellInterventionCost;

        var wellWithCumulativeDrillingSchedule = developmentWells
            .ToDictionary(
                developmentWell => developmentWell.Well,
                developmentWell => new TimeSeries
                {
                    StartYear = developmentWell.StartYear,
                    Values = GetCumulativeWellsDrilled(developmentWell, lastYear).Values
                }
            );

        var wellInterventionCosts = new List<TimeSeries>();

        foreach (var (well, cumulativeDrillingSchedule) in wellWithCumulativeDrillingSchedule)
        {
            var wellInterventionCost = new TimeSeries
            {
                StartYear = cumulativeDrillingSchedule.StartYear,
                Values = cumulativeDrillingSchedule.Values.Select(value => value * well.WellInterventionCost).ToArray()
            };

            wellInterventionCosts.Add(wellInterventionCost);
        }

        var caseWellInterventionCost = TimeSeriesMerger.MergeTimeSeries(wellInterventionCosts);

        if (initialYearsWithoutWellInterventionCost > 0 && caseWellInterventionCost.Values.Length > initialYearsWithoutWellInterventionCost)
        {
            caseWellInterventionCost.Values = caseWellInterventionCost.Values.Skip((int)initialYearsWithoutWellInterventionCost).ToArray();
            caseWellInterventionCost.StartYear += (int)initialYearsWithoutWellInterventionCost;
        }

        if (finalYearsWithoutWellInterventionCost > 0 && caseWellInterventionCost.Values.Length > finalYearsWithoutWellInterventionCost)
        {
            caseWellInterventionCost.Values = caseWellInterventionCost.Values.Take(caseWellInterventionCost.Values.Length - (int)finalYearsWithoutWellInterventionCost).ToArray();
        }

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.WellInterventionCostProfile);
        profile.Values = caseWellInterventionCost.Values;
        profile.StartYear = caseWellInterventionCost.StartYear;
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
    private static TimeSeries GetCumulativeWellsDrilled(CampaignWell campaignWell, int lastYear)
    {
        var cumulativeSchedule = new TimeSeries
        {
            StartYear = campaignWell.StartYear
        };

        var values = new List<double>();
        var sum = 0.0;

        var yearValueMap = campaignWell.Values
            .Select((value, index) => new { Year = campaignWell.StartYear + index, Value = value })
            .ToDictionary(x => x.Year, x => x.Value);

        for (var year = campaignWell.StartYear; year <= lastYear; year++)
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
