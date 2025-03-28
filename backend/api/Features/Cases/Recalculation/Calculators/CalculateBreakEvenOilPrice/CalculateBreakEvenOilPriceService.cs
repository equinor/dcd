using api.Features.Cases.Recalculation.Calculators.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

using static api.Features.Profiles.VolumeConstants;

namespace api.Features.Cases.Recalculation.Calculators.CalculateBreakEvenOilPrice;

public static class CalculateBreakEvenOilPriceService
{
    public static void RunCalculation(Case caseItem)
    {
        var discountRate = caseItem.Project.DiscountRate;
        var oilPrice = caseItem.Project.OilPriceUsd;
        var gasPriceNok = caseItem.Project.GasPriceNok;
        var exchangeRateUsdToNok = caseItem.Project.ExchangeRateUsdToNok;
        var calculatedTotalCostCostProfileUsd = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfileUsd);

        var oilVolume = EconomicsHelper.MergeProductionAndAdditionalProduction(
            caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil),
            caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)
        );

        if (oilVolume.Values.Length == 0)
        {
            return;
        }

        oilVolume.Values = oilVolume.Values.Select(v => v / 1_000_000).ToArray();

        var netSalesGasProfile = caseItem.GetOverrideProfileOrProfile(ProfileTypes.NetSalesGas);

        var netSalesGasVolume = new TimeSeries(netSalesGasProfile);

        netSalesGasVolume.Values = netSalesGasVolume.Values.Select(v => v / 1_000_000_000).ToArray();

        var dg4Year = caseItem.Dg4Date.Year;
        var discountYearInRelationToDg4Year = caseItem.Project.NpvYear - dg4Year;

        var discountedOilVolume = EconomicsHelper.CalculateSumOfDiscountedVolume(
            oilVolume.Values,
            discountRate,
            oilVolume.StartYear,
            discountYearInRelationToDg4Year
        );

        var discountedNetSalesGasVolumeVolume = EconomicsHelper.CalculateSumOfDiscountedVolume(
            netSalesGasVolume.Values,
            discountRate,
            netSalesGasVolume.StartYear,
            discountYearInRelationToDg4Year
        );

        if (discountedOilVolume == 0 || discountedNetSalesGasVolumeVolume == 0)
        {
            return;
        }

        var discountedTotalCost = EconomicsHelper.CalculateSumOfDiscountedVolume(
            calculatedTotalCostCostProfileUsd?.Values ?? [],
            discountRate,
            calculatedTotalCostCostProfileUsd?.StartYear ?? 0, // discount factor should be applied from the year after discount year
            discountYearInRelationToDg4Year
        );

        var gor = discountedNetSalesGasVolumeVolume / discountedOilVolume;

        var pa = gasPriceNok > 0 ? gasPriceNok * 1000 / (exchangeRateUsdToNok * BarrelsPerCubicMeter * oilPrice) : 0;

        var breakEvenPrice = discountedTotalCost / (gor * pa + 1) / discountedOilVolume / BarrelsPerCubicMeter;
        caseItem.BreakEven = breakEvenPrice;
    }
}
