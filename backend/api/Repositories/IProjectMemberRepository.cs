using api.Models;

namespace api.Repositories;

public interface IProjectMemberRepository
{
    Task<ProjectMember> CreateProjectMember(ProjectMember projectMember);
    void DeleteProjectMember(ProjectMember projectMember);
    Task<ProjectMember?> GetProjectMember(Guid projectId, Guid userId);
    Task SaveChangesAsync();
}
