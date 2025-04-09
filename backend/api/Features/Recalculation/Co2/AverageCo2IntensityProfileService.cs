using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

using static api.Features.Profiles.CalculationConstants;

namespace api.Features.Recalculation.Co2;

// dependency order 6
public static class AverageCo2IntensityProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var co2IntensityData = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.Co2Intensity));
        var oilProductionSum = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileOil).Values.Sum() / Mega;
        var netSalesGasSum = caseItem.GetOverrideProfileOrProfile(ProfileTypes.NetSalesGas)?.Values.Sum() / Giga ?? 0;
        var co2EmissionsSum = caseItem.GetOverrideProfileOrProfile(ProfileTypes.Co2Emissions)?.Values.Sum() / Mega ?? 0;

        if (co2IntensityData.Values.Length != 0)
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
