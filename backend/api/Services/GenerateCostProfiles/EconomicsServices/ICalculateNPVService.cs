
namespace api.Services.EconomicsServices;

public interface ICalculateNPVService
{
    Task CalculateNPV(Guid caseId);
}

