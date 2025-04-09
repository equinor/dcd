using api.Features.Profiles;
using api.Features.Recalculation.Helpers;
using api.Models;

namespace api.Features.Recalculation.Production;

// dependency order 1
public static class FuelFlaringLossesProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var fuelFlaringAndLosses = EmissionCalculationHelper.CalculateFuelFlaringAndLosses(caseItem);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.FuelFlaringAndLosses);

        profile.StartYear = fuelFlaringAndLosses.StartYear;
        profile.Values = fuelFlaringAndLosses.Values;
    }
}
