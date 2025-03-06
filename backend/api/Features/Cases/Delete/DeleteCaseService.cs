using api.Context;
using api.Context.Extensions;
using api.Features.Images.Delete;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Delete;

public class DeleteCaseService(DcdDbContext context, DeleteCaseImageService deleteCaseImageService)
{
    public async Task DeleteCase(Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var imageIds = await context.CaseImages
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.CaseId == caseId)
            .Select(x => x.Id)
            .ToListAsync();

        foreach (var imageId in imageIds)
        {
            await deleteCaseImageService.DeleteImage(projectPk, caseId, imageId);
        }

        var revisionCases = await context.Cases
            .Where(x => x.OriginalCaseId == caseId)
            .ToListAsync();

        foreach (var revisionCase in revisionCases)
        {
            revisionCase.OriginalCaseId = null;
        }

        var caseItem = await context.Cases
            .Include(x => x.Campaigns).ThenInclude(x => x.CampaignWells)
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        var campaigns = await context.Campaigns
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.CaseId == caseId)
            .ToListAsync();

        context.CampaignWells.RemoveRange(caseItem.Campaigns.SelectMany(x => x.CampaignWells));
        context.Campaigns.RemoveRange(campaigns);
        context.Cases.Remove(caseItem);

        await context.SaveChangesAsync();
    }
}
