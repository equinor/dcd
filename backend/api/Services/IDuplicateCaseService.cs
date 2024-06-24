using api.Dtos;
using api.Models;

namespace api.Services;

public interface IDuplicateCaseService
{
    Task<ProjectDto> DuplicateCase(Guid caseId);
}
