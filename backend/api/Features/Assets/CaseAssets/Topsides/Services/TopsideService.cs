using System.Linq.Expressions;

using api.Context.Recalculation;
using api.Exceptions;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Update;
using api.Features.Assets.CaseAssets.Topsides.Repositories;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Topsides.Services;

public class TopsideService(
    ILogger<TopsideService> logger,
    ITopsideRepository repository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService,
    IRecalculationService recalculationService)
    : ITopsideService
{
    public async Task<Topside> GetTopsideWithIncludes(Guid topsideId, params Expression<Func<Topside, object>>[] includes)
    {
        return await repository.GetTopsideWithIncludes(topsideId, includes)
            ?? throw new NotFoundInDBException($"Topside with id {topsideId} not found.");
    }

    public async Task<TopsideDto> UpdateTopside<TDto>(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        TDto updatedTopsideDto
    )
        where TDto : BaseUpdateTopsideDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Topside>(projectId, topsideId);

        var existingTopside = await repository.GetTopside(topsideId)
            ?? throw new NotFoundInDBException($"Topside with id {topsideId} not found.");

        mapperService.MapToEntity(updatedTopsideDto, existingTopside, topsideId);
        existingTopside.LastChangedDate = DateTimeOffset.UtcNow;

        try
        {
            await caseRepository.UpdateModifyTime(caseId);
            await recalculationService.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update topside with id {topsideId} for case id {caseId}.", topsideId, caseId);
            throw;
        }

        var dto = mapperService.MapToDto<Topside, TopsideDto>(existingTopside, topsideId);
        return dto;
    }
}
