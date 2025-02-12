using api.Context;
using api.Context.Extensions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Images.Update;

public class UpdateCaseImageService(DcdDbContext context)
{
    public async Task UpdateImage(Guid projectId, Guid caseId, Guid imageId, UpdateImageDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var image = await context.CaseImages
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Id == imageId)
            .SingleAsync();

        image.Description = dto.Description;

        await context.SaveChangesAsync();
    }
}
