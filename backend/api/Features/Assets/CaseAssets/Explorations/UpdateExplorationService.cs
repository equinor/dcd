using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Features.Cases.GetWithAssets;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Explorations;

public class UpdateExplorationService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<ExplorationDto> UpdateExploration(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        UpdateExplorationDto updatedExplorationDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<Exploration>(projectId, explorationId);

        var existingExploration = await context.Explorations.SingleAsync(x => x.Id == explorationId);

        mapperService.MapToEntity(updatedExplorationDto, existingExploration, explorationId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<Exploration, ExplorationDto>(existingExploration, explorationId);
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
                .SingleAsync(e => e.ExplorationWells.Any(w => w.DrillingScheduleId == drillingScheduleId));

        await projectIntegrityService.EntityIsConnectedToProject<Exploration>(projectId, existingExploration.Id);

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
        await projectIntegrityService.EntityIsConnectedToProject<Exploration>(projectId, explorationId);

        var existingExploration = await context.Explorations.SingleAsync(x => x.Id == explorationId);

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
