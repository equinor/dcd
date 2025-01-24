using api.Context;
using api.Features.Cases.Recalculation.Calculators.Helpers;
using api.Features.TimeSeriesCalculators;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;

public class CalculateTotalIncomeService(DcdDbContext context)
{
    public async Task CalculateTotalIncome(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(x => x.Project)
            .Include(x => x.CalculatedTotalIncomeCostProfile)
            .SingleAsync(x => x.Id == caseId);

        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileGas)
            .Include(d => d.ProductionProfileOil)
            .Include(d => d.AdditionalProductionProfileOil)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        CalculateTotalIncome(caseItem, drainageStrategy);
    }

    public static void CalculateTotalIncome(Case caseItem, DrainageStrategy drainageStrategy)
    {
        var gasPriceNok = caseItem.Project.GasPriceNOK;
        var oilPrice = caseItem.Project.OilPriceUSD;
        var exchangeRateUsdToNok = caseItem.Project.ExchangeRateUSDToNOK;
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
            Values = oilProductionInMillionsOfBarrels.Select(v => v * oilPrice * exchangeRateUsdToNok).ToArray(),
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

        var totalIncome = CostProfileMerger.MergeCostProfiles(oilIncome, gasIncome);

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

