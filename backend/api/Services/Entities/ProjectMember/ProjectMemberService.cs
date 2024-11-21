
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;


namespace api.Services;

public class ProjectMemberService(IProjectMemberRepository projectMemberRepository) : IProjectMemberService
{
    public async Task<ProjectMemberDto> CreateProjectMember(Guid projectId, CreateProjectMemberDto dto)
    {
        var projectMember = new ProjectMember
        {
            ProjectId = projectId,
            UserId = dto.UserId,
            Role = dto.Role
        };

        var existingProjectMember = await projectMemberRepository.GetProjectMember(projectId, dto.UserId);

        if (existingProjectMember != null)
        {
            throw new ResourceAlreadyExistsException("Project member already exists");
        }

        await projectMemberRepository.CreateProjectMember(projectMember);
        await projectMemberRepository.SaveChangesAsync();

        return new ProjectMemberDto
        {
            ProjectId = projectMember.ProjectId,
            UserId = projectMember.UserId,
            Role = projectMember.Role
        };
    }

    public async Task<ProjectMemberDto> UpdateProjectMember(Guid projectId, UpdateProjectMemberDto dto)
    {
        var existingProjectMember = await projectMemberRepository.GetProjectMember(projectId, dto.UserId);

        if (existingProjectMember == null)
        {
            throw new NotFoundInDBException("Project member not found");
        }

        existingProjectMember.Role = dto.Role;

        await projectMemberRepository.SaveChangesAsync();

        return new ProjectMemberDto
        {
            ProjectId = existingProjectMember.ProjectId,
            UserId = existingProjectMember.UserId,
            Role = existingProjectMember.Role
        };
    }

    public async Task DeleteProjectMember(Guid projectId, Guid userId)
    {
        var projectMember = await projectMemberRepository.GetProjectMember(projectId, userId);

        if (projectMember == null)
        {
            throw new NotFoundInDBException("Project member not found");
        }

        projectMemberRepository.DeleteProjectMember(projectMember);
        await projectMemberRepository.SaveChangesAsync();
    }
}
