using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.FuelFlaringLossesProfile;

public class FuelFlaringLossesProfileService : ICalculationService
{
    public void RunCalculation(CaseWithCampaignWells caseWithCampaignWells)
    {
        var caseItem = caseWithCampaignWells.CaseItem;
        if (caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLossesOverride)?.Override == true)
        {
            return;
        }

        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem);
        var flaring = EmissionCalculationHelper.CalculateFlaring(caseItem);
        var losses = EmissionCalculationHelper.CalculateLosses(caseItem);

        var total = TimeSeriesMerger.MergeTimeSeries(fuelConsumptions, flaring, losses);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.FuelFlaringAndLosses);

        profile.StartYear = total.StartYear;
        profile.Values = total.Values;
    }
}
