using api.Context;
using api.Context.Extensions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectMembers.Get;

public class GetProjectMemberService(DcdDbContext context)
{
    public async Task<List<ProjectMemberDto>> GetProjectMembers(Guid projectId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        return await context.ProjectMembers
            .Where(c => c.ProjectId == projectPk)
            .Select(x => new ProjectMemberDto
            {
                ProjectId = x.ProjectId,
                UserId = x.UserId,
                Role = x.Role,
                IsPmt = x.FromOrgChart
            })
            .ToListAsync();
    }

    public async Task<ProjectMemberDto> GetProjectMember(Guid projectId, Guid projectMemberId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        return await context.ProjectMembers
            .Where(x => x.ProjectId == projectPk)
            .Where(c => c.UserId == projectMemberId)
            .Select(x => new ProjectMemberDto
            {
                ProjectId = x.ProjectId,
                UserId = x.UserId,
                Role = x.Role,
                IsPmt = x.FromOrgChart
            })
            .SingleAsync();
    }
}
