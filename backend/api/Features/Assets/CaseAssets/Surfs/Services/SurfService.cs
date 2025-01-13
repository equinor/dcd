using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Update;
using api.Features.Assets.CaseAssets.Surfs.Repositories;
using api.Features.CaseProfiles.Repositories;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

namespace api.Features.Assets.CaseAssets.Surfs.Services;

public class SurfService(
    ISurfRepository repository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : ISurfService
{
    public async Task<SurfDto> UpdateSurf<TDto>(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        TDto updatedSurfDto
    )
        where TDto : BaseUpdateSurfDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectIntegrityService.EntityIsConnectedToProject<Surf>(projectId, surfId);

        var existingSurf = await repository.GetSurf(surfId)
            ?? throw new ArgumentException($"Surf with id {surfId} not found.");

        mapperService.MapToEntity(updatedSurfDto, existingSurf, surfId);
        existingSurf.LastChangedDate = DateTime.UtcNow;

        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        var dto = mapperService.MapToDto<Surf, SurfDto>(existingSurf, surfId);
        return dto;
    }
}
