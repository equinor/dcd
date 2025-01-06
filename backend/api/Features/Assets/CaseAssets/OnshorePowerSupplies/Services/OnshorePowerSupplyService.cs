using System.Linq.Expressions;

using api.Exceptions;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;
using api.Features.CaseProfiles.Repositories;
using api.Features.Cases.Recalculation;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

public class OnshorePowerSupplyService(
    ICaseRepository caseRepository,
    IOnshorePowerSupplyRepository onshorePowerSupplyRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService,
    IRecalculationService recalculationService)
    : IOnshorePowerSupplyService
{
    public async Task<OnshorePowerSupply> GetOnshorePowerSupplyWithIncludes(Guid onshorePowerSupplyId, params Expression<Func<OnshorePowerSupply, object>>[] includes)
    {
        return await onshorePowerSupplyRepository.GetOnshorePowerSupplyWithIncludes(onshorePowerSupplyId, includes)
            ?? throw new NotFoundInDbException($"OnshorePowerSupply with id {onshorePowerSupplyId} not found.");
    }

    public async Task<OnshorePowerSupplyDto> UpdateOnshorePowerSupply<TDto>(Guid projectId, Guid caseId, Guid onshorePowerSupplyId, TDto updatedOnshorePowerSupplyDto)
        where TDto : BaseUpdateOnshorePowerSupplyDto
    {
        await projectAccessService.ProjectExists<OnshorePowerSupply>(projectId, onshorePowerSupplyId);

        var existing = await onshorePowerSupplyRepository.GetOnshorePowerSupply(onshorePowerSupplyId)
            ?? throw new NotFoundInDbException($"OnshorePowerSupply with id {onshorePowerSupplyId} not found.");

        mapperService.MapToEntity(updatedOnshorePowerSupplyDto, existing, onshorePowerSupplyId);
        existing.LastChangedDate = DateTime.UtcNow;

        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        var dto = mapperService.MapToDto<OnshorePowerSupply, OnshorePowerSupplyDto>(existing, onshorePowerSupplyId);
        return dto;
    }
}
