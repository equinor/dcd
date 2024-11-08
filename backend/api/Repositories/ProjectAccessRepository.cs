using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class ProjectAccessRepository(DcdDbContext context) : IProjectAccessRepository
{
    public async Task<T?> Get<T>(Guid id) where T : class
    {
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<Project?> GetProjectByExternalId(Guid externalId)
    {
        return await context.Projects.FirstOrDefaultAsync(p => p.FusionProjectId == externalId && !p.IsRevision);
    }

    public async Task<Project?> GetProjectById(Guid id)
    {
        return await context.Projects
            .Include(p => p.ProjectMembers)
            .FirstOrDefaultAsync(p => p.Id == id || (p.FusionProjectId == id && !p.IsRevision));
    }
}
