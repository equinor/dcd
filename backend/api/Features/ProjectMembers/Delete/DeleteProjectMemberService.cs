using api.Context;
using api.Context.Extensions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectMembers.Delete;

public class DeleteProjectMemberService(DcdDbContext context)
{
    public async Task DeleteProjectMember(Guid projectId, Guid azureAdUserId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var projectMember = await context.ProjectMembers.SingleOrDefaultAsync(c => c.ProjectId == projectPk && c.AzureAdUserId == azureAdUserId);

        if (projectMember == null)
        {
            return;
        }

        context.ProjectMembers.Remove(projectMember);

        await context.SaveChangesAsync();
    }
}
