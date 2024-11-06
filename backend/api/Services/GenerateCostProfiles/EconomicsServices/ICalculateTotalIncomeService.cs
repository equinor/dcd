namespace api.Services.EconomicsServices;

public interface ICalculateTotalIncomeService
{
    Task CalculateTotalIncome(Guid caseId);
}
