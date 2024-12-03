using api.Context;
using api.Models;
using api.Repositories;

namespace api.Features.Assets.CaseAssets.Substructures.Repositories;

public class SubstructureTimeSeriesRepository(DcdDbContext context) : BaseRepository(context), ISubstructureTimeSeriesRepository
{
    public SubstructureCostProfile CreateSubstructureCostProfile(SubstructureCostProfile substructureCostProfile)
    {
        Context.SubstructureCostProfiles.Add(substructureCostProfile);
        return substructureCostProfile;
    }

    public async Task<SubstructureCostProfile?> GetSubstructureCostProfile(Guid substructureCostProfileId)
    {
        return await GetWithIncludes<SubstructureCostProfile>(substructureCostProfileId, s => s.Substructure);
    }

    public SubstructureCostProfile UpdateSubstructureCostProfile(SubstructureCostProfile substructureCostProfile)
    {
        return Update(substructureCostProfile);
    }

    public SubstructureCostProfileOverride CreateSubstructureCostProfileOverride(SubstructureCostProfileOverride profile)
    {
        Context.SubstructureCostProfileOverride.Add(profile);
        return profile;
    }

    public async Task<SubstructureCostProfileOverride?> GetSubstructureCostProfileOverride(Guid substructureCostProfileOverrideId)
    {
        return await GetWithIncludes<SubstructureCostProfileOverride>(substructureCostProfileOverrideId, s => s.Substructure);
    }

    public SubstructureCostProfileOverride UpdateSubstructureCostProfileOverride(SubstructureCostProfileOverride substructureCostProfileOverride)
    {
        return Update(substructureCostProfileOverride);
    }
}
