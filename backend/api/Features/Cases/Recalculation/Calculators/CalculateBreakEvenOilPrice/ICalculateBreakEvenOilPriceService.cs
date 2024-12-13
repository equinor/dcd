
namespace api.Features.Cases.Recalculation.Calculators.CalculateBreakEvenOilPrice;

public interface ICalculateBreakEvenOilPriceService
{
    Task CalculateBreakEvenOilPrice(Guid caseId);
}

