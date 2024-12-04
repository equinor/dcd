using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.ProjectMembers.Get;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectMembers.Update;

public class UpdateProjectMemberService(DcdDbContext context)
{
    public async Task<ProjectMemberDto> UpdateProjectMember(Guid projectId, UpdateProjectMemberDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingProjectMember = await context.ProjectMembers
            .SingleOrDefaultAsync(c => c.ProjectId == projectPk && c.UserId == dto.UserId);

        if (existingProjectMember == null)
        {
            throw new ResourceAlreadyExistsException("Project member does not exists");
        }

        existingProjectMember.Role = dto.Role;

        await context.SaveChangesAsync();

        return new ProjectMemberDto
        {
            ProjectId = existingProjectMember.ProjectId,
            UserId = existingProjectMember.UserId,
            Role = existingProjectMember.Role,
            IsPmt = existingProjectMember.FromOrgChart
        };
    }
}
