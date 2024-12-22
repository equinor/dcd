using api.Context;
using api.Context.Extensions;
using api.Features.CaseProfiles.Dtos.Well;
using api.Features.CaseProfiles.Repositories;
using api.Features.CaseProfiles.Services;
using api.Features.TechnicalInput.Dtos;
using api.Features.Wells.Create;
using api.Features.Wells.Update;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Features.TechnicalInput;

public class TechnicalInputService(
    DcdDbContext context,
    IProjectWithCasesAndAssetsRepository projectWithCasesAndAssetsRepository,
    ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
    IMapper mapper)
{
    public async Task UpdateTechnicalInput(Guid projectId, UpdateTechnicalInputDto technicalInputDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var project = await projectWithCasesAndAssetsRepository.GetProjectWithCasesAndAssets(projectPk);

        if (technicalInputDto.DeleteWellDtos?.Length > 0)
        {
            await DeleteWells(technicalInputDto.DeleteWellDtos);
        }

        if (technicalInputDto.UpdateWellDtos?.Length > 0 || technicalInputDto.CreateWellDtos?.Length > 0)
        {
            await CreateAndUpdateWells(projectPk, technicalInputDto.CreateWellDtos, technicalInputDto.UpdateWellDtos);
        }

        await context.SaveChangesAsync();
    }

    private async Task DeleteWells(DeleteWellDto[] deleteWellDtos)
    {
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
            await costProfileFromDrillingScheduleHelper.UpdateExplorationCostProfiles(explorationId);
        }

        foreach (var wellProjectId in affectedAssets[nameof(WellProject)])
        {
            await costProfileFromDrillingScheduleHelper.UpdateWellProjectCostProfiles(wellProjectId);
        }
    }

    private async Task CreateAndUpdateWells(
            Guid projectId,
            CreateWellDto[]? createWellDtos,
            UpdateWellDto[]? updateWellDtos)
    {
        var updatedWells = new List<Guid>();

        if (createWellDtos != null)
        {
            foreach (var wellDto in createWellDtos)
            {
                var well = mapper.Map<Well>(wellDto);

                if (well == null)
                {
                    throw new ArgumentNullException(nameof(well));
                }

                well.ProjectId = projectId;
                context.Wells.Add(well);
            }
        }

        if (updateWellDtos != null)
        {
            foreach (var wellDto in updateWellDtos)
            {
                var existing = await GetWell(wellDto.Id);

                if (wellDto.WellCost != existing.WellCost || wellDto.WellCategory != existing.WellCategory)
                {
                    updatedWells.Add(wellDto.Id);
                }

                mapper.Map(wellDto, existing);
                context.Wells.Update(existing);
            }
        }

        if (createWellDtos?.Any() == true || updateWellDtos?.Any() == true)
        {
            await context.SaveChangesAsync();
        }

        if (updatedWells.Count != 0)
        {
            await costProfileFromDrillingScheduleHelper.UpdateCostProfilesForWells(updatedWells);
        }
    }

    private async Task<Well> GetWell(Guid wellId)
    {
        var well = await context.Wells
            .Include(e => e.WellProjectWells)
            .Include(e => e.ExplorationWells)
            .FirstOrDefaultAsync(w => w.Id == wellId);

        if (well == null)
        {
            throw new ArgumentException($"Well {wellId} not found.");
        }

        return well;
    }
}
