using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface ITotalIncomeProfileService
    {
        Task CalculateTotalIncome(Guid caseId);
    }
}
