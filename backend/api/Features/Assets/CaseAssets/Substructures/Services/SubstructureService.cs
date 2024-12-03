using System.Linq.Expressions;

using api.Exceptions;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Update;
using api.Features.Assets.CaseAssets.Substructures.Repositories;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.Models;
using api.Services;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Substructures.Services;

public class SubstructureService(
    ILogger<SubstructureService> logger,
    ISubstructureRepository substructureRepository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService)
    : ISubstructureService
{
    public async Task<Substructure> GetSubstructureWithIncludes(Guid substructureId, params Expression<Func<Substructure, object>>[] includes)
    {
        return await substructureRepository.GetSubstructureWithIncludes(substructureId, includes)
            ?? throw new NotFoundInDBException($"Substructure with id {substructureId} not found.");
    }

    public async Task<SubstructureDto> UpdateSubstructure<TDto>(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        TDto updatedSubstructureDto
    )
        where TDto : BaseUpdateSubstructureDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Substructure>(projectId, substructureId);

        var existingSubstructure = await substructureRepository.GetSubstructure(substructureId)
            ?? throw new NotFoundInDBException($"Substructure with id {substructureId} not found.");

        mapperService.MapToEntity(updatedSubstructureDto, existingSubstructure, substructureId);
        existingSubstructure.LastChangedDate = DateTimeOffset.UtcNow;

        // Substructure updatedSubstructure;
        try
        {
            // updatedSubstructure = _repository.UpdateSubstructure(existingSubstructure);
            await caseRepository.UpdateModifyTime(caseId);
            await substructureRepository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update substructure with id {SubstructureId} for case id {CaseId}.", substructureId, caseId);
            throw;
        }

        var dto = mapperService.MapToDto<Substructure, SubstructureDto>(existingSubstructure, substructureId);

        return dto;
    }
}
