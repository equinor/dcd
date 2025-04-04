using api.Features.Profiles;
using api.Models;

using static api.Features.Profiles.VolumeConstants;

namespace api.Features.Cases.Recalculation.Co2;

public static class AverageCo2IntensityProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var co2IntensityData = Co2IntensityProfileService.GetCo2IntensityProfile(caseItem);
        var oilProductionSum = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileOil).Values.Sum() / Mega;
        var netSalesGasSum = caseItem.GetOverrideProfileOrProfile(ProfileTypes.NetSalesGas)?.Values.Sum() / Giga ?? 0;
        var co2EmissionsSum = Co2IntensityProfileService.GetCo2EmissionsProfile(caseItem).Values.Sum() / Mega;

        if (co2IntensityData.Values.Any())
        {
            var denominator = (oilProductionSum + netSalesGasSum) * BarrelsPerCubicMeter;

            if (denominator > 0)
            {
                caseItem.AverageCo2Intensity = co2EmissionsSum * 1000 / denominator;

                return;
            }
        }

        caseItem.AverageCo2Intensity = 0;
    }
}
