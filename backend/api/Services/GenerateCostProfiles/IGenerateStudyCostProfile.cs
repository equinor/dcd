using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IGenerateStudyCostProfile
    {
        Task<StudyCostProfileWrapperDto> GenerateAsync(Guid caseId);
        TotalFeasibilityAndConceptStudies CalculateTotalFeasibilityAndConceptStudies(Case caseItem, double sumFacilityCost, double sumWellCost);
        TotalFEEDStudies CalculateTotalFEEDStudies(Case caseItem, double sumFacilityCost, double sumWellCost);
        Task<double> SumAllCostFacility(Case caseItem);
        Task<double> SumWellCost(Case caseItem);
    }
}
