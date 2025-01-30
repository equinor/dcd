using api.Context;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.CessationCostProfile;

public class CessationCostProfileService(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(x => x.Project).ThenInclude(x => x.DevelopmentOperationalWellCosts)
            .Include(x => x.Surf)
            .SingleAsync(x => x.Id == caseId);

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        var drillingSchedulesForWellProjectWell = await context.WellProjectWell
            .Where(w => w.WellProjectId == caseItem.WellProjectLink)
            .Select(x => x.DrillingSchedule)
            .Where(x => x != null)
            .Select(x => x!)
            .ToListAsync();

        RunCalculation(caseItem, drillingSchedulesForWellProjectWell);
    }

    public static void RunCalculation(Case caseItem, List<DrillingSchedule> drillingSchedulesForWellProjectWell)
    {
        var lastYearOfProduction = CalculationHelper.GetRelativeLastYearOfProduction(caseItem);

        CalculateCessationWellsCost(caseItem, drillingSchedulesForWellProjectWell, lastYearOfProduction);
        GetCessationOffshoreFacilitiesCost(caseItem, lastYearOfProduction);
    }

    private static void CalculateCessationWellsCost(Case caseItem, List<DrillingSchedule> drillingSchedulesForWellProjectWell, int? lastYear)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)?.Override == true)
        {
            return;
        }

        if (!lastYear.HasValue)
        {
            CalculationHelper.ResetTimeSeries(caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost));
            return;
        }

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.CessationWellsCost);

        GenerateCessationWellsCost(caseItem.Project, drillingSchedulesForWellProjectWell, lastYear.Value, profile);
    }

    private static void GetCessationOffshoreFacilitiesCost(Case caseItem, int? lastYear)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)?.Override == true)
        {
            return;
        }

        if (!lastYear.HasValue)
        {
            CalculationHelper.ResetTimeSeries(caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost));
            return;
        }

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.CessationOffshoreFacilitiesCost);

        GenerateCessationOffshoreFacilitiesCost(caseItem.Surf!, lastYear.Value, profile);
    }

    private static void GenerateCessationWellsCost(Project project, List<DrillingSchedule> drillingSchedulesForWellProjectWell, int lastYear, TimeSeriesProfile cessationWells)
    {
        var pluggingAndAbandonment = project.DevelopmentOperationalWellCosts?.PluggingAndAbandonment ?? 0;

        var sumDrilledWells = drillingSchedulesForWellProjectWell
            .Select(x => x.Values.Sum())
            .Sum();

        var totalCost = sumDrilledWells * pluggingAndAbandonment;
        cessationWells.StartYear = lastYear;
        cessationWells.Values = [totalCost / 2, totalCost / 2];
    }

    private static void GenerateCessationOffshoreFacilitiesCost(Surf surf, int lastYear, TimeSeriesProfile cessationOffshoreFacilities)
    {
        var surfCessationCost = surf.CessationCost;

        cessationOffshoreFacilities.StartYear = lastYear + 1;
        cessationOffshoreFacilities.Values = [surfCessationCost / 2, surfCessationCost / 2];
    }
}
