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

    public Project UpdateProject(Project updatedProject)
    {
        return Update(updatedProject);
    }
}
