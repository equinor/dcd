using api.Context;
using api.Context.Extensions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectMembers.Create;

public class CreateProjectMemberService(DcdDbContext context)
{
    public async Task<Guid> CreateProjectMember(Guid projectId, CreateProjectMemberDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingProjectMember = await context.ProjectMembers.SingleOrDefaultAsync(c => c.ProjectId == projectPk && c.UserId == dto.UserId);

        if (existingProjectMember != null)
        {
            return dto.UserId;
        }

        context.ProjectMembers.Add(new ProjectMember
        {
            ProjectId = projectPk,
            UserId = dto.UserId,
            Role = dto.Role,
            FromOrgChart = false
        });

        await context.SaveChangesAsync();

        return dto.UserId;
    }
}
