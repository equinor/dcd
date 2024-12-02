using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.Create;

public class CreateWellService(DcdDbContext context)
{
    public async Task<Guid> CreateWell(Guid projectId, CreateWellDto createWellDto)
    {
        var project = await context.Projects.SingleAsync(c => c.Id == projectId);
        project.ModifyTime = DateTimeOffset.UtcNow;

        var well = new Well
        {
            ProjectId = project.Id,
            Name = createWellDto. Name,
            WellCategory = createWellDto. WellCategory,
            WellInterventionCost = createWellDto. WellInterventionCost,
            PlugingAndAbandonmentCost = createWellDto. PlugingAndAbandonmentCost,
            WellCost = createWellDto. WellCost,
            DrillingDays = createWellDto. DrillingDays
        };

        context.Wells.Add(well);

        await context.SaveChangesAsync();

        return well.Id;
    }
}
