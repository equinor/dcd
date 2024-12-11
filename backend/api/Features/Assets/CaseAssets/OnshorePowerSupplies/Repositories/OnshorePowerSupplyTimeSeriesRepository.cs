using api.Context;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Repositories;
using api.Features.CaseProfiles.Repositories;
using api.Models;


public class OnshorePowerSupplyTimeSeriesRepository(DcdDbContext context) : BaseRepository(context), IOnshorePowerSupplyTimeSeriesRepository
{
    public OnshorePowerSupplyCostProfile CreateOnshorePowerSupplyCostProfile(OnshorePowerSupplyCostProfile onshorePowerSupplyCostProfile)
    {
        Context.OnshorePowerSupplyCostProfile.Add(onshorePowerSupplyCostProfile);
        return onshorePowerSupplyCostProfile;
    }

    public async Task<OnshorePowerSupplyCostProfile?> GetOnshorePowerSupplyCostProfile(Guid onshorePowerSupplyCostProfileId)
    {
        return await GetWithIncludes<OnshorePowerSupplyCostProfile>(onshorePowerSupplyCostProfileId, t => t.OnshorePowerSupply);
    }

    public OnshorePowerSupplyCostProfile UpdateOnshorePowerSupplyCostProfile(OnshorePowerSupplyCostProfile onshorePowerSupplyCostProfile)
    {
        return Update(onshorePowerSupplyCostProfile);
    }

    public OnshorePowerSupplyCostProfileOverride CreateOnshorePowerSupplyCostProfileOverride(OnshorePowerSupplyCostProfileOverride profile)
    {
        Context.OnshorePowerSupplyCostProfileOverride.Add(profile);
        return profile;
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
