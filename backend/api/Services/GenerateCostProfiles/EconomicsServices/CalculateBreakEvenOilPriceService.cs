using api.Models;
using api.Services;

using EconomicsServices;

namespace EconomicsServices
{
    public class CalculateBreakEvenOilPriceService : ICalculateBreakEvenOilPriceService
    {
        private readonly ICaseService _caseService;
        private readonly IDrainageStrategyService _drainageStrategyService;

        public CalculateBreakEvenOilPriceService(ICaseService caseService, IDrainageStrategyService drainageStrategyService)
        {
            _caseService = caseService;
            _drainageStrategyService = drainageStrategyService;
        }

        public async Task CalculateBreakEvenOilPrice(Guid caseId)
        {
            var caseItem = await _caseService.GetCaseWithIncludes(
                            caseId,
                            c => c.Project
                        );

            var drainageStrategy = await _drainageStrategyService.GetDrainageStrategyWithIncludes(
                caseItem.DrainageStrategyLink,
                d => d.ProductionProfileOil!,
                d => d.AdditionalProductionProfileOil!,
                d => d.ProductionProfileGas!,
                d => d.AdditionalProductionProfileGas!
            );

            var discountRate = caseItem.Project?.DiscountRate ?? 8;
            var defaultOilPrice = caseItem.Project?.OilPriceUSD ?? 75;
            var gasPriceNOK = caseItem.Project?.GasPriceNOK ?? 3;
            var exchangeRateUSDToNOK = caseItem.Project?.ExchangeRateUSDToNOK ?? 10;

            var oilVolume = TimeSeriesCost.MergeCostProfiles(
                new TimeSeries<double> { StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0, Values = drainageStrategy.ProductionProfileOil?.Values ?? Array.Empty<double>() },
                new TimeSeries<double> { StartYear = drainageStrategy.AdditionalProductionProfileOil?.StartYear ?? 0, Values = drainageStrategy.AdditionalProductionProfileOil?.Values ?? Array.Empty<double>() }
            );
            if (!oilVolume.Values.Any()) { return; }
            oilVolume.Values = oilVolume.Values.Select(v => v / 1_000_000).ToArray();

            var gasVolume = TimeSeriesCost.MergeCostProfiles(
                new TimeSeries<double> { StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0, Values = drainageStrategy.ProductionProfileGas?.Values ?? Array.Empty<double>() },
                new TimeSeries<double> { StartYear = drainageStrategy.AdditionalProductionProfileGas?.StartYear ?? 0, Values = drainageStrategy.AdditionalProductionProfileGas?.Values ?? Array.Empty<double>() }
            );
            gasVolume.Values = gasVolume.Values.Any() ? gasVolume.Values.Select(v => v / 1_000_000_000).ToArray() : Array.Empty<double>();

            var currentYear = DateTime.Now.Year;
            var nextYear = currentYear + 1;
            var dg4Year = caseItem.DG4Date.Year;
            var nextYearInRelationToDg4Year = nextYear - dg4Year;

            var discountedGasVolume = EconomicsHelper.CalculateDiscountedVolume(gasVolume.Values, discountRate, gasVolume.StartYear + Math.Abs(nextYearInRelationToDg4Year));
            var discountedOilVolume = EconomicsHelper.CalculateDiscountedVolume(oilVolume.Values, discountRate, oilVolume.StartYear + Math.Abs(nextYearInRelationToDg4Year));

            if (discountedOilVolume == 0 || discountedGasVolume == 0) { return; }

            var discountedTotalCost = EconomicsHelper.CalculateDiscountedVolume(caseItem?.CalculatedTotalCostCostProfile?.Values ?? Array.Empty<double>(), discountRate, (caseItem?.CalculatedTotalCostCostProfile?.StartYear ?? 0) + Math.Abs(nextYearInRelationToDg4Year));

            var GOR = discountedGasVolume / discountedOilVolume;

            var PA = gasPriceNOK > 0 ? gasPriceNOK * 1000 / (exchangeRateUSDToNOK * 6.29 * defaultOilPrice) : 0;

            var breakEvenPrice = discountedTotalCost / ((GOR * PA) + 1) / discountedOilVolume / 6.29;

            if (caseItem != null)
            {
                caseItem.BreakEven = breakEvenPrice;
            }
        }
    }
}