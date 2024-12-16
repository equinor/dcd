using api.Context;
using api.Context.Extensions;
using api.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectMembers.Update;

public class UpdateProjectMemberService(DcdDbContext context)
{
    public async Task UpdateProjectMember(Guid projectId, UpdateProjectMemberDto dto)
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
    }
}
