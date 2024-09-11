using api.Context;

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
}
