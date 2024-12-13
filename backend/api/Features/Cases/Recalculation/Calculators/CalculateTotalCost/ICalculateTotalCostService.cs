namespace api.Features.Cases.Recalculation.Calculators.CalculateTotalCost;

public interface ICalculateTotalCostService
{
    Task CalculateTotalCost(Guid caseId);
}
