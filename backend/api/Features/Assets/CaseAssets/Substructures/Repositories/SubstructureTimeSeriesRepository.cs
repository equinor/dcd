using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.Substructures.Repositories;

public class SubstructureTimeSeriesRepository(DcdDbContext context) : BaseRepository(context)
{
    public async Task<SubstructureCostProfile?> GetSubstructureCostProfile(Guid substructureCostProfileId)
    {
        return await GetWithIncludes<SubstructureCostProfile>(substructureCostProfileId, s => s.Substructure);
    }

    public SubstructureCostProfile UpdateSubstructureCostProfile(SubstructureCostProfile substructureCostProfile)
    {
        return Update(substructureCostProfile);
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
