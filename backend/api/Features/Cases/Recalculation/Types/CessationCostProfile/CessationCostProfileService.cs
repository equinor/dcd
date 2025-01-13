using api.Context;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.CessationCostProfile;

public class CessationCostProfileService(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .SingleAsync(x => x.Id == caseId);

        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.ProductionProfileOil)
            .Include(d => d.AdditionalProductionProfileOil)
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileGas)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        var surf = await context.Surfs.SingleAsync(x => x.Id == caseItem.SurfLink);

        var linkedWells = await context.WellProjectWell
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.WellProjectId == caseItem.WellProjectLink)
            .ToListAsync();

        var project = await context.Projects
            .Include(p => p.Cases)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .SingleAsync(p => p.Id == caseItem.ProjectId);

        var lastYearOfProduction = CalculationHelper.GetRelativeLastYearOfProduction(drainageStrategy);


        CalculateCessationWellsCost(caseItem, project,linkedWells, lastYearOfProduction);
        GetCessationOffshoreFacilitiesCost(caseItem, surf, lastYearOfProduction);
    }

    private static void CalculateCessationWellsCost(Case caseItem, Project project, List<WellProjectWell> linkedWells, int? lastYear)
    {
        if (caseItem.CessationWellsCostOverride?.Override == true)
        {
            return;
        }

        if (!lastYear.HasValue)
        {
            CalculationHelper.ResetTimeSeries(caseItem.CessationWellsCost);
            return;
        }

        caseItem.CessationWellsCost = GenerateCessationWellsCost(
            project,
            linkedWells,
            lastYear.Value,
            caseItem.CessationWellsCost ?? new CessationWellsCost()
        );
    }

    private static void GetCessationOffshoreFacilitiesCost(Case caseItem, Surf surf, int? lastYear)
    {
        if (caseItem.CessationOffshoreFacilitiesCostOverride?.Override == true)
        {
            return;
        }

        if (!lastYear.HasValue)
        {
            CalculationHelper.ResetTimeSeries(caseItem.CessationOffshoreFacilitiesCost);
            return;
        }

        caseItem.CessationOffshoreFacilitiesCost = GenerateCessationOffshoreFacilitiesCost(
            surf,
            lastYear.Value,
            caseItem.CessationOffshoreFacilitiesCost ?? new CessationOffshoreFacilitiesCost()
        );
    }

    private static CessationWellsCost GenerateCessationWellsCost(Project project, List<WellProjectWell> linkedWells, int lastYear, CessationWellsCost cessationWells)
    {
        var pluggingAndAbandonment = project.DevelopmentOperationalWellCosts?.PluggingAndAbandonment ?? 0;

        var sumDrilledWells = linkedWells
            .Select(well => well.DrillingSchedule?.Values.Sum() ?? 0)
            .Sum();

        var totalCost = sumDrilledWells * pluggingAndAbandonment;
        cessationWells.StartYear = lastYear;
        cessationWells.Values = [totalCost / 2, totalCost / 2];

        return cessationWells;
    }

    private static CessationOffshoreFacilitiesCost GenerateCessationOffshoreFacilitiesCost(Surf surf, int lastYear, CessationOffshoreFacilitiesCost cessationOffshoreFacilities)
    {
        var surfCessationCost = surf.CessationCost;

        cessationOffshoreFacilities.StartYear = lastYear + 1;
        cessationOffshoreFacilities.Values = [surfCessationCost / 2, surfCessationCost / 2];
        return cessationOffshoreFacilities;
    }
}
