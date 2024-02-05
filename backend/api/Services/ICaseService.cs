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
        ProjectDto NewCreateCase(CaseDto caseDto);
        CaseDto NewUpdateCase(CaseDto updatedCaseDto);
        ProjectDto UpdateCase(CaseDto updatedCaseDto);
    }
}
