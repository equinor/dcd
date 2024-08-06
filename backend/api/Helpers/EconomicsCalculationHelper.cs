using api.Dtos;
using api.Models;
using api.Repositories;
using api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class EconomicsCalculationHelper
    {
        private const int cd = 365;
        private readonly IStudyCostProfileService _generateStudyCostProfile;
        private readonly IOpexCostProfileService _generateOpexCostProfile;
        private readonly ICessationCostProfileService _generateCessationCostProfile;
        private readonly IWellCostProfileService _generateWellCostProfile;
        private readonly IExplorationService _generateExplorationCostProfile;

        // Constructor with dependency injection
        public EconomicsCalculationHelper(
            IStudyCostProfileService generateStudyCostProfile,
            IOpexCostProfileService generateOpexCostProfile,
            ICessationCostProfileService generateCessationCostProfile,
            IWellCostProfileService generateWellCostProfile,
            IExplorationService generateExplorationCostProfile)
        {
            _generateStudyCostProfile = generateStudyCostProfile ?? throw new ArgumentNullException(nameof(generateStudyCostProfile));
            _generateOpexCostProfile = generateOpexCostProfile ?? throw new ArgumentNullException(nameof(generateOpexCostProfile));
            _generateCessationCostProfile = generateCessationCostProfile ?? throw new ArgumentNullException(nameof(generateCessationCostProfile));
            _generateWellCostProfile = generateWellCostProfile ?? throw new ArgumentNullException(nameof(generateWellCostProfile));
            _generateExplorationCostProfile = generateExplorationCostProfile ?? throw new ArgumentNullException(nameof(generateExplorationCostProfile));
        }

        private static TimeSeries<double> CalculateIncome(DrainageStrategyWithProfilesDto drainageStrategy, double oilPrice, double gasPrice, double exchangeRate)
        {
            // Calculate income from oil
            var oilProfile = drainageStrategy.ProductionProfileOil?.Values ?? Array.Empty<double>();
            var additionalOilProfile = drainageStrategy.AdditionalProductionProfileOil?.Values ?? Array.Empty<double>();

            var productionProfileOil = new TimeSeries<double>
            {
                StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0,
                Values = oilProfile
            };

            var additionalProductionProfileOil = new TimeSeries<double>
            {
                StartYear = drainageStrategy.AdditionalProductionProfileOil?.StartYear ?? 0,
                Values = additionalOilProfile
            };

            var totalOilProduction = TimeSeriesCost.MergeCostProfiles(productionProfileOil, additionalProductionProfileOil);
            var oilIncome = new TimeSeries<double>
            {
                StartYear = totalOilProduction.StartYear,
                Values = totalOilProduction.Values.Select(v => v * oilPrice).ToArray()
            };

            // Calculate income from gas
            var gasProfile = drainageStrategy.NetSalesGas?.Values ?? Array.Empty<double>();
            var additionalGasProfile = drainageStrategy.NetSalesGasOverride?.Values ?? Array.Empty<double>();

            var productionProfileGas = new TimeSeries<double>
            {
                StartYear = drainageStrategy.NetSalesGas?.StartYear ?? 0,
                Values = gasProfile
            };

            var additionalProductionProfileGas = new TimeSeries<double>
            {
                StartYear = drainageStrategy.NetSalesGasOverride?.StartYear ?? 0,
                Values = additionalGasProfile
            };

            var totalGasProduction = TimeSeriesCost.MergeCostProfiles(productionProfileGas, additionalProductionProfileGas);
            var gasIncome = new TimeSeries<double>
            {
                StartYear = totalGasProduction.StartYear,
                Values = totalGasProduction.Values.Select(v => v * gasPrice * exchangeRate).ToArray()
            };

            // Merge the income profiles
            var totalIncome = TimeSeriesCost.MergeCostProfiles(oilIncome, gasIncome);

            return totalIncome;
        }

        public async Task<TimeSeries<double>> CalculateTotalCostAsync(Case caseItem)
        {
            // Calculate total study cost
            var totalStudyCost = await _generateStudyCostProfile.Generate(caseItem.Id);
            var studiesProfile = new TimeSeries<double>
            {
                StartYear = totalStudyCost.StudyCostProfileDto?.StartYear ?? 0,
                Values = totalStudyCost.StudyCostProfileDto?.Values ?? Array.Empty<double>()
            };

            // Calculate total Opex cost
            var totalOpexCost = await _generateOpexCostProfile.Generate(caseItem.Id);
            var opexProfile = new TimeSeries<double>
            {
                StartYear = totalOpexCost.OpexCostProfileDto?.StartYear ?? 0,
                Values = totalOpexCost.OpexCostProfileDto?.Values ?? Array.Empty<double>()
            };

            // Calculate total Cessation cost
            var totalCessationCost = await _generateCessationCostProfile.Generate(caseItem.Id);
            var cessationProfile = new TimeSeries<double>
            {
                StartYear = totalCessationCost.CessationCostDto.StartYear,
                Values = totalCessationCost.CessationCostDto?.Values ?? Array.Empty<double>()
            };

            // Calculate total Offshore facility cost
            var totalOffshoreFacilityCost = await _generateStudyCostProfile.SumAllCostFacility(caseItem);
            var offshoreFacilityProfile = new TimeSeries<double>
            {
                StartYear = totalOffshoreFacilityCost
                Values = totalOffshoreFacilityCost.Values ?? Array.Empty<double>()
            };

            // Calculate total Development cost
            var totalDevelopmentCost = await _generateWellCostProfile.Generate(caseItem.Id);
            var developmentProfile = new TimeSeries<double>
            {
                StartYear = totalDevelopmentCost.StartYear,
                Values = totalDevelopmentCost.Values ?? Array.Empty<double>()
            };

            // Calculate total Exploration cost
            var explorationCost = await _generateExplorationCostProfile.GetExploration(caseItem.Id);
            var explorationProfile = new TimeSeries<double>
            {
                StartYear = explorationCost.ExplorationWellCostProfile?.StartYear ?? 0,
                Values = explorationCost.ExplorationWellCostProfile?.Values ?? Array.Empty<double>()
            };

            var totalCost = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>>
            {
                studiesProfile,
                opexProfile,
                cessationProfile,
                offshoreFacilityProfile,
                developmentProfile,
                explorationProfile
            });

            return totalCost;
        }

        private static TimeSeries<double> CalculateCashFlow(TimeSeries<double> income, TimeSeries<double> totalCost)
        {
            var cashFlowValues = income.Values.Zip(totalCost.Values, (i, c) => i - c).ToArray();

            return new TimeSeries<double>
            {
                StartYear = income.StartYear,
                Values = cashFlowValues,
            };
        }

        public async Task<TimeSeries<double>> CalculateProjectCashFlowAsync(Guid caseId, DrainageStrategyWithProfilesDto drainageStrategy, double oilPrice, double gasPrice, double exchangeRate)
        {
            var income = CalculateIncome(drainageStrategy, oilPrice, gasPrice, exchangeRate);
            var totalCost = await CalculateTotalCostAsync(new Case { Id = caseId });
            var cashFlow = CalculateCashFlow(income, totalCost);
            return cashFlow;
        }
    }
}
