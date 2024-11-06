using api.Dtos;

namespace api.Services;

public interface ICreateCaseService
{
    Task<ProjectWithAssetsDto> CreateCase(Guid projectId, CreateCaseDto createCaseDto);
}
