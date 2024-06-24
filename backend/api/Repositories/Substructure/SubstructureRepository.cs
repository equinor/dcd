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

    public SubstructureCostProfile CreateSubstructureCostProfile(SubstructureCostProfile substructureCostProfile)
    {
        _context.SubstructureCostProfiles.Add(substructureCostProfile);
        return substructureCostProfile;
    }

    public async Task<SubstructureCostProfile?> GetSubstructureCostProfile(Guid substructureCostProfileId)
    {
        return await Get<SubstructureCostProfile>(substructureCostProfileId);
    }

    public SubstructureCostProfile UpdateSubstructureCostProfile(SubstructureCostProfile substructureCostProfile)
    {
        return Update(substructureCostProfile);
    }

    public SubstructureCostProfileOverride CreateSubstructureCostProfileOverride(SubstructureCostProfileOverride profile)
    {
        _context.SubstructureCostProfileOverride.Add(profile);
        return profile;
    }

    public async Task<SubstructureCostProfileOverride?> GetSubstructureCostProfileOverride(Guid substructureCostProfileOverrideId)
    {
        return await Get<SubstructureCostProfileOverride>(substructureCostProfileOverrideId);
    }

    public SubstructureCostProfileOverride UpdateSubstructureCostProfileOverride(SubstructureCostProfileOverride substructureCostProfileOverride)
    {
        return Update(substructureCostProfileOverride);
    }
}
