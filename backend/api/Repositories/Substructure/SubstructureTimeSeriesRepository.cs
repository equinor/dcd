using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class SubstructureTimeSeriesRepository : BaseRepository, ISubstructureTimeSeriesRepository
{

    public SubstructureTimeSeriesRepository(DcdDbContext context) : base(context)
    {
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
