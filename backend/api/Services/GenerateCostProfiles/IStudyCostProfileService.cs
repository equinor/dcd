using api.Models;

namespace api.Services;

public interface IStudyCostProfileService
{
    Task Generate(Guid caseId);
    double SumAllCostFacilityWithPreloadedData(Case caseItem);
    double SumWellCostWithPreloadedData(Case caseItem);
}
