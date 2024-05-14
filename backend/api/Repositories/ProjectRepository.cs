using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly DcdDbContext _context;
    private readonly ILogger<ProjectRepository> _logger;

    public ProjectRepository(
        DcdDbContext context,
        ILogger<ProjectRepository> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Project?> GetProject(Guid projectId)
    {
        return await _context.Projects.FindAsync(projectId);
    }
}
