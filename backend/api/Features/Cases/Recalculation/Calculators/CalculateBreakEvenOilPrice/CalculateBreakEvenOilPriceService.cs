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
        var oilPrice = caseItem.Project.OilPriceUSD;
        var gasPriceNok = caseItem.Project.GasPriceNOK;
        var exchangeRateUsdToNok = caseItem.Project.ExchangeRateUSDToNOK;
        var calculatedTotalCostCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfile);

        var oilVolume = EconomicsHelper.MergeProductionAndAdditionalProduction(
            caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil),
            caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)
        );

        if (oilVolume.Values.Length == 0)
        {
            return;
        }

        oilVolume.Values = oilVolume.Values.Select(v => v / 1_000_000).ToArray();

        var netSalesGasProfile = caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas);
        var netSalesGasOverride = caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride);

        var netSalesGasVolume = netSalesGasOverride?.Override == true
            ? new TimeSeries { StartYear = netSalesGasOverride.StartYear, Values = netSalesGasOverride.Values }
            : new TimeSeries(netSalesGasProfile);


        netSalesGasVolume.Values = netSalesGasVolume.Values.Length != 0 ? netSalesGasVolume.Values.Select(v => v / 1_000_000_000).ToArray() : [];

        var currentYear = DateTime.Now.Year;
        var nextYear = currentYear + 1;
        var dg4Year = caseItem.DG4Date.Year;
        var nextYearInRelationToDg4Year = nextYear - dg4Year;
        var discountYearInRelationToDg4Year = caseItem.Project.NpvYear - dg4Year;

        var discountedOilVolume = EconomicsHelper.CalculateDiscountedVolume(
            oilVolume.Values,
            discountRate,
            oilVolume?.StartYear ?? 0,
            discountYearInRelationToDg4Year
        );

        var discountedNetSalesGasVolumeVolume = EconomicsHelper.CalculateDiscountedVolume(
            netSalesGasVolume.Values,
            discountRate,
            netSalesGasVolume?.StartYear ?? 0,
            discountYearInRelationToDg4Year
        );

        if (discountedOilVolume == 0 || discountedNetSalesGasVolumeVolume == 0)
        {
            return;
        }


        var discountedTotalCost = EconomicsHelper.CalculateDiscountedVolume(
            calculatedTotalCostCostProfile?.Values ?? [],
            discountRate,
            calculatedTotalCostCostProfile?.StartYear ?? 0, // discount factor should be applied from the year after discount year
            discountYearInRelationToDg4Year
        );

        var gor = discountedNetSalesGasVolumeVolume / discountedOilVolume;

        var pa = gasPriceNok > 0 ? gasPriceNok * 1000 / (exchangeRateUsdToNok * BarrelsPerCubicMeter * oilPrice) : 0;

        var breakEvenPrice = discountedTotalCost / ((gor * pa) + 1) / discountedOilVolume / BarrelsPerCubicMeter;
        caseItem.BreakEven = breakEvenPrice;

    }
}

