using api.Models;

namespace api.Services
{
    public interface IEconomicsCalculationService
    {
        Task CalculateTotalIncome(Guid caseId);

        Task CalculateTotalCost(Guid caseId);

        static TimeSeries<double> CalculateTotalOffshoreFacilityCost(
            Substructure? substructure,
            Surf? surf,
            Topside? topside,
            Transport? transport)
        {
            throw new NotImplementedException();
        }

        static TimeSeries<double> CalculateTotalDevelopmentCost(WellProject wellProject)
        {
            throw new NotImplementedException();
        }

        static TimeSeries<double> CalculateTotalExplorationCost(Exploration exploration)
        {
            throw new NotImplementedException();
        }

        TimeSeries<double> CalculateCashFlow(TimeSeries<double> income, TimeSeries<double> totalCost);
        Task CalculateNPV(Guid caseId);
        Task CalculateBreakEvenOilPrice(Guid caseId);


    }
}
