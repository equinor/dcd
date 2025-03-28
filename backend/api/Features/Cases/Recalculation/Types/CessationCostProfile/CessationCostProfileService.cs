using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.CessationCostProfile;

public static class CessationCostProfileService
{
    public static void RunCalculation(Case caseItem, List<CampaignWell> developmentWells)
    {
        var lastYearOfProduction = CalculationHelper.GetRelativeLastYearOfProduction(caseItem);

        CalculateCessationWellsCost(caseItem, developmentWells, lastYearOfProduction);
        GetCessationOffshoreFacilitiesCost(caseItem, lastYearOfProduction);
    }

    private static void CalculateCessationWellsCost(Case caseItem, List<CampaignWell> developmentWells, int? lastYear)
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

        GenerateCessationWellsCost(developmentWells, lastYear.Value, profile);
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

        GenerateCessationOffshoreFacilitiesCost(caseItem.Surf, lastYear.Value, profile);
    }

    public static void GenerateCessationWellsCost(
        List<CampaignWell> developmentWells,
        int lastYear,
        TimeSeriesProfile cessationWells)
    {
        var totalPluggingAndAbandonmentCost = developmentWells
            .Sum(campaignWell => campaignWell.Values.Sum() * campaignWell.Well.PlugingAndAbandonmentCost);

        cessationWells.StartYear = lastYear;
        cessationWells.Values =
        [
            totalPluggingAndAbandonmentCost / 2,
            totalPluggingAndAbandonmentCost / 2
        ];
    }

    private static void GenerateCessationOffshoreFacilitiesCost(Surf surf, int lastYear, TimeSeriesProfile cessationOffshoreFacilities)
    {
        var surfCessationCost = surf.CessationCost;

        cessationOffshoreFacilities.StartYear = lastYear + 1;
        cessationOffshoreFacilities.Values = [surfCessationCost / 2.0, surfCessationCost / 2.0];
    }
}
