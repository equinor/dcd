using api.Context;
using api.Context.Extensions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.Create;

public class CreateWellService(DcdDbContext context)
{
    public async Task<Guid> CreateWell(Guid projectId, CreateWellDto createWellDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);
        var project = await context.Projects.SingleAsync(c => c.Id == projectPk);

        project.ModifyTime = DateTime.UtcNow;

        var well = new Well
        {
            ProjectId = projectPk,
            Name = createWellDto.Name,
            WellCategory = createWellDto.WellCategory,
            WellInterventionCost = createWellDto.WellInterventionCost,
            PlugingAndAbandonmentCost = createWellDto.PlugingAndAbandonmentCost,
            WellCost = createWellDto.WellCost,
            DrillingDays = createWellDto.DrillingDays
        };

        context.Wells.Add(well);

        await context.SaveChangesAsync();

        return well.Id;
    }
}
