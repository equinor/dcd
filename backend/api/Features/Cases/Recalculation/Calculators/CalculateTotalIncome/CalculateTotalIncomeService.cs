using api.Features.Cases.Recalculation.Calculators.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.TimeSeriesCalculators;
using api.Models;

namespace api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;

public static class CalculateTotalIncomeService
{
    public static void RunCalculation(Case caseItem)
    {
        var gasPriceNok = caseItem.Project.GasPriceNOK;
        var oilPrice = caseItem.Project.OilPriceUSD;
        var exchangeRateUsdToNok = caseItem.Project.ExchangeRateUSDToNOK;
        var cubicMetersToBarrelsFactor = 6.29;

        var totalOilProductionInMegaCubics = EconomicsHelper.MergeProductionAndAdditionalProduction(
            caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil),
            caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)
        );

        // Convert oil production from million sm³ to barrels in millions
        var oilProductionInMillionsOfBarrels = totalOilProductionInMegaCubics.Values.Select(v => v * cubicMetersToBarrelsFactor).ToArray();

        var oilIncome = new TimeSeriesCost
        {
            StartYear = totalOilProductionInMegaCubics.StartYear,
            Values = oilProductionInMillionsOfBarrels.Select(v => v * oilPrice * exchangeRateUsdToNok).ToArray(),
        };

        var totalGasProductionInGigaCubics = EconomicsHelper.MergeProductionAndAdditionalProduction(
            caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas),
            caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)
        );

        var gasIncome = new TimeSeriesCost
        {
            StartYear = totalGasProductionInGigaCubics.StartYear,
            Values = totalGasProductionInGigaCubics.Values.Select(v => v * gasPriceNok).ToArray()
        };

        var totalIncome = TimeSeriesMerger.MergeTimeSeries(oilIncome, gasIncome);

        // Divide the totalIncome by 1 million before assigning it to CalculatedTotalIncomeCostProfile to get correct unit
        var scaledTotalIncomeValues = totalIncome.Values.Select(v => v / 1_000_000).ToArray();

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalIncomeCostProfile);

        profile.Values = scaledTotalIncomeValues;
        profile.StartYear = totalIncome.StartYear;
    }
}

