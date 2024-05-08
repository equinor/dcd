using api.Context;
using api.Models;


namespace api.Repositories;

public class SubstructureRepository : ISubstructureRepository
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
        _context.Substructures.Update(substructure);
        await _context.SaveChangesAsync();
        return substructure;
    }

    public async Task<SubstructureCostProfileOverride?> GetSubstructureCostProfileOverride(Guid substructureCostProfileOverrideId)
    {
        return await _context.SubstructureCostProfileOverride.FindAsync(substructureCostProfileOverrideId);
    }

    public async Task<SubstructureCostProfileOverride> UpdateSubstructureCostProfileOverride(SubstructureCostProfileOverride substructureCostProfileOverride)
    {
        _context.SubstructureCostProfileOverride.Update(substructureCostProfileOverride);
        await _context.SaveChangesAsync();
        return substructureCostProfileOverride;
    }
}
