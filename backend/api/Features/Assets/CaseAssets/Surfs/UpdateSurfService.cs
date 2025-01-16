using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Surfs;

public class UpdateSurfService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task UpdateSurf(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        UpdateSurfDto updatedSurfDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<Surf>(projectId, surfId);

        var existingSurf = await context.Surfs.SingleAsync(x => x.Id == surfId);

        mapperService.MapToEntity(updatedSurfDto, existingSurf, surfId);
        existingSurf.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task UpdateSurf(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        ProspUpdateSurfDto updatedSurfDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<Surf>(projectId, surfId);

        var existingSurf = await context.Surfs.SingleAsync(x => x.Id == surfId);

        mapperService.MapToEntity(updatedSurfDto, existingSurf, surfId);
        existingSurf.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }
}
