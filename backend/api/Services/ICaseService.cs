using api.Dtos;
using api.Models;

namespace api.Services;

public interface ICaseService
{
    Task<ProjectDto> CreateCase(Guid projectId, CaseDto caseDto);
    Task<ProjectDto> UpdateCase(Guid caseId, CaseDto updatedCaseDto);
    Task<ProjectDto> DeleteCase(Guid caseId);
    Task<Case> GetCase(Guid caseId);
    Task<IEnumerable<Case>> GetAll();
}