namespace api.Features.CaseProfiles.Services.GenerateCostProfiles.EconomicsServices;

public interface ICalculateTotalCostService
{
    Task CalculateTotalCost(Guid caseId);
}
