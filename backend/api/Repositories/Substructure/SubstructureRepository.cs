using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class SubstructureRepository : BaseRepository, ISubstructureRepository
{

    public SubstructureRepository(DcdDbContext context) : base(context)
    {
    }

    public async Task<Substructure?> GetSubstructure(Guid substructureId)
    {
        return await Get<Substructure>(substructureId);
    }

    public async Task<Substructure?> GetSubstructureWithCostProfile(Guid substructureId)
    {
        return await _context.Substructures
                        .Include(t => t.CostProfile)
                        .FirstOrDefaultAsync(t => t.Id == substructureId);
    }

    public async Task<bool> SubstructureHasCostProfileOverride(Guid substructureId)
    {
        return await _context.Substructures
            .AnyAsync(t => t.Id == substructureId && t.CostProfileOverride != null);
    }

    public Substructure UpdateSubstructure(Substructure substructure)
    {
        return Update(substructure);
    }
}
