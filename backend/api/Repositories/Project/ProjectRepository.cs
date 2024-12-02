using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class ProjectRepository(DcdDbContext context) : BaseRepository(context), IProjectRepository
{
    public async Task<Project?> GetProject(Guid id)
    {
        return await Context.Projects
            .FirstOrDefaultAsync(p => (p.Id == id || p.FusionProjectId == id) && !p.IsRevision);
    }

    public async Task<Project?> GetProjectWithCases(Guid projectId)
    {
        return await Context.Projects
            .Include(p => p.Cases)
            .FirstOrDefaultAsync(p => p.Id == projectId);
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

        var project = await Context.Projects.SingleOrDefaultAsync(c => c.Id == projectId)
            ?? throw new KeyNotFoundException($"Project with id {projectId} not found.");

        project.ModifyTime = DateTimeOffset.UtcNow;
    }
}
