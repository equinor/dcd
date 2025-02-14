using api.Features.Profiles;
using api.Models;

namespace api.Features.Cases.Recalculation.Calculators.GenerateCo2Intensity;

public static class AverageCo2IntensityProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var co2IntensityData = caseItem.GetProfileOrNull(ProfileTypes.Co2Intensity);
        var oilProductionSum = Co2IntensityProfileService.GetOilProfile(caseItem).Values.Sum();
        var netSalesGasSum = caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas)?.Values.Sum() / 1_000_000_000 ?? 0;
        var co2EmissionsProfile = Co2IntensityProfileService.GetCo2EmissionsProfile(caseItem).Values.Sum() / 1_000_000;

        if (co2IntensityData != null && co2IntensityData.Values.Any())
        {
            var denominator = (oilProductionSum + netSalesGasSum) * 6.29;

            if (denominator > 0)
            {
                caseItem.AverageCo2Intensity = co2EmissionsProfile * 1000 / denominator;
                return;
            }
        }

        caseItem.AverageCo2Intensity = 0;
    }
}
