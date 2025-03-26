using api.Features.Profiles;
using api.Models;

using static api.Features.Profiles.VolumeConstants;

namespace api.Features.Cases.Recalculation.Calculators.GenerateCo2Intensity;

public static class AverageCo2IntensityProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var co2IntensityData = Co2IntensityProfileService.GetCo2IntensityProfile(caseItem);
        var oilProductionSum = Co2IntensityProfileService.GetOilProfile(caseItem).Values.Sum();
        var netSalesGasSum = caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas)?.Values.Sum() / 1_000_000_000 ?? 0;
        var co2EmissionsProfile = Co2IntensityProfileService.GetCo2EmissionsProfile(caseItem).Values.Sum() / 1_000_000;

        if (co2IntensityData.Values.Any())
        {
            var denominator = (oilProductionSum + netSalesGasSum) * BarrelsPerCubicMeter;

            if (denominator > 0)
            {
                caseItem.AverageCo2Intensity = co2EmissionsProfile * 1000 / denominator;

                return;
            }
        }

        caseItem.AverageCo2Intensity = 0;
    }
}
