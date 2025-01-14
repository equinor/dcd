using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Assets.CaseAssets.Explorations.Profiles.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Update.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Features.Cases.Recalculation;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Explorations.Update;

public class UpdateExplorationService(DcdDbContext context, IMapperService mapperService, IRecalculationService recalculationService)
{
    public async Task<ExplorationDto> UpdateExploration(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        UpdateExplorationDto updatedExplorationDto)
    {
        var existingExploration = await context.Explorations.SingleAsync(x => x.ProjectId == projectId && x.Id == explorationId);

        existingExploration.RigMobDemob = updatedExplorationDto.RigMobDemob;
        existingExploration.Currency = updatedExplorationDto.Currency;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new ExplorationDto
        {
            Id = existingExploration.Id,
            ProjectId = existingExploration.ProjectId,
            Name = existingExploration.Name,
            RigMobDemob = existingExploration.RigMobDemob,
            Currency = existingExploration.Currency
        };
    }

    public async Task<DrillingScheduleDto> UpdateExplorationWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateDrillingScheduleDto updatedExplorationWellDto)
    {
        var existingExploration = await context.Explorations
                .Include(e => e.ExplorationWells)
                .ThenInclude(w => w.DrillingSchedule)
                .Where(x => x.ProjectId == projectId)
                .SingleAsync(e => e.ExplorationWells.Any(w => w.DrillingScheduleId == drillingScheduleId));

        var existingDrillingSchedule = existingExploration.ExplorationWells.FirstOrDefault(w => w.WellId == wellId)?.DrillingSchedule
            ?? throw new NotFoundInDbException($"Drilling schedule with id {drillingScheduleId} not found.");

        mapperService.MapToEntity(updatedExplorationWellDto, existingDrillingSchedule, drillingScheduleId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(existingDrillingSchedule, drillingScheduleId);
    }

    public async Task<DrillingScheduleDto> CreateExplorationWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        CreateDrillingScheduleDto updatedExplorationWellDto)
    {
        var existingExploration = await context.Explorations.SingleAsync(x => x.ProjectId == projectId && x.Id == explorationId);

        var existingWell = await context.Wells.SingleAsync(x => x.Id == wellId);

        DrillingSchedule drillingSchedule = new();
        var newDrillingSchedule = mapperService.MapToEntity(updatedExplorationWellDto, drillingSchedule, explorationId);

        ExplorationWell newExplorationWell = new()
        {
            Well = existingWell,
            Exploration = existingExploration,
            DrillingSchedule = newDrillingSchedule
        };

        context.ExplorationWell.Add(newExplorationWell);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        if (newExplorationWell.DrillingSchedule == null)
        {
            throw new Exception(nameof(newExplorationWell.DrillingSchedule));
        }

        return mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(newExplorationWell.DrillingSchedule, explorationId);
    }
}
