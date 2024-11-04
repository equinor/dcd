using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class ProjectMemberRepository : BaseRepository, IProjectMemberRepository
{
    private readonly ILogger<ProjectRepository> _logger;

    public ProjectMemberRepository(
        DcdDbContext context,
        ILogger<ProjectRepository> logger
    ) : base(context)
    {
        _logger = logger;
    }

    public async Task<ProjectMember> CreateProjectMember(ProjectMember projectMember)
    {
        return (await _context.ProjectMembers.AddAsync(projectMember)).Entity;
    }

    public void DeleteProjectMember(ProjectMember projectMember)
    {
        _context.ProjectMembers.Remove(projectMember);
    }

    public async Task<ProjectMember?> GetProjectMember(Guid projectId, Guid userId)
    {
        return await _context.ProjectMembers
            .SingleOrDefaultAsync(c => c.ProjectId == projectId && c.UserId == userId);
    }
}
