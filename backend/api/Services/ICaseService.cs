using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface ICaseService
    {
        Task<ProjectDto> CreateCase(CaseDto caseDto);
        ProjectDto DeleteCase(Guid caseId);
        IEnumerable<Case> GetAll();
        Case GetCase(Guid caseId);
        Task<ProjectDto> NewCreateCaseAsync(CaseDto caseDto);
        CaseDto NewUpdateCase(CaseDto updatedCaseDto);
        ProjectDto UpdateCase(CaseDto updatedCaseDto);
    }
}
