using api.Models;

public interface IOnshorePowerSupplyRepository
{
    Task<OnshorePowerSupply?> GetOnshorePowerSupply(Guid onshorePowerSupplyId);
    Task<OnshorePowerSupply?> GetOnshorePowerSupplyWithCostProfile(Guid onshorePowerSupplyId);
    Task<bool> OnshorePowerSupplyHasCostProfileOverride(Guid onshorePowerSupplyId);
}
