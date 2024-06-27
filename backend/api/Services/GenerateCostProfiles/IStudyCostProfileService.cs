using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IStudyCostProfileService
    {
        Task<StudyCostProfileWrapperDto> Generate(Guid caseId);
        Task<StudyCostProfileWrapperDto> Generate(Case caseItem);
        Task<double> SumAllCostFacility(Case caseItem);
        Task<double> SumWellCost(Case caseItem);
    }
}
