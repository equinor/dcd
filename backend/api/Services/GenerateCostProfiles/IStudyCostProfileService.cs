using api.Models;

namespace api.Services;

public interface IStudyCostProfileService
{
    Task Generate(Guid caseId);
    Task<double> SumAllCostFacility(Case caseItem);
    Task<double> SumWellCost(Case caseItem);
}
