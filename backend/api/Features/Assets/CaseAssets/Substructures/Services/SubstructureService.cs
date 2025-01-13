using api.Exceptions;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Update;
using api.Features.Assets.CaseAssets.Substructures.Repositories;
using api.Features.CaseProfiles.Repositories;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

namespace api.Features.Assets.CaseAssets.Substructures.Services;

public class SubstructureService(
    ISubstructureRepository substructureRepository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : ISubstructureService
{
    public async Task<SubstructureDto> UpdateSubstructure<TDto>(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        TDto updatedSubstructureDto
    )
        where TDto : BaseUpdateSubstructureDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectIntegrityService.EntityIsConnectedToProject<Substructure>(projectId, substructureId);

        var existingSubstructure = await substructureRepository.GetSubstructure(substructureId)
            ?? throw new NotFoundInDbException($"Substructure with id {substructureId} not found.");

        mapperService.MapToEntity(updatedSubstructureDto, existingSubstructure, substructureId);
        existingSubstructure.LastChangedDate = DateTime.UtcNow;

        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        var dto = mapperService.MapToDto<Substructure, SubstructureDto>(existingSubstructure, substructureId);

        return dto;
    }
}
