using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IStudyCostProfileService
    {
        Task<StudyCostProfileWrapperDto> Generate(Guid caseId);
        Task<double> SumAllCostFacility(Case caseItem);
        Task<double> SumWellCost(Case caseItem);
    }
}
