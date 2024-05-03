using System;

using api.Context;
using api.Models;


namespace api.Repositories;

public class SubstructureRepository
{
    private readonly DcdDbContext _context;

    public SubstructureRepository(DcdDbContext context)
    {
        _context = context;
    }

    public async Task<Substructure?> GetSubstructure(Guid substructureId)
    {
        return await _context.Substructures.FindAsync(substructureId);
    }

    public async Task<Substructure> UpdateSubstructure(Substructure substructure)
    {
        ArgumentNullException.ThrowIfNull(substructure);

        _context.Substructures.Update(substructure);
        await _context.SaveChangesAsync();
        return substructure;
    }
}
