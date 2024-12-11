

using System.Linq.Expressions;

using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;
using api.Models;

public interface IOnshorePowerSupplyService
{
    Task<OnshorePowerSupply> GetOnshorePowerSupplyWithIncludes(Guid onshorePowerSupplyId, params Expression<Func<OnshorePowerSupply, object>>[] includes);
    Task<OnshorePowerSupplyDto> UpdateOnshorePowerSupply<TDto>(Guid projectId, Guid caseId, Guid onshorePowerSupplyId, TDto updatedOnshorePowerSupplyDto)
            where TDto : BaseUpdateOnshorePowerSupplyDto;
}
