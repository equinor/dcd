using api.Models;
using api.Services;

namespace EconomicsServices
{
    public class CalculateTotalIncomeService : ICalculateTotalIncomeService
    {
        private readonly ICaseService _caseService;
        private readonly IDrainageStrategyService _drainageStrategyService;

        public CalculateTotalIncomeService(ICaseService caseService, IDrainageStrategyService drainageStrategyService)
        {
            _caseService = caseService;
            _drainageStrategyService = drainageStrategyService;
        }

        public async Task CalculateTotalIncome(Guid caseId)
        {
            var caseItem = await _caseService.GetCaseWithIncludes(
                            caseId,
                            c => c.Project
                        );

            var drainageStrategy = await _drainageStrategyService.GetDrainageStrategyWithIncludes(
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
            var exchangeRateNOKToUSD = 1 / exchangeRateUSDToNOK;

            var oilProfile = drainageStrategy.ProductionProfileOil?.Values ?? Array.Empty<double>();
            var additionalOilProfile = drainageStrategy.AdditionalProductionProfileOil?.Values ?? Array.Empty<double>();

            var totalOilProductionInMegaCubics = TimeSeriesCost.MergeCostProfiles(
                new TimeSeries<double>
                {
                    StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0,
                    Values = oilProfile
                },
                new TimeSeries<double>
                {
                    StartYear = drainageStrategy.AdditionalProductionProfileOil?.StartYear ?? 0,
                    Values = additionalOilProfile
                }
            );

            // Convert oil production from million sm³ to barrels in millions
            var oilProductionInMillionsOfBarrels = totalOilProductionInMegaCubics.Values.Select(v => v * cubicMetersToBarrelsFactor).ToArray();

            var oilIncome = new TimeSeries<double>
            {
                StartYear = totalOilProductionInMegaCubics.StartYear,
                Values = oilProductionInMillionsOfBarrels.Select(v => v * oilPrice * exchangeRateUSDToNOK).ToArray(),
            };

            var gasProfile = drainageStrategy.ProductionProfileGas?.Values ?? Array.Empty<double>();
            var additionalGasProfile = drainageStrategy.AdditionalProductionProfileGas?.Values ?? Array.Empty<double>();

            var totalGasProductionInGigaCubics = TimeSeriesCost.MergeCostProfiles(
                new TimeSeries<double>
                {
                    StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
                    Values = gasProfile
                },
                new TimeSeries<double>
                {
                    StartYear = drainageStrategy.AdditionalProductionProfileGas?.StartYear ?? 0,
                    Values = additionalGasProfile
                }
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


            return;
        }
    }
}