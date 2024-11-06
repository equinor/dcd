using api.Dtos;

namespace api.Services;

public interface IRevisionService
{
    Task<ProjectWithAssetsDto> GetRevision(Guid projectId);
    Task<ProjectWithAssetsDto> CreateRevision(Guid projectId, CreateRevisionDto createRevisionDto);
}
