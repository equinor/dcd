using api.Context;
using api.Context.Extensions;
using api.Features.Wells.Update.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.Update;

public class UpdateWellsService(
    DcdDbContext context,
    UpdateExplorationWellCostProfilesService updateExplorationWellCostProfilesService,
    UpdateWellProjectCostProfilesService updateWellProjectCostProfilesService)
{
    public async Task UpdateWells(Guid projectId, UpdateWellsDto updateWellsDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        await DeleteWells(updateWellsDto.DeleteWellDtos);
        await CreateWells(updateWellsDto.CreateWellDtos, projectPk);
        await UpdateWells(updateWellsDto.UpdateWellDtos);

        await context.SaveChangesAsync();
    }

    private async Task DeleteWells(List<DeleteWellDto> deleteWellDtos)
    {
        if (!deleteWellDtos.Any())
        {
            return;
        }

        var affectedAssets = new Dictionary<string, List<Guid>>
        {
            { nameof(Exploration), [] },
            { nameof(WellProject), [] }
        };

        foreach (var wellDto in deleteWellDtos)
        {
            var well = await context.Wells.FindAsync(wellDto.Id);

            if (well != null)
            {
                var explorationWells = context.ExplorationWell.Where(ew => ew.WellId == well.Id);

                foreach (var explorationWell in explorationWells)
                {
                    context.ExplorationWell.Remove(explorationWell);
                    affectedAssets[nameof(Exploration)].Add(explorationWell.ExplorationId);
                }

                var developmentWells = context.DevelopmentWells.Where(ew => ew.WellId == well.Id);

                foreach (var developmentWell in developmentWells)
                {
                    context.DevelopmentWells.Remove(developmentWell);
                    affectedAssets[nameof(WellProject)].Add(developmentWell.WellProjectId);
                }

                context.Wells.Remove(well);
            }
        }

        await context.SaveChangesAsync();

        foreach (var explorationId in affectedAssets[nameof(Exploration)])
        {
            await updateExplorationWellCostProfilesService.UpdateExplorationCostProfiles(explorationId);
        }

        foreach (var wellProjectId in affectedAssets[nameof(WellProject)])
        {
            await updateWellProjectCostProfilesService.UpdateWellProjectCostProfiles(wellProjectId);
        }
    }

    private async Task CreateWells(List<CreateWellDto> createWellDtos, Guid projectPk)
    {
        if (!createWellDtos.Any())
        {
            return;
        }

        foreach (var wellDto in createWellDtos)
        {
            context.Wells.Add(new Well
            {
                ProjectId = projectPk,
                Name = wellDto.Name,
                WellCategory = wellDto.WellCategory,
                WellInterventionCost = wellDto.WellInterventionCost,
                PlugingAndAbandonmentCost = wellDto.PlugingAndAbandonmentCost,
                WellCost = wellDto.WellCost,
                DrillingDays = wellDto.DrillingDays
            });
        }

        await context.SaveChangesAsync();
    }

    private async Task UpdateWells(List<UpdateWellDto> updateWellDtos)
    {
        if (!updateWellDtos.Any())
        {
            return;
        }

        var updatedWells = new List<Guid>();

        foreach (var wellDto in updateWellDtos)
        {
            var existing = await context.Wells
                .Include(e => e.DevelopmentWells)
                .Include(e => e.ExplorationWells)
                .SingleAsync(w => w.Id == wellDto.Id);

            if (wellDto.WellCost != existing.WellCost || wellDto.WellCategory != existing.WellCategory)
            {
                updatedWells.Add(wellDto.Id);
            }

            existing.Name = wellDto.Name;
            existing.WellInterventionCost = wellDto.WellInterventionCost;
            existing.PlugingAndAbandonmentCost = wellDto.PlugingAndAbandonmentCost;
            existing.WellCategory = wellDto.WellCategory;
            existing.WellCost = wellDto.WellCost;
            existing.DrillingDays = wellDto.DrillingDays;
        }

        await context.SaveChangesAsync();

        if (updatedWells.Count != 0)
        {
            await updateExplorationWellCostProfilesService.HandleExplorationWell(updatedWells);
            await updateWellProjectCostProfilesService.HandleWellProjects(updatedWells);

            await context.SaveChangesAsync();
        }
    }
}
