
namespace api.Features.CaseProfiles.Services.GenerateCostProfiles.EconomicsServices;

public interface ICalculateBreakEvenOilPriceService
{
    Task CalculateBreakEvenOilPrice(Guid caseId);
}

