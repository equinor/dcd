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

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        context.Cases.Remove(caseItem);

        await context.SaveChangesAsync();
    }
}
