
namespace api.Features.Cases.Recalculation.Calculators.CalculateNpv;

public interface ICalculateNPVService
{
    Task CalculateNPV(Guid caseId);
}

