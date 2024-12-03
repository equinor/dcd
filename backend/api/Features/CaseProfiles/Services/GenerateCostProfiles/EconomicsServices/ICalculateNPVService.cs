
namespace api.Features.CaseProfiles.Services.GenerateCostProfiles.EconomicsServices;

public interface ICalculateNPVService
{
    Task CalculateNPV(Guid caseId);
}

