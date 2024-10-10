using api.Dtos;

namespace api.Services
{
    public interface ICalculateTotalCostService
    {
        Task CalculateTotalCost(Guid caseId);
    }
}
