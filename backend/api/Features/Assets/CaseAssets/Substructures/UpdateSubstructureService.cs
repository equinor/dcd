using api.Context;
using api.Context.Extensions;
using api.Features.Cases.GetWithAssets;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Substructures;

public class UpdateSubstructureService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<SubstructureDto> UpdateSubstructure(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureDto updatedSubstructureDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<Substructure>(projectId, substructureId);

        var existingSubstructure = await context.Substructures.SingleAsync(x => x.Id == substructureId);

        mapperService.MapToEntity(updatedSubstructureDto, existingSubstructure, substructureId);
        existingSubstructure.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<Substructure, SubstructureDto>(existingSubstructure, substructureId);
    }

    public async Task UpdateSubstructure(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        ProspUpdateSubstructureDto updatedSubstructureDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<Substructure>(projectId, substructureId);

        var existingSubstructure = await context.Substructures.SingleAsync(x => x.Id == substructureId);

        mapperService.MapToEntity(updatedSubstructureDto, existingSubstructure, substructureId);
        existingSubstructure.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }
}
