using api.Context;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.Delete;

public class DeleteWellService(DcdDbContext context)
{
    public async Task DeleteWell(Guid projectId, Guid wellId)
    {
        var project = await context.Projects.SingleAsync(c => c.Id == projectId);
        project.ModifyTime = DateTimeOffset.UtcNow;

        var well = await context.Wells
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.Id == wellId)
            .SingleAsync();

        context.Wells.Remove(well);

        await context.SaveChangesAsync(); // TODO: Run calculations
    }
}
