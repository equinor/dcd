using api.Dtos;
using api.Models;
using System.Threading.Tasks;

namespace api.Helpers
{
    public interface IEconomicsCalculationHelper
    {
        // TimeSeries<double> CalculateIncome(DrainageStrategy drainageStrategy, Project project, Case caseItem);
        
        Task<TimeSeries<double>> CalculateTotalCostAsync(Case caseItem);

        Task<TimeSeries<double>> CalculateTotalOffshoreFacilityCostAsync(Case caseItem);

        Task<TimeSeries<double>> CalculateTotalDevelopmentCostAsync(Case caseItem);

        Task<TimeSeries<double>> CalculateTotalExplorationCostAsync(Case caseItem);

        TimeSeries<double> CalculateCashFlow(TimeSeries<double> income, TimeSeries<double> totalCost);

        Task<TimeSeries<double>> CalculateProjectCashFlowAsync(Case caseItem, DrainageStrategy drainageStrategy);
    }
}
