using api.Dtos;

namespace api.Services;

public interface IProjectMemberService
{
    Task<ProjectMemberDto> CreateProjectMember(Guid projectId, CreateProjectMemberDto dto);
    Task<ProjectMemberDto> UpdateProjectMember(Guid projectId, UpdateProjectMemberDto dto);
    Task DeleteProjectMember(Guid projectId, Guid userId);
}
