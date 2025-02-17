using api.Features.Cases.Recalculation.Calculators.Helpers;
using api.Features.Profiles;
using api.Models;

using static api.Features.Profiles.VolumeConstants;

namespace api.Features.Cases.Recalculation.Calculators.CalculateBreakEvenOilPrice;

public static class CalculateBreakEvenOilPriceService
{
    public static void RunCalculation(Case caseItem)
    {
        var discountRate = caseItem.Project.DiscountRate;
        var defaultOilPrice = caseItem.Project.OilPriceUSD;
        var gasPriceNok = caseItem.Project.GasPriceNOK;
        var exchangeRateUsdToNok = caseItem.Project.ExchangeRateUSDToNOK;

        var oilVolume = EconomicsHelper.MergeProductionAndAdditionalProduction(
            caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil),
            caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)
        );

        if (oilVolume.Values.Length == 0)
        {
            return;
        }

        oilVolume.Values = oilVolume.Values.Select(v => v / 1_000_000).ToArray();

        var gasVolume = EconomicsHelper.MergeProductionAndAdditionalProduction(
            caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas),
            caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)
        );

        gasVolume.Values = gasVolume.Values.Length != 0 ? gasVolume.Values.Select(v => v / 1_000_000_000).ToArray() : [];

        var currentYear = DateTime.Now.Year;
        var nextYear = currentYear + 1;
        var dg4Year = caseItem.DG4Date.Year;
        var nextYearInRelationToDg4Year = nextYear - dg4Year;

        var discountedGasVolume = EconomicsHelper.CalculateDiscountedVolume(
            gasVolume.Values,
            discountRate,
            gasVolume.StartYear + Math.Abs(nextYearInRelationToDg4Year)
        );

        var discountedOilVolume = EconomicsHelper.CalculateDiscountedVolume(
            oilVolume.Values,
            discountRate,
            oilVolume.StartYear + Math.Abs(nextYearInRelationToDg4Year)
        );

        if (discountedOilVolume == 0 || discountedGasVolume == 0)
        {
            return;
        }

        var calculatedTotalCostCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfile);

        var discountedTotalCost = EconomicsHelper.CalculateDiscountedVolume(
            calculatedTotalCostCostProfile?.Values ?? [],
            discountRate,
            (calculatedTotalCostCostProfile?.StartYear ?? 0) + Math.Abs(nextYearInRelationToDg4Year)
        );

        var gor = discountedGasVolume / discountedOilVolume;

        var pa = gasPriceNok > 0 ? gasPriceNok * 1000 / (exchangeRateUsdToNok * BarrelsPerCubicMeter * defaultOilPrice) : 0;

        var breakEvenPrice = discountedTotalCost / ((gor * pa) + 1) / discountedOilVolume / BarrelsPerCubicMeter;

        caseItem.BreakEven = breakEvenPrice;
    }
}

