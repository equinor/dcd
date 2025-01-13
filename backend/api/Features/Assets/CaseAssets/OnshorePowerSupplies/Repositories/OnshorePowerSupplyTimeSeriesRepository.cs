using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Repositories;

public class OnshorePowerSupplyTimeSeriesRepository(DcdDbContext context) : BaseRepository(context)
{
    public async Task<OnshorePowerSupplyCostProfile?> GetOnshorePowerSupplyCostProfile(Guid onshorePowerSupplyCostProfileId)
    {
        return await GetWithIncludes<OnshorePowerSupplyCostProfile>(onshorePowerSupplyCostProfileId, t => t.OnshorePowerSupply);
    }

    public OnshorePowerSupplyCostProfile UpdateOnshorePowerSupplyCostProfile(OnshorePowerSupplyCostProfile onshorePowerSupplyCostProfile)
    {
        return Update(onshorePowerSupplyCostProfile);
    }

    public async Task<OnshorePowerSupplyCostProfileOverride?> GetOnshorePowerSupplyCostProfileOverride(Guid onshorePowerSupplyCostProfileOverrideId)
    {
        return await GetWithIncludes<OnshorePowerSupplyCostProfileOverride>(onshorePowerSupplyCostProfileOverrideId, t => t.OnshorePowerSupply);
    }

    public OnshorePowerSupplyCostProfileOverride UpdateOnshorePowerSupplyCostProfileOverride(OnshorePowerSupplyCostProfileOverride onshorePowerSupplyCostProfileOverride)
    {
        return Update(onshorePowerSupplyCostProfileOverride);
    }
}
