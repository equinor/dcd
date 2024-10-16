using api.Dtos;
using api.Models;

namespace api.Services;

public interface IRevisionService
{
    Task<ProjectWithAssetsDto> GetRevision(Guid projectId);
    Task<ProjectWithAssetsDto> CreateRevision(Guid projectId, ProjectDto projectDto);
}
