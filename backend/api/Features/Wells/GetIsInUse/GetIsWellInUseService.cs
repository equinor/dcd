using api.Context;
using api.Context.Extensions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.GetIsInUse;

public class GetIsWellInUseService(DcdDbContext context)
{
    public async Task<bool> IsWellInUse(Guid projectId, Guid wellId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);

        var well = await context.Wells
            .Include(w => w.CampaignWells)
            .SingleOrDefaultAsync(w => w.ProjectId == projectPk && w.Id == wellId);

        if (well == null)
        {
            return false;
        }

        return well.CampaignWells.Any();
    }
}
