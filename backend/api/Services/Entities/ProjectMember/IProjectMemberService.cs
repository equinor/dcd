using api.Dtos;

namespace api.Services;

public interface IProjectMemberService
{
    Task<ProjectMemberDto> CreateProjectMember(Guid projectId, CreateProjectMemberDto dto);
    Task DeleteProjectMember(Guid projectId, Guid userId);
}
