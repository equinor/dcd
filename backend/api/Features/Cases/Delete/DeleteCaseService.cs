using api.Context;
using api.Features.Images.Service;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Delete;

public class DeleteCaseService(DcdDbContext context, IBlobStorageService blobStorageService)
{
    public async Task DeleteCase(Guid projectId, Guid caseId)
    {
        var imageIds = await context.Images
            .Where(x => x.CaseId == caseId)
            .Select(x => x.Id)
            .ToListAsync();

        foreach (var imageId in imageIds)
        {
            await blobStorageService.DeleteImage(imageId);
        }

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.Id == caseId)
            .SingleOrDefaultAsync();

        if (caseItem == null)
        {
            return;
        }

        context.Cases.Remove(caseItem);

        await context.SaveChangesAsync();
    }
}
