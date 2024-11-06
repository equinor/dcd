using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class ProjectMemberRepository(DcdDbContext context) : BaseRepository(context), IProjectMemberRepository
{
    public async Task<ProjectMember> CreateProjectMember(ProjectMember projectMember)
    {
        return (await Context.ProjectMembers.AddAsync(projectMember)).Entity;
    }

    public void DeleteProjectMember(ProjectMember projectMember)
    {
        Context.ProjectMembers.Remove(projectMember);
    }

    public async Task<ProjectMember?> GetProjectMember(Guid projectId, Guid userId)
    {
        return await Context.ProjectMembers
            .SingleOrDefaultAsync(c => c.ProjectId == projectId && c.UserId == userId);
    }
}
