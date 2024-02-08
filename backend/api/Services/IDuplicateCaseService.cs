using api.Dtos;
using api.Models;

namespace api.Services;

public interface IDuplicateCaseService
{
    Task<Case> GetCaseNoTracking(Guid caseId);
    Task<ProjectDto> DuplicateCase(Guid caseId);
}
