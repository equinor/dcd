using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.WellProjects;

public class UpdateWellProjectService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task UpdateWellProject(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        UpdateWellProjectDto updatedWellProjectDto)
    {
        var existingWellProject = await context.WellProjects.SingleAsync(x => x.ProjectId == projectId && x.Id == wellProjectId);

        existingWellProject.Name = updatedWellProjectDto.Name;
        existingWellProject.ArtificialLift = updatedWellProjectDto.ArtificialLift;
        existingWellProject.Currency = updatedWellProjectDto.Currency;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }
}
