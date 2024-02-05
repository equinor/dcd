using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IGenerateStudyCostProfile
    {
        TotalFeasibilityAndConceptStudies CalculateTotalFeasibilityAndConceptStudies(Case caseItem, double sumFacilityCost, double sumWellCost);
        TotalFEEDStudies CalculateTotalFEEDStudies(Case caseItem, double sumFacilityCost, double sumWellCost);
        Task<StudyCostProfileWrapperDto> GenerateAsync(Guid caseId);
        double SumAllCostFacility(Case caseItem);
        double SumWellCost(Case caseItem);
    }
}
