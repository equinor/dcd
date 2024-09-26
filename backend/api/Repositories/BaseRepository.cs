using System.Linq.Expressions;

using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class BaseRepository : IBaseRepository
{
    protected readonly DcdDbContext _context;

    public BaseRepository(
        DcdDbContext context
    )
    {
        _context = context;
    }

    protected async Task<T?> Get<T>(Guid id) where T : class
    {
        return await _context.Set<T>().FindAsync(id);
    }

    protected async Task<T?> GetWithIncludes<T>(Guid id, params Expression<Func<T, object>>[] includes)
        where T : class
    {
        IQueryable<T> query = _context.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    }

    protected T Update<T>(T updated) where T : class
    {
        _context.Set<T>().Update(updated);
        return updated;
    }

    public async Task SaveChangesAndRecalculateAsync(Guid caseId)
    {
        await _context.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
