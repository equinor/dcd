using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface ICaseService
    {
        Task<ProjectDto> CreateCase(CaseDto caseDto);
        Task<ProjectDto> NewCreateCase(CaseDto caseDto);
        Task<ProjectDto> UpdateCase(CaseDto updatedCaseDto);
        Task<ProjectDto> DeleteCase(Guid caseId);
        Task<Case> GetCase(Guid caseId);
        Task<IEnumerable<Case>> GetAll();
    }
}
