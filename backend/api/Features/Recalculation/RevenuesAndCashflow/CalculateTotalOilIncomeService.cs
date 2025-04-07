using api.Features.Profiles;
using api.Features.Recalculation.Helpers;
using api.Models;

namespace api.Features.Recalculation.RevenuesAndCashflow;

public static class CalculateTotalOilIncomeService
{
    public static void RunCalculation(Case caseItem)
    {
        var oilProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileOil);
        var oilPriceUsd = caseItem.Project.OilPriceUsd;
        var usdToNok = caseItem.Project.ExchangeRateUsdToNok;
        var currency = caseItem.Project.Currency;

        var totalOilIncome = EconomicsHelper.CalculateTotalOilIncome(oilProduction, oilPriceUsd, usdToNok, currency);
        var oilIncomeProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalOilIncomeCostProfile);

        oilIncomeProfile.StartYear = totalOilIncome.StartYear;
        oilIncomeProfile.Values = totalOilIncome.Values;
    }
}
