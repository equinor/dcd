using api.Context;
using api.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Project.ProjectMember.Delete;

public class DeleteProjectMemberService(DcdDbContext context)
{
    public async Task DeleteProjectMember(Guid projectId, Guid userId)
    {
        var projectMember = await context.ProjectMembers.SingleOrDefaultAsync(c => c.ProjectId == projectId && c.UserId == userId);

        if (projectMember == null)
        {
            throw new NotFoundInDBException("Project member not found");
        }

        context.ProjectMembers.Remove(projectMember);

        await context.SaveChangesAsync();
    }
}
