using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Recalculation.Helpers;
using api.Models;

namespace api.Features.Recalculation.RevenuesAndCashflow;

public class CalculateTotalGasIncomeService
{
    public static void RunCalculation(Case caseItem)
    {
        var gasProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileGas);
        var gasPriceNok = caseItem.Project.GasPriceNok;

        var totalGasIncome = EconomicsHelper.CalculateTotalGasIncome(gasProduction, gasPriceNok);
        var gasIncomeProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalGasIncomeCostProfile);

        gasIncomeProfile.StartYear = totalGasIncome.StartYear;
        gasIncomeProfile.Values = totalGasIncome.Values;
    }
}
