using api.Dtos;

namespace api.Services
{
    public interface ICalculateBreakEvenOilPriceService
    {
        Task CalculateBreakEvenOilPrice(Guid caseId);
    }
}
