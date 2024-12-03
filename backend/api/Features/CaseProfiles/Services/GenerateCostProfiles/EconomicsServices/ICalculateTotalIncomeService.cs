namespace api.Features.CaseProfiles.Services.GenerateCostProfiles.EconomicsServices;

public interface ICalculateTotalIncomeService
{
    Task CalculateTotalIncome(Guid caseId);
}
