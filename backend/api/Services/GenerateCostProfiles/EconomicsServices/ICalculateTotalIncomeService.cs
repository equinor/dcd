using api.Dtos;

namespace api.Services
{
    public interface ICalculateTotalIncomeService
    {
        Task CalculateTotalIncome(Guid caseId);
    }
}
