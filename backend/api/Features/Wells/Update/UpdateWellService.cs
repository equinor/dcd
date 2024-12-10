using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.Update;

public class UpdateWellService(DcdDbContext context)
{
    public async Task UpdateWell(Guid projectId, Guid wellId, UpdateWellDto updatedWellDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var project = await context.Projects.SingleAsync(c => c.Id == projectPk);
        project.ModifyTime = DateTimeOffset.UtcNow;

        var well = await context.Wells
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == wellId)
            .SingleAsync();

        if (InvalidWellCategory(updatedWellDto))
        {
            throw new InvalidInputException("Invalid well category", wellId);
        }

        if (UpdateChangesWellType(well, updatedWellDto))
        {
            throw new WellChangeTypeException("Cannot change well type", wellId);
        }

        well.Name = updatedWellDto.Name;
        well.WellInterventionCost = updatedWellDto.WellInterventionCost;
        well.PlugingAndAbandonmentCost = updatedWellDto.PlugingAndAbandonmentCost;
        well.WellCategory = updatedWellDto.WellCategory;
        well.WellCost = updatedWellDto.WellCost;
        well.DrillingDays = updatedWellDto.DrillingDays;

        await context.SaveChangesAsync(); // TODO: run calculations
    }

    private static bool InvalidWellCategory(UpdateWellDto updatedWellDto)
    {
        return new[] {
            WellCategory.Oil_Producer,
            WellCategory.Gas_Producer,
            WellCategory.Water_Injector,
            WellCategory.Gas_Injector,
            WellCategory.Exploration_Well,
            WellCategory.Appraisal_Well,
            WellCategory.Sidetrack,
        }.Contains(updatedWellDto.WellCategory);
    }

    private static bool UpdateChangesWellType(Well well, UpdateWellDto updatedWellDto)
    {
        var isWellProjectWell = Well.IsWellProjectWell(well.WellCategory);
        return isWellProjectWell != Well.IsWellProjectWell(updatedWellDto.WellCategory);
    }
}
