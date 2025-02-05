using api.Context;
using api.Context.Extensions;
using api.Features.Images.Delete;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Delete;

public class DeleteCaseService(DcdDbContext context, DeleteImageService deleteImageService)
{
    public async Task DeleteCase(Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var imageIds = await context.Images
            .Where(x => x.CaseId == caseId)
            .Select(x => x.Id)
            .ToListAsync();

        foreach (var imageId in imageIds)
        {
            await deleteImageService.DeleteImage(projectPk, imageId);
        }

        var campaigns = await context.Campaigns
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.CaseId == caseId)
            .ToListAsync();

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        await context.Cases
            .Include(x => x.WellProject).ThenInclude(x => x.DevelopmentWells)
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .LoadAsync();

        await context.Cases
            .Include(x => x.Exploration).ThenInclude(x => x.ExplorationWells)
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .LoadAsync();

        context.DevelopmentWells.RemoveRange(caseItem.WellProject.DevelopmentWells);
        context.ExplorationWell.RemoveRange(caseItem.Exploration.ExplorationWells);
        context.Campaigns.RemoveRange(campaigns);
        context.Cases.Remove(caseItem);

        await context.SaveChangesAsync();
    }
}
