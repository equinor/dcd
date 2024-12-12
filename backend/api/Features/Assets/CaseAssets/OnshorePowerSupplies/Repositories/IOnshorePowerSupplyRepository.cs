

using System.Linq.Expressions;

using api.Features.CaseProfiles.Repositories;
using api.Models;

public interface IOnshorePowerSupplyRepository
{
    Task<OnshorePowerSupply?> GetOnshorePowerSupply(Guid onshorePowerSupplyId);
    Task<OnshorePowerSupply?> GetOnshorePowerSupplyWithIncludes(Guid onshorePowerSupplyId, params Expression<Func<OnshorePowerSupply, object>>[] includes);
    Task<OnshorePowerSupply?> GetOnshorePowerSupplyWithCostProfile(Guid onshorePowerSupplyId);
    Task<bool> OnshorePowerSupplyHasCostProfileOverride(Guid onshorePowerSupplyId);
    OnshorePowerSupply UpdateOnshorePowerSupply(OnshorePowerSupply onshorePowerSupply);
}
