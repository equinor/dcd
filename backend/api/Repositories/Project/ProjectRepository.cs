using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class ProjectRepository : BaseRepository, IProjectRepository
{
    private readonly ILogger<ProjectRepository> _logger;

    public ProjectRepository(
        DcdDbContext context,
        ILogger<ProjectRepository> logger
    ) : base(context)
    {
        _logger = logger;
    }

    public async Task<Project?> GetProject(Guid projectId)
    {
        return await Get<Project>(projectId);
    }

    public async Task<Project?> GetProjectByIdOrExternalId(Guid id)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id || p.FusionProjectId == id);
    }

    public async Task<Project?> GetProjectByExternalId(Guid id)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.FusionProjectId == id);
    }

    public async Task<Project?> GetProjectWithCases(Guid projectId)
    {
        return await _context.Projects
            .Include(p => p.Cases)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public Project UpdateProject(Project updatedProject)
    {
        return Update(updatedProject);
    }

    public async Task<ExplorationOperationalWellCosts?> GetExplorationOperationalWellCosts(Guid id)
    {
        return await Get<ExplorationOperationalWellCosts>(id);
    }

    public async Task<DevelopmentOperationalWellCosts?> GetDevelopmentOperationalWellCosts(Guid id)
    {
        return await Get<DevelopmentOperationalWellCosts>(id);
    }

    public async Task UpdateModifyTime(Guid projectId)
    {
        if (projectId == Guid.Empty)
        {
            throw new ArgumentException("The project id cannot be empty.", nameof(projectId));
        }

        var project = await _context.Projects.SingleOrDefaultAsync(c => c.Id == projectId)
            ?? throw new KeyNotFoundException($"Project with id {projectId} not found.");

        project.ModifyTime = DateTimeOffset.UtcNow;
    }
}
