using System.Linq.Expressions;

using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class BaseRepository(DcdDbContext context) : IBaseRepository
{
    protected readonly DcdDbContext Context = context;

    protected async Task<T?> Get<T>(Guid id) where T : class
    {
        return await Context.Set<T>().FindAsync(id);
    }

    protected async Task<T?> GetWithIncludes<T>(Guid id, params Expression<Func<T, object>>[] includes)
        where T : class
    {
        IQueryable<T> query = Context.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    }

    protected T Update<T>(T updated) where T : class
    {
        Context.Set<T>().Update(updated);
        return updated;
    }

    public async Task SaveChangesAndRecalculateAsync(Guid caseId)
    {
        await Context.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }
}
