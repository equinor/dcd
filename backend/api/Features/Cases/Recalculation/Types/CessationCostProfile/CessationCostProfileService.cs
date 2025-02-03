using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.CessationCostProfile;

public static class CessationCostProfileService
{
    public static void RunCalculation(Case caseItem, List<DevelopmentWell> developmentWells)
    {
        var lastYearOfProduction = CalculationHelper.GetRelativeLastYearOfProduction(caseItem);

        CalculateCessationWellsCost(caseItem, developmentWells, lastYearOfProduction);
        GetCessationOffshoreFacilitiesCost(caseItem, lastYearOfProduction);
    }

    private static void CalculateCessationWellsCost(Case caseItem, List<DevelopmentWell> developmentWells, int? lastYear)
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

        GenerateCessationWellsCost(caseItem.Project, developmentWells, lastYear.Value, profile);
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

    private static void GenerateCessationWellsCost(Project project, List<DevelopmentWell> developmentWells, int lastYear, TimeSeriesProfile cessationWells)
    {
        var pluggingAndAbandonment = project.DevelopmentOperationalWellCosts?.PluggingAndAbandonment ?? 0;

        var sumDrilledWells = developmentWells
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
