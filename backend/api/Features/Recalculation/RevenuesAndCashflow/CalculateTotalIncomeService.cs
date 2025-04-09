using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Features.Recalculation.Helpers;
using api.Models;

using static api.Features.Profiles.CalculationConstants;

namespace api.Features.Recalculation.RevenuesAndCashflow;

// dependency order 3
public static class CalculateTotalIncomeService
{
    /// <summary>
    /// sum oil, gas and ngl income
    /// </summary>
    public static void RunCalculation(Case caseItem)
    {
        var gasPriceNok = caseItem.Project.GasPriceNok;
        var nglPriceUsd = caseItem.Project.NglPriceUsd;
        var usdToNok = caseItem.Project.ExchangeRateUsdToNok;
        var currency = caseItem.Project.Currency;

        var oilIncome = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.CalculatedTotalOilIncomeCostProfile);

        var totalNglProduction = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.ProductionProfileNgl));
        var nglIncome = EconomicsHelper.CalculateTotalNglIncome(totalNglProduction, nglPriceUsd, usdToNok, currency);

        var netSalesGas = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.NetSalesGas));
        var gasIncome = EconomicsHelper.CalculateTotalGasIncome(netSalesGas, gasPriceNok, usdToNok, currency);

        var totalIncome = TimeSeriesMerger.MergeTimeSeries(oilIncome, nglIncome, gasIncome);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalIncomeCostProfile);

        profile.Values = totalIncome.Values;
        profile.StartYear = totalIncome.StartYear;
    }
}
