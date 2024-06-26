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
