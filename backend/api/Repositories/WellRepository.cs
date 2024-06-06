using api.Context;
using api.Models;


namespace api.Repositories;

public class WellRepository : IWellRepository
{
    private readonly DcdDbContext _context;

    public WellRepository(DcdDbContext context)
    {
        _context = context;
    }

    public async Task<Well?> GetWell(Guid wellId)
    {
        return await _context.Wells.FindAsync(wellId);
    }

    public async Task<Well> UpdateWell(Well well)
    {
        _context.Wells.Update(well);
        await _context.SaveChangesAsync();
        return well;
    }
}
