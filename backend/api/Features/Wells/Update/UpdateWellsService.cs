using api.Context;
using api.Context.Extensions;
using api.Features.Wells.Update.Dtos;
using api.Models;
using api.Models.Infrastructure.ProjectRecalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.Update;

public class UpdateWellsService(DcdDbContext context)
{
    public async Task UpdateWells(Guid projectId, UpdateWellsDto updateWellsDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        await DeleteWells(updateWellsDto.DeleteWellDtos);
        await CreateWells(updateWellsDto.CreateWellDtos, projectPk);
        await UpdateWells(updateWellsDto.UpdateWellDtos);

        if (updateWellsDto.CreateWellDtos.Any() || updateWellsDto.UpdateWellDtos.Any() || updateWellsDto.DeleteWellDtos.Any())
        {
            context.PendingRecalculations.Add(new PendingRecalculation
            {
                ProjectId = projectPk,
                CreatedUtc = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
        }
    }

    private async Task DeleteWells(List<DeleteWellDto> deleteWellDtos)
    {
        if (!deleteWellDtos.Any())
        {
            return;
        }

        foreach (var wellDto in deleteWellDtos)
        {
            var well = await context.Wells.SingleOrDefaultAsync(x => x.Id == wellDto.Id);

            if (well != null)
            {
                var campaignWells = context.CampaignWells.Where(ew => ew.WellId == well.Id);

                foreach (var campaignWell in campaignWells)
                {
                    context.CampaignWells.Remove(campaignWell);
                }

                context.Wells.Remove(well);
            }
        }

        await context.SaveChangesAsync();
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

        foreach (var wellDto in updateWellDtos)
        {
            var existing = await context.Wells.SingleAsync(w => w.Id == wellDto.Id);

            existing.Name = wellDto.Name;
            existing.WellInterventionCost = wellDto.WellInterventionCost;
            existing.PlugingAndAbandonmentCost = wellDto.PlugingAndAbandonmentCost;
            existing.WellCategory = wellDto.WellCategory;
            existing.WellCost = wellDto.WellCost;
            existing.DrillingDays = wellDto.DrillingDays;
        }

        await context.SaveChangesAsync();
    }
}
