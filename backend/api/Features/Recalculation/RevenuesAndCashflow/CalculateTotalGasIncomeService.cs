using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Recalculation.Helpers;
using api.Models;

namespace api.Features.Recalculation.RevenuesAndCashflow;
// dependency order 3

public static class CalculateTotalGasIncomeService
{
    /// <summary>
    /// (gas production - fuel flaring and losses) * gas price
    /// </summary>
    public static void RunCalculation(Case caseItem)
    {
        var netGasSales = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.NetSalesGas));
        var gasPriceNok = caseItem.Project.GasPriceNok;
        var usdToNok = caseItem.Project.ExchangeRateUsdToNok;
        var currency = caseItem.Project.Currency;

        var totalGasIncome = EconomicsHelper.CalculateTotalGasIncome(netGasSales, gasPriceNok, usdToNok, currency);
        var gasIncomeProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalGasIncomeCostProfile);

        gasIncomeProfile.StartYear = totalGasIncome.StartYear;
        gasIncomeProfile.Values = totalGasIncome.Values;
    }
}
