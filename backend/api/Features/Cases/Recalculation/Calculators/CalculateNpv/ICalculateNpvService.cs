
namespace api.Features.Cases.Recalculation.Calculators.CalculateNpv;

public interface ICalculateNpvService
{
    Task CalculateNpv(Guid caseId);
}

