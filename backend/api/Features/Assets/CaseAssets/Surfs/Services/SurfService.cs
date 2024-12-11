using System.Linq.Expressions;

using api.Context;
using api.Context.Recalculation;
using api.Exceptions;
using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Update;
using api.Features.Assets.CaseAssets.Surfs.Repositories;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Surfs.Services;

public class SurfService(
    ILogger<SurfService> logger,
    ISurfRepository repository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService,
    IRecalculationService recalculationService)
    : ISurfService
{
    public async Task<Surf> GetSurfWithIncludes(Guid surfId, params Expression<Func<Surf, object>>[] includes)
    {
        return await repository.GetSurfWithIncludes(surfId, includes)
            ?? throw new NotFoundInDBException($"Topside with id {surfId} not found.");
    }

    public async Task<SurfDto> UpdateSurf<TDto>(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        TDto updatedSurfDto
    )
        where TDto : BaseUpdateSurfDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Surf>(projectId, surfId);

        var existingSurf = await repository.GetSurf(surfId)
            ?? throw new ArgumentException($"Surf with id {surfId} not found.");

        mapperService.MapToEntity(updatedSurfDto, existingSurf, surfId);
        existingSurf.LastChangedDate = DateTimeOffset.UtcNow;

        try
        {
            await caseRepository.UpdateModifyTime(caseId);
            await recalculationService.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update surf with id {surfId} for case id {caseId}.", surfId, caseId);
            throw;
        }


        var dto = mapperService.MapToDto<Surf, SurfDto>(existingSurf, surfId);
        return dto;
    }
}
