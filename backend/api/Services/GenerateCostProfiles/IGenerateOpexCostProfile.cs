using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IGenerateOpexCostProfile
    {
        Task<OpexCostProfileWrapperDto> GenerateAsync(Guid caseId);
        Task<WellInterventionCostProfile> CalculateWellInterventionCostProfile(Case caseItem, Project project, DrainageStrategy drainageStrategy);
        Task<OffshoreFacilitiesOperationsCostProfile> CalculateOffshoreFacilitiesOperationsCostProfile(Case caseItem, DrainageStrategy drainageStrategy);
    }
}
