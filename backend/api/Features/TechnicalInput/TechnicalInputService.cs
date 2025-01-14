using api.Context;
using api.Context.Extensions;
using api.Features.TechnicalInput.Dtos;
using api.Features.Wells.Create;
using api.Features.Wells.Update;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.TechnicalInput;

public class TechnicalInputService(
    DcdDbContext context,
    UpdateExplorationWellCostProfilesService updateExplorationWellCostProfilesService,
    UpdateWellProjectCostProfilesService updateWellProjectCostProfilesService)
{
    public async Task UpdateTechnicalInput(Guid projectId, UpdateTechnicalInputDto technicalInputDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        await DeleteWells(technicalInputDto.DeleteWellDtos);
        await CreateWells(technicalInputDto.CreateWellDtos, projectPk);
        await UpdateWells(technicalInputDto.UpdateWellDtos);

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

                var wellProjectWells = context.WellProjectWell.Where(ew => ew.WellId == well.Id);

                foreach (var wellProjectWell in wellProjectWells)
                {
                    context.WellProjectWell.Remove(wellProjectWell);
                    affectedAssets[nameof(WellProject)].Add(wellProjectWell.WellProjectId);
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
                .Include(e => e.WellProjectWells)
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
