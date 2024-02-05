using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IGenerateOpexCostProfile
    {
        OffshoreFacilitiesOperationsCostProfile CalculateOffshoreFacilitiesOperationsCostProfile(Case caseItem, DrainageStrategy drainageStrategy);
        WellInterventionCostProfile CalculateWellInterventionCostProfile(Case caseItem, Project project, DrainageStrategy drainageStrategy);
        Task<OpexCostProfileWrapperDto> GenerateAsync(Guid caseId);
    }
}
