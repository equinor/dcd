using api.Dtos;

namespace api.Services;

public interface IDuplicateCaseService
{
    Task<ProjectWithAssetsDto> DuplicateCase(Guid caseId);
}
