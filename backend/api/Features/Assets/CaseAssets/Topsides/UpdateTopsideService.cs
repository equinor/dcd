using api.Context;
using api.Context.Extensions;
using api.Features.Cases.GetWithAssets;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Topsides;

public class UpdateTopsideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<TopsideDto> UpdateTopside(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        UpdateTopsideDto updatedTopsideDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<Topside>(projectId, topsideId);

        var existingTopside = await context.Topsides.SingleAsync(x => x.Id == topsideId);

        mapperService.MapToEntity(updatedTopsideDto, existingTopside, topsideId);
        existingTopside.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<Topside, TopsideDto>(existingTopside, topsideId);
    }

    public async Task UpdateTopside(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        ProspUpdateTopsideDto updatedTopsideDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<Topside>(projectId, topsideId);

        var existingTopside = await context.Topsides.SingleAsync(x => x.Id == topsideId);

        mapperService.MapToEntity(updatedTopsideDto, existingTopside, topsideId);
        existingTopside.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }
}
