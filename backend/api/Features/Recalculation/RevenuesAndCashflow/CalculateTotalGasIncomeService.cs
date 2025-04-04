using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Recalculation.Helpers;
using api.Models;

namespace api.Features.Recalculation.RevenuesAndCashflow;

public static class CalculateTotalGasIncomeService
{
    public static void RunCalculation(Case caseItem)
    {
        var gasProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileGas);
        var gasPriceNok = caseItem.Project.GasPriceNok;

        var totalGasIncome = EconomicsHelper.CalculateTotalGasIncome(gasProduction, gasPriceNok);
        var gasIncomeProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalGasIncomeCostProfileNok);

        gasIncomeProfile.StartYear = totalGasIncome.StartYear;
        gasIncomeProfile.Values = totalGasIncome.Values;
    }
}
