using api.Context;
using api.Context.Extensions;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.GetIsInUse;

public class GetIsWellInUseService(DcdDbContext context)
{
    public async Task<bool> IsWellInUse(Guid projectId, Guid wellId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);

        return await context.CampaignWells
            .Where(x => x.WellId == wellId)
            .Where(x => x.Well.ProjectId == projectPk)
            .AnyAsync();
    }
}
