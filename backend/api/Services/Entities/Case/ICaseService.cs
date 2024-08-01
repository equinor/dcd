using api.Dtos;
using api.Models;

namespace api.Services;

public interface ICaseService
{
    Task<ProjectWithAssetsDto> CreateCase(Guid projectId, CreateCaseDto createCaseDto);
    Task<ProjectWithAssetsDto> UpdateCaseAndProfiles<TDto>(Guid caseId, TDto updatedCaseDto) where TDto : BaseUpdateCaseDto;
    Task<ProjectWithAssetsDto> DeleteCase(Guid caseId);
    Task<Case> GetCase(Guid caseId);
    Task<IEnumerable<Case>> GetAll();
    Task<CaseDto> UpdateCase<TDto>(Guid caseId, TDto updatedCaseDto) where TDto : BaseUpdateCaseDto;
}
