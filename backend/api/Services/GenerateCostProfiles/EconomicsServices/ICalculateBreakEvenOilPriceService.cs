
namespace api.Services.EconomicsServices;

public interface ICalculateBreakEvenOilPriceService
{
    Task CalculateBreakEvenOilPrice(Guid caseId);
}

