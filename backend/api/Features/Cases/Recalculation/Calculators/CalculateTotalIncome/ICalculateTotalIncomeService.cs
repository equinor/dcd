namespace api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;

public interface ICalculateTotalIncomeService
{
    Task CalculateTotalIncome(Guid caseId);
}
