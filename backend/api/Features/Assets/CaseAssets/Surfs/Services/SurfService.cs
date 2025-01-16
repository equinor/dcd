using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Update;
using api.Features.Cases.GetWithAssets;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Surfs.Services;

public class SurfService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<SurfDto> UpdateSurf<TDto>(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        TDto updatedSurfDto
    )
        where TDto : BaseUpdateSurfDto
    {
        await projectIntegrityService.EntityIsConnectedToProject<Surf>(projectId, surfId);

        var existingSurf = await context.Surfs.SingleAsync(x => x.Id == surfId);

        mapperService.MapToEntity(updatedSurfDto, existingSurf, surfId);
        existingSurf.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<Surf, SurfDto>(existingSurf, surfId);
    }
}
