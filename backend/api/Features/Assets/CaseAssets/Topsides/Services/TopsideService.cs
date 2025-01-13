using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Update;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Topsides.Services;

public class TopsideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<TopsideDto> UpdateTopside<TDto>(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        TDto updatedTopsideDto
    )
        where TDto : BaseUpdateTopsideDto
    {
        await projectIntegrityService.EntityIsConnectedToProject<Topside>(projectId, topsideId);

        var existingTopside = await context.Topsides.SingleAsync(x => x.Id == topsideId);

        mapperService.MapToEntity(updatedTopsideDto, existingTopside, topsideId);
        existingTopside.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<Topside, TopsideDto>(existingTopside, topsideId);
    }
}
