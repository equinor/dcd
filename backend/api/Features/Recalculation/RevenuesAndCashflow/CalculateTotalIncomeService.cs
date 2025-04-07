using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Features.Recalculation.Helpers;
using api.Models;

using static api.Features.Profiles.CalculationConstants;

namespace api.Features.Recalculation.RevenuesAndCashflow;

public static class CalculateTotalIncomeService
{
    /// <summary>
    /// sum oil, gas and ngl income in usd
    /// </summary>
    public static void RunCalculation(Case caseItem)
    {
        var gasPriceNok = caseItem.Project.GasPriceNok;
        var oilPriceUsd = caseItem.Project.OilPriceUsd;
        var nglPriceUsd = caseItem.Project.NglPriceUsd;
        var usdToNok = caseItem.Project.ExchangeRateUsdToNok;
        var currency = caseItem.Project.Currency;

        var totalOilProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileOil);
        var oilIncome = EconomicsHelper.CalculateTotalOilIncome(totalOilProduction, oilPriceUsd, usdToNok, currency);

        var totalNglProduction = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.ProductionProfileNgl));
        var nglIncome = EconomicsHelper.CalculateTotalNglIncome(totalNglProduction, nglPriceUsd, usdToNok, currency);

        var totalGasProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileGas);
        var gasIncome = EconomicsHelper.CalculateTotalGasIncome(totalGasProduction, gasPriceNok, usdToNok, currency);

        var totalIncome = TimeSeriesMerger.MergeTimeSeries(oilIncome, nglIncome, gasIncome);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalIncomeCostProfile);

        profile.Values = totalIncome.Values;
        profile.StartYear = totalIncome.StartYear;
    }
}
