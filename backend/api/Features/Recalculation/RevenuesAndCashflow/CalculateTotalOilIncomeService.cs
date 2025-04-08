using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Recalculation.Helpers;
using api.Models;

namespace api.Features.Recalculation.RevenuesAndCashflow;

/// <summary>
/// (oil production + condensate Production) * gas price
/// </summary>
public static class CalculateTotalOilIncomeService
{
    public static void RunCalculation(Case caseItem)
    {
        var oilProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileOil);
        var totalCondensateProduction = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.CondensateProduction));
        var oilPriceUsd = caseItem.Project.OilPriceUsd;
        var usdToNok = caseItem.Project.ExchangeRateUsdToNok;
        var currency = caseItem.Project.Currency;

        var totalOilIncome = EconomicsHelper.CalculateTotalOilIncome(oilProduction, totalCondensateProduction, oilPriceUsd, usdToNok, currency);
        var oilIncomeProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalOilIncomeCostProfile);

        oilIncomeProfile.StartYear = totalOilIncome.StartYear;
        oilIncomeProfile.Values = totalOilIncome.Values;
    }
}
