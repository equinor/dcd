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

    public async Task<ProjectMemberDto> GetProjectMember(Guid projectMemberId)
    {
        return await context.ProjectMembers
            .Where(c => c.Id == projectMemberId)
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
