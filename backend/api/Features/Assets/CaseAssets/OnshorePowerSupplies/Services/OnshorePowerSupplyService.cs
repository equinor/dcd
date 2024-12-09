using System.Linq.Expressions;

using api.Exceptions;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;


public class OnshorePowerSupplyService(
    ILogger<OnshorePowerSupplyService> logger,
    ICaseRepository caseRepository,
    IOnshorePowerSupplyRepository onshorePowerSupplyRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService)
    : IOnshorePowerSupplyService
{
    public async Task<OnshorePowerSupply> GetOnshorePowerSupplyWithIncludes(Guid onshorePowerSupplyId, params Expression<Func<OnshorePowerSupply, object>>[] includes)
    {
        return await onshorePowerSupplyRepository.GetOnshorePowerSupplyWithIncludes(onshorePowerSupplyId, includes)
            ?? throw new NotFoundInDBException($"OnshorePowerSupply with id {onshorePowerSupplyId} not found.");
    }

    public async Task<OnshorePowerSupplyDto> UpdateOnshorePowerSupply<TDto>(Guid projectId, Guid caseId, Guid onshorePowerSupplyId, TDto updatedOnshorePowerSupplyDto)
        where TDto : BaseUpdateOnshorePowerSupplyDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<OnshorePowerSupply>(projectId, onshorePowerSupplyId);

        var existing = await onshorePowerSupplyRepository.GetOnshorePowerSupply(onshorePowerSupplyId)
            ?? throw new NotFoundInDBException($"OnshorePowerSupply with id {onshorePowerSupplyId} not found.");

        mapperService.MapToEntity(updatedOnshorePowerSupplyDto, existing, onshorePowerSupplyId);
        existing.LastChangedDate = DateTimeOffset.UtcNow;

        // OnshorePowerSupply updatedOnshorePowerSupply;
        try
        {
            // updatedOnshorePowerSupply = _repository.UpdateOnshorePowerSupply(existing);
            await caseRepository.UpdateModifyTime(caseId);
            await onshorePowerSupplyRepository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "Failed to update onshorePowerSupply with id {onshorePowerSupplyId} for case id {caseId}.", onshorePowerSupplyId, caseId);
            throw;
        }


        var dto = mapperService.MapToDto<OnshorePowerSupply, OnshorePowerSupplyDto>(existing, onshorePowerSupplyId);
        return dto;
    }
}
