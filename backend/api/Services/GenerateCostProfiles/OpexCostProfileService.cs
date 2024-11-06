using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;

using AutoMapper;

namespace api.Services;

public class OpexCostProfileService(
    ICaseService caseService,
    IProjectService projectService,
    IDrainageStrategyService drainageStrategyService,
    IWellProjectWellService wellProjectWellService,
    ITopsideService topsideService)
    : IOpexCostProfileService
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await caseService.GetCase(caseId);
        var project = await projectService.GetProjectWithoutAssets(caseItem.ProjectId);

        var drainageStrategy = await drainageStrategyService.GetDrainageStrategyWithIncludes(
            caseItem.DrainageStrategyLink,
            d => d.ProductionProfileOil!,
            d => d.AdditionalProductionProfileOil!,
            d => d.ProductionProfileGas!,
            d => d.AdditionalProductionProfileGas!
        );

        var lastYearOfProduction = CalculationHelper.GetRelativeLastYearOfProduction(drainageStrategy);
        var firstYearOfProduction = CalculationHelper.GetRelativeFirstYearOfProduction(drainageStrategy);

        await CalculateWellInterventionCostProfile(caseItem, project, lastYearOfProduction);
        await CalculateOffshoreFacilitiesOperationsCostProfile(caseItem, firstYearOfProduction, lastYearOfProduction);
    }

    public async Task CalculateWellInterventionCostProfile(Case caseItem, Project project, int? lastYearofProduction)
    {
        if (caseItem.WellInterventionCostProfileOverride?.Override == true)
        {
            return;
        }

        var lastYear = lastYearofProduction ?? 0;

        var linkedWells = await wellProjectWellService.GetWellProjectWellsForWellProject(caseItem.WellProjectLink);
        if (linkedWells.Count == 0)
        {
            CalculationHelper.ResetTimeSeries(caseItem.WellInterventionCostProfile);
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
            wellInterventionCostsFromDrillingSchedule = TimeSeriesCost.MergeCostProfiles(wellInterventionCostsFromDrillingSchedule, timeSeries);
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

        var result = new WellInterventionCostProfile
        {
            Values = wellInterventionCostsFromDrillingSchedule.Values,
            StartYear = wellInterventionCostsFromDrillingSchedule.StartYear,
        };

        if (caseItem.WellInterventionCostProfile != null)
        {
            caseItem.WellInterventionCostProfile.Values = result.Values;
            caseItem.WellInterventionCostProfile.StartYear = result.StartYear;
        }
        else
        {
            caseItem.WellInterventionCostProfile = result;
        }
    }

    public async Task CalculateOffshoreFacilitiesOperationsCostProfile(Case caseItem, int? firstYearOfProduction, int? lastYearofProduction)
    {
        if (caseItem.OffshoreFacilitiesOperationsCostProfileOverride?.Override == true)
        {
            return;
        }

        if (!firstYearOfProduction.HasValue || !lastYearofProduction.HasValue)
        {
            CalculationHelper.ResetTimeSeries(caseItem.OffshoreFacilitiesOperationsCostProfile);
            return;
        }

        int firstYear = firstYearOfProduction.Value;
        int lastYear = lastYearofProduction.Value;

        var topside = await topsideService.GetTopsideWithIncludes(caseItem.TopsideLink);

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

        var offshoreFacilitiesOperationsCost = new OffshoreFacilitiesOperationsCostProfile
        {
            StartYear = firstYear - preOpexCostYearOffset,
            Values = values.ToArray()
        };

        if (caseItem.OffshoreFacilitiesOperationsCostProfile != null)
        {
            caseItem.OffshoreFacilitiesOperationsCostProfile.Values = offshoreFacilitiesOperationsCost.Values;
            caseItem.OffshoreFacilitiesOperationsCostProfile.StartYear = offshoreFacilitiesOperationsCost.StartYear;
        }
        else
        {
            caseItem.OffshoreFacilitiesOperationsCostProfile = offshoreFacilitiesOperationsCost;
        }
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
