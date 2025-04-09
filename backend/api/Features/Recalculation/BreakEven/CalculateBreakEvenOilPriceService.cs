using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Recalculation.Helpers;
using api.Models;
using api.Models.Enums;

using static api.Features.Profiles.CalculationConstants;

namespace api.Features.Recalculation.BreakEven;

// dependency order 4
public static class CalculateBreakEvenOilPriceService
{
    public static void RunCalculation(Case caseItem)
    {
        var discountRate = caseItem.Project.DiscountRate;
        var oilPrice = caseItem.Project.OilPriceUsd;
        var gasPriceNok = caseItem.Project.GasPriceNok;
        var exchangeRateUsdToNok = caseItem.Project.ExchangeRateUsdToNok;
        var calculatedTotalCostCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfile);

        var oilVolume = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileOil);

        if (oilVolume.Values.Length == 0)
        {
            return;
        }

        oilVolume.Values = oilVolume.Values.Select(v => v / Mega).ToArray();

        var netSalesGasVolume = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.NetSalesGas));
        netSalesGasVolume.Values = netSalesGasVolume.Values.Select(v => v / Giga).ToArray();

        var dg4Year = caseItem.Dg4Date.Year;
        var discountYearInRelationToDg4Year = caseItem.Project.NpvYear - dg4Year;

        var discountedOilVolume = EconomicsHelper.CalculateSumOfDiscountedVolume(
            oilVolume.Values,
            discountRate,
            oilVolume.StartYear,
            discountYearInRelationToDg4Year
        );

        var discountedNetSalesGasVolume = EconomicsHelper.CalculateSumOfDiscountedVolume(
            netSalesGasVolume.Values,
            discountRate,
            netSalesGasVolume.StartYear,
            discountYearInRelationToDg4Year
        );

        if (discountedOilVolume == 0 || discountedNetSalesGasVolume == 0)
        {
            return;
        }

        var discountedTotalCost = EconomicsHelper.CalculateSumOfDiscountedVolume(
            calculatedTotalCostCostProfile?.Values ?? [],
            discountRate,
            calculatedTotalCostCostProfile?.StartYear ?? 0,
            discountYearInRelationToDg4Year
        );

        var gor = discountedNetSalesGasVolume / discountedOilVolume;

        var pa = gasPriceNok * (Giga / Mega) / (exchangeRateUsdToNok * BarrelsPerCubicMeter * oilPrice);

        var rate = caseItem.Project.Currency == Currency.Usd ? 1 : exchangeRateUsdToNok;

        var breakEvenPriceUsd = discountedTotalCost / (gor * pa + 1) / discountedOilVolume / BarrelsPerCubicMeter / rate;
        caseItem.BreakEven = breakEvenPriceUsd;
    }
}
