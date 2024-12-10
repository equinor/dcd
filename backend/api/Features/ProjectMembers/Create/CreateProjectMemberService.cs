using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.ProjectMembers.Get;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectMembers.Create;

public class CreateProjectMemberService(DcdDbContext context)
{
    public async Task<ProjectMemberDto> CreateProjectMember(Guid projectId, CreateProjectMemberDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var projectMember = new ProjectMember
        {
            ProjectId = projectPk,
            UserId = dto.UserId,
            Role = dto.Role
        };

        var existingProjectMember = await context.ProjectMembers.SingleOrDefaultAsync(c => c.ProjectId == projectPk && c.UserId == dto.UserId);

        if (existingProjectMember != null)
        {
            throw new ResourceAlreadyExistsException("Project member already exists");
        }

        context.ProjectMembers.Add(projectMember);

        await context.SaveChangesAsync();

        return new ProjectMemberDto
        {
            ProjectId = projectPk,
            UserId = projectMember.UserId,
            Role = projectMember.Role
        };
    }
}
