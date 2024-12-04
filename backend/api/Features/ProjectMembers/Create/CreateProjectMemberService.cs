using api.Context;
using api.Exceptions;
using api.Features.ProjectMembers.Get;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectMembers.Create;

public class CreateProjectMemberService(DcdDbContext context)
{
    public async Task<ProjectMemberDto> CreateProjectMember(Guid projectId, CreateProjectMemberDto dto)
    {
        var projectMember = new ProjectMember
        {
            ProjectId = projectId,
            UserId = dto.UserId,
            Role = dto.Role,
            FromOrgChart = false
        };

        var existingProjectMember = await context.ProjectMembers.SingleOrDefaultAsync(c => c.ProjectId == projectId && c.UserId == dto.UserId);

        if (existingProjectMember != null)
        {
            throw new ResourceAlreadyExistsException("Project member already exists");
        }

        await context.ProjectMembers.AddAsync(projectMember);
        await context.SaveChangesAsync();

        return new ProjectMemberDto
        {
            ProjectId = projectMember.ProjectId,
            UserId = projectMember.UserId,
            Role = projectMember.Role,
            IsPmt = projectMember.FromOrgChart
        };
    }
}
