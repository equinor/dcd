using System.Threading.Tasks;

using api.Dtos;
using api.Models;

namespace api.Helpers
{
    public interface IEconomicsCalculationHelper
    {
        Task CalculateTotalIncome(Guid caseId);

        Task CalculateTotalCost(Guid caseId);

        Task<TimeSeries<double>> CalculateTotalOffshoreFacilityCostAsync(Case caseItem);

        Task<TimeSeries<double>> CalculateTotalDevelopmentCostAsync(Case caseItem);

        Task<TimeSeries<double>> CalculateTotalExplorationCostAsync(Case caseItem);

        TimeSeries<double> CalculateCashFlow(TimeSeries<double> income, TimeSeries<double> totalCost);
        Task CalculateNPV(Guid caseId);
        Task CalculateBreakEvenOilPrice(Guid caseId);


    }
}
