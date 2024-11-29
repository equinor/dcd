using api.Context;
using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectMembers.Get;

public class GetProjectMemberService(DcdDbContext context)
{
    public async Task<List<ProjectMemberDto>> GetProjectMembers(Guid projectId)
    {
        return await context.ProjectMembers
            .Where(c => c.ProjectId == projectId)
            .Select(x => new ProjectMemberDto
            {
                ProjectId = x.ProjectId,
                UserId = x.UserId,
                Role = x.Role
            })
            .ToListAsync();
    }
}
