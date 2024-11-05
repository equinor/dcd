
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;


namespace api.Services;

public class ProjectMemberService : IProjectMemberService
{

    private readonly IProjectMemberRepository _projectMemberRepository;

    public ProjectMemberService(
        IProjectMemberRepository projectMemberRepository
    )
    {
        _projectMemberRepository = projectMemberRepository;
    }

    public async Task<ProjectMemberDto> CreateProjectMember(Guid projectId, CreateProjectMemberDto dto)
    {
        var projectMember = new ProjectMember
        {
            ProjectId = projectId,
            UserId = dto.UserId,
            Role = dto.Role
        };

        var existingProjectMember = await _projectMemberRepository.GetProjectMember(projectId, dto.UserId);

        if (existingProjectMember != null)
        {
            throw new ResourceAlreadyExistsException("Project member already exists");
        }

        await _projectMemberRepository.CreateProjectMember(projectMember);
        await _projectMemberRepository.SaveChangesAsync();

        return new ProjectMemberDto
        {
            ProjectId = projectMember.ProjectId,
            UserId = projectMember.UserId,
            Role = projectMember.Role
        };
    }

    public async Task DeleteProjectMember(Guid projectId, Guid userId)
    {
        var projectMember = await _projectMemberRepository.GetProjectMember(projectId, userId);

        if (projectMember == null)
        {
            throw new NotFoundInDBException("Project member not found");
        }

        _projectMemberRepository.DeleteProjectMember(projectMember);
        await _projectMemberRepository.SaveChangesAsync();
    }
}
