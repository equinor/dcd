using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class ProjectAccessRepository : IProjectAccessRepository
{
    protected readonly DcdDbContext _context;

    public ProjectAccessRepository(
        DcdDbContext context
    )
    {
        _context = context;
    }

    public async Task<T?> Get<T>(Guid id) where T : class
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<Project?> GetProjectByExternalId(Guid externalId)
    {
        return await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.FusionProjectId == externalId);
    }

    public async Task<Project?> GetProjectById(Guid id)
    {
        return await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }
}
