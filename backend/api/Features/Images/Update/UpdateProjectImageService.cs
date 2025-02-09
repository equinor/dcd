using api.Context;
using api.Context.Extensions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Images.Update;

public class UpdateProjectImageService(DcdDbContext context)
{
    public async Task UpdateImage(Guid projectId, Guid imageId, UpdateImageDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var image = await context.ProjectImages
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == imageId)
            .SingleAsync();

        image.Description = dto.Description;

        await context.SaveChangesAsync();
    }
}
