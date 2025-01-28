using api.Context;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.TimeSeriesCalculators;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.OpexCostProfile;

public class OpexCostProfileService(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(c => c.TimeSeriesProfiles)
            .SingleAsync(c => c.Id == caseId);

        var project = await context.Projects
            .Include(p => p.Cases)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .SingleAsync(p => p.Id == caseItem.ProjectId);

        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.AdditionalProductionProfileOil)
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileGas)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        var lastYearOfProduction = CalculationHelper.GetRelativeLastYearOfProduction(caseItem, drainageStrategy);
        var firstYearOfProduction = CalculationHelper.GetRelativeFirstYearOfProduction(caseItem, drainageStrategy);

        var linkedWells = await context.WellProjectWell
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.WellProjectId == caseItem.WellProjectLink)
            .ToListAsync();

        var topside = await context.Topsides.SingleAsync(x => x.Id == caseItem.TopsideLink);

        CalculateWellInterventionCostProfile(caseItem, project, linkedWells, lastYearOfProduction);
        CalculateOffshoreFacilitiesOperationsCostProfile(caseItem, topside, firstYearOfProduction, lastYearOfProduction);
    }

    public static void CalculateWellInterventionCostProfile(Case caseItem, Project project, List<WellProjectWell> linkedWells, int? lastYearOfProduction)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)?.Override == true)
        {
            return;
        }

        var lastYear = lastYearOfProduction ?? 0;

        if (linkedWells.Count == 0)
        {
            CalculationHelper.ResetTimeSeries(caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile));
            return;
        }

        var wellInterventionCostsFromDrillingSchedule = new TimeSeries<double>();
        foreach (var linkedWell in linkedWells)
        {
            if (linkedWell.DrillingSchedule == null) { continue; }

            var timeSeries = new TimeSeries<double>
            {
                StartYear = linkedWell.DrillingSchedule.StartYear,
                Values = linkedWell.DrillingSchedule.Values.Select(v => (double)v).ToArray()
            };
            wellInterventionCostsFromDrillingSchedule = CostProfileMerger.MergeCostProfiles(wellInterventionCostsFromDrillingSchedule, timeSeries);
        }

        var tempSeries = new TimeSeries<int>
        {
            StartYear = wellInterventionCostsFromDrillingSchedule.StartYear,
            Values = wellInterventionCostsFromDrillingSchedule.Values.Select(v => (int)v).ToArray()
        };

        var cumulativeDrillingSchedule = GetCumulativeDrillingSchedule(tempSeries);
        cumulativeDrillingSchedule.StartYear = tempSeries.StartYear;

        var interventionCost = project.DevelopmentOperationalWellCosts?.AnnualWellInterventionCostPerWell ?? 0;

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

    public static void CalculateOffshoreFacilitiesOperationsCostProfile(Case caseItem, Topside topside, int? firstYearOfProduction, int? lastYearOfProduction)
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

        var facilityOpex = topside.FacilityOpex;

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
    private static TimeSeries<double> GetCumulativeDrillingSchedule(TimeSeries<int> drillingSchedule)
    {
        var cumulativeSchedule = new TimeSeries<double>
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
