using api.Models;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Repositories;

public interface IOnshorePowerSupplyTimeSeriesRepository
{
    OnshorePowerSupplyCostProfile CreateOnshorePowerSupplyCostProfile(OnshorePowerSupplyCostProfile onshorePowerSupplyCostProfile);
    Task<OnshorePowerSupplyCostProfile?> GetOnshorePowerSupplyCostProfile(Guid onshorePowerSupplyostProfileId);
    OnshorePowerSupplyCostProfile UpdateOnshorePowerSupplyCostProfile(OnshorePowerSupplyCostProfile onshorePowerSupplyostProfile);
    OnshorePowerSupplyCostProfileOverride CreateOnshorePowerSupplyCostProfileOverride(OnshorePowerSupplyCostProfileOverride profile);
    Task<OnshorePowerSupplyCostProfileOverride?> GetOnshorePowerSupplyCostProfileOverride(Guid onshorePowerSupplyostProfileOverrideId);
    OnshorePowerSupplyCostProfileOverride UpdateOnshorePowerSupplyCostProfileOverride(OnshorePowerSupplyCostProfileOverride onshorePowerSupplyostProfileOverride);
}
