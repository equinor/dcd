using api.Context;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.Get;

public class GetWellService(DcdDbContext context)
{
    public async Task<WellDto> GetWell(Guid wellId)
    {
        return await context.Wells
            .Where(x => x.Id == wellId)
            .Select(x => new WellDto
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Name = x.Name,
                WellInterventionCost = x.WellInterventionCost,
                PlugingAndAbandonmentCost = x.PlugingAndAbandonmentCost,
                WellCategory = x.WellCategory,
                WellCost = x.WellCost,
                DrillingDays = x.DrillingDays
            })
            .SingleAsync();
    }
}
