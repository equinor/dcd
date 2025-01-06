using api.Features.Assets.CaseAssets.DrainageStrategies.Services;
using api.Features.CaseProfiles.Services;
using api.Features.Cases.Recalculation.Calculators.Helpers;
using api.Models;

namespace api.Features.Cases.Recalculation.Calculators.CalculateBreakEvenOilPrice;

public class CalculateBreakEvenOilPriceService(
    ICaseService caseService,
    IDrainageStrategyService drainageStrategyService) : ICalculateBreakEvenOilPriceService
{
    public async Task CalculateBreakEvenOilPrice(Guid caseId)
    {
        var caseItem = await caseService.GetCaseWithIncludes(
            caseId,
            c => c.Project
        );

        var drainageStrategy = await drainageStrategyService.GetDrainageStrategyWithIncludes(
            caseItem.DrainageStrategyLink,
            d => d.ProductionProfileOil!,
            d => d.AdditionalProductionProfileOil!,
            d => d.ProductionProfileGas!,
            d => d.AdditionalProductionProfileGas!
        );

        CalculateBreakEvenOilPrice(caseItem, drainageStrategy);
    }

    public static void CalculateBreakEvenOilPrice(Case caseItem, DrainageStrategy drainageStrategy)
    {
        var discountRate = caseItem.Project.DiscountRate;
        var defaultOilPrice = caseItem.Project.OilPriceUSD;
        var gasPriceNok = caseItem.Project.GasPriceNOK;
        var exchangeRateUsdToNok = caseItem.Project.ExchangeRateUSDToNOK;

        var oilVolume = EconomicsHelper.MergeProductionAndAdditionalProduction(
            drainageStrategy.ProductionProfileOil,
            drainageStrategy.AdditionalProductionProfileOil
        );

        if (oilVolume.Values.Length == 0) { return; }

        oilVolume.Values = oilVolume.Values.Select(v => v / 1_000_000).ToArray();

        var gasVolume = EconomicsHelper.MergeProductionAndAdditionalProduction(
            drainageStrategy.ProductionProfileGas,
            drainageStrategy.AdditionalProductionProfileGas
        );

        gasVolume.Values = gasVolume.Values.Length != 0 ? gasVolume.Values.Select(v => v / 1_000_000_000).ToArray() : [];

        var currentYear = DateTime.Now.Year;
        var nextYear = currentYear + 1;
        var dg4Year = caseItem.DG4Date.Year;
        var nextYearInRelationToDg4Year = nextYear - dg4Year;

        var discountedGasVolume = EconomicsHelper.CalculateDiscountedVolume(
            gasVolume.Values,
            discountRate,
            gasVolume.StartYear + Math.Abs(nextYearInRelationToDg4Year)
        );

        var discountedOilVolume = EconomicsHelper.CalculateDiscountedVolume(
            oilVolume.Values,
            discountRate,
            oilVolume.StartYear + Math.Abs(nextYearInRelationToDg4Year)
        );

        if (discountedOilVolume == 0 || discountedGasVolume == 0) { return; }

        var discountedTotalCost = EconomicsHelper.CalculateDiscountedVolume(
            caseItem.CalculatedTotalCostCostProfile?.Values ?? [],
            discountRate,
            (caseItem.CalculatedTotalCostCostProfile?.StartYear ?? 0) + Math.Abs(nextYearInRelationToDg4Year)
        );

        var gor = discountedGasVolume / discountedOilVolume;

        var pa = gasPriceNok > 0 ? gasPriceNok * 1000 / (exchangeRateUsdToNok * 6.29 * defaultOilPrice) : 0;

        var breakEvenPrice = discountedTotalCost / ((gor * pa) + 1) / discountedOilVolume / 6.29;

        if (caseItem != null)
        {
            caseItem.BreakEven = breakEvenPrice / caseItem.Project.ExchangeRateUSDToNOK;
        }
    }
}

