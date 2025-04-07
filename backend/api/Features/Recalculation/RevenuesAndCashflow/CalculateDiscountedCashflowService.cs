using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Features.Recalculation.Helpers;
using api.Models;

namespace api.Features.Recalculation.RevenuesAndCashflow;

public static class CalculatedDiscountedCashflowService
{
    public static void RunCalculation(Case caseItem)
    {
        var discountRate = caseItem.Project.DiscountRate;

        var totalCost = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfile);

        if (caseItem.Project.Currency == Models.Enums.Currency.Nok && totalCost != null)
        {
            totalCost.Values = totalCost.Values
                .Select(v => v * caseItem.Project.ExchangeRateUsdToNok)
                .ToArray();
        }

        if (totalCost == null)
        {
            return;
        }

        var discountedTotalCost = CalculateDiscountedTotalCost(caseItem, totalCost, discountRate);
        var negatedDiscountedTotalCost = discountedTotalCost.Values.Select(v => -v).ToArray();

        var discountedLiquidsRevenue = CalculateDiscountedLiquidsRevenue(caseItem, discountRate);
        var discountedGasRevenue = CalculateDiscountedGasRevenue(caseItem, discountRate);

        var calculateDiscountedCashflow = TimeSeriesMerger.MergeTimeSeries(
            new TimeSeries { StartYear = discountedTotalCost.StartYear, Values = negatedDiscountedTotalCost },
            new TimeSeries { StartYear = discountedLiquidsRevenue.StartYear, Values = discountedLiquidsRevenue.Values },
            new TimeSeries { StartYear = discountedGasRevenue.StartYear, Values = discountedGasRevenue.Values }
        ).Values;

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedDiscountedCashflow);
        profile.Values = calculateDiscountedCashflow;
        profile.StartYear = Math.Min(discountedTotalCost.StartYear, Math.Min(discountedLiquidsRevenue.StartYear, discountedGasRevenue.StartYear));
    }

    private static TimeSeries CalculateDiscountedTotalCost(Case caseItem, TimeSeriesProfile totalCost, double discountRate)
    {
        return EconomicsHelper.CalculateDiscountedVolume(
            totalCost.Values,
            discountRate,
            totalCost.StartYear,
            caseItem.Project.NpvYear - caseItem.Dg4Date.Year
        );
    }

    private static TimeSeries CalculateDiscountedLiquidsRevenue(Case caseItem, double discountRate)
    {
        var oilProduction = TimeSeriesMerger.MergeTimeSeries(
            new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)),
            new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil))
        );

        var nglProduction = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.ProductionProfileNgl));

        var convertCurrency = caseItem.Project.Currency == Models.Enums.Currency.Nok
            ? caseItem.Project.ExchangeRateUsdToNok
            : 1;

        var oilProductionRevenue = oilProduction.Values.Select(v => v / 1_000_000 * caseItem.Project.OilPriceUsd * CalculationConstants.BarrelsPerCubicMeter * convertCurrency).ToArray();
        var nglProductionRevenue = nglProduction.Values.Select(v => v / 1_000_000 * caseItem.Project.NglPriceUsd * convertCurrency).ToArray();

        var liquidsRevenue = TimeSeriesMerger.MergeTimeSeries(
            new TimeSeries
            {
                StartYear = oilProduction.StartYear,
                Values = oilProductionRevenue
            },
            new TimeSeries
            {
                StartYear = nglProduction.StartYear,
                Values = nglProductionRevenue
            }
        );

        return EconomicsHelper.CalculateDiscountedVolume(
            liquidsRevenue.Values,
            discountRate,
            liquidsRevenue.StartYear,
            caseItem.Project.NpvYear - caseItem.Dg4Date.Year
        );
    }

    private static TimeSeries CalculateDiscountedGasRevenue(Case caseItem, double discountRate)
    {
        var gasProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileGas);

        var convertCurrency = caseItem.Project.Currency == Models.Enums.Currency.Nok
            ? 1
            : caseItem.Project.ExchangeRateUsdToNok;

        var gasRevenue = gasProduction.Values
            .Select(v => v / 1_000_000_000 * 1000 * caseItem.Project.GasPriceNok / convertCurrency)
            .ToArray();

        return EconomicsHelper.CalculateDiscountedVolume(
            gasRevenue,
            discountRate,
            gasProduction.StartYear,
            caseItem.Project.NpvYear - caseItem.Dg4Date.Year
        );
    }
}
