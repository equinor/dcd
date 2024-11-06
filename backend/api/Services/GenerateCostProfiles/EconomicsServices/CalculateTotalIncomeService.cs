using api.Models;

namespace api.Services.EconomicsServices;

public class CalculateTotalIncomeService(ICaseService caseService, IDrainageStrategyService drainageStrategyService) : ICalculateTotalIncomeService
{
    public async Task CalculateTotalIncome(Guid caseId)
    {
        var caseItem = await caseService.GetCaseWithIncludes(
            caseId,
            c => c.Project
        );

        var drainageStrategy = await drainageStrategyService.GetDrainageStrategyWithIncludes(
            caseItem.DrainageStrategyLink,
            d => d.ProductionProfileGas!,
            d => d.AdditionalProductionProfileGas!,
            d => d.ProductionProfileOil!,
            d => d.AdditionalProductionProfileOil!
        );

        var gasPriceNok = caseItem.Project.GasPriceNOK;
        var oilPrice = caseItem.Project.OilPriceUSD;
        var exchangeRateUSDToNOK = caseItem.Project.ExchangeRateUSDToNOK;
        var cubicMetersToBarrelsFactor = 6.29;

        var totalOilProductionInMegaCubics = EconomicsHelper.MergeProductionAndAdditionalProduction(
            drainageStrategy.ProductionProfileOil,
            drainageStrategy.AdditionalProductionProfileOil
        );

        // Convert oil production from million sm³ to barrels in millions
        var oilProductionInMillionsOfBarrels = totalOilProductionInMegaCubics.Values.Select(v => v * cubicMetersToBarrelsFactor).ToArray();

        var oilIncome = new TimeSeries<double>
        {
            StartYear = totalOilProductionInMegaCubics.StartYear,
            Values = oilProductionInMillionsOfBarrels.Select(v => v * oilPrice * exchangeRateUSDToNOK).ToArray(),
        };

        var totalGasProductionInGigaCubics = EconomicsHelper.MergeProductionAndAdditionalProduction(
            drainageStrategy.ProductionProfileGas,
            drainageStrategy.AdditionalProductionProfileGas
        );

        var gasIncome = new TimeSeries<double>
        {
            StartYear = totalGasProductionInGigaCubics.StartYear,
            Values = totalGasProductionInGigaCubics.Values.Select(v => v * gasPriceNok).ToArray()
        };

        var totalIncome = TimeSeriesCost.MergeCostProfiles(oilIncome, gasIncome);

        // Divide the totalIncome by 1 million before assigning it to CalculatedTotalIncomeCostProfile to get correct unit
        var scaledTotalIncomeValues = totalIncome.Values.Select(v => v / 1_000_000).ToArray();

        if (caseItem.CalculatedTotalIncomeCostProfile != null)
        {
            caseItem.CalculatedTotalIncomeCostProfile.Values = scaledTotalIncomeValues;
            caseItem.CalculatedTotalIncomeCostProfile.StartYear = totalIncome.StartYear;
        }
        else
        {
            caseItem.CalculatedTotalIncomeCostProfile = new CalculatedTotalIncomeCostProfile
            {
                Values = scaledTotalIncomeValues,
                StartYear = totalIncome.StartYear
            };
        }
    }
}

