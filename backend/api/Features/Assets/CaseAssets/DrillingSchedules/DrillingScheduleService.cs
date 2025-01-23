using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.DrillingSchedules;

public class DrillingScheduleService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task<TimeSeriesScheduleDto> UpdateExplorationWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateTimeSeriesScheduleDto dto)
    {
        var existingExploration = await context.Explorations
            .Include(e => e.ExplorationWells).ThenInclude(w => w.DrillingSchedule)
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.Id == explorationId)
            .SingleAsync(e => e.ExplorationWells.Any(w => w.DrillingScheduleId == drillingScheduleId));

        var existingDrillingSchedule = existingExploration.ExplorationWells.FirstOrDefault(w => w.WellId == wellId)?.DrillingSchedule
                                       ?? throw new NotFoundInDbException($"Drilling schedule with id {drillingScheduleId} not found.");

        existingDrillingSchedule.StartYear = dto.StartYear;
        existingDrillingSchedule.Values = dto.Values;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return MapToDto(existingDrillingSchedule);
    }

    public async Task<TimeSeriesScheduleDto> CreateExplorationWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        CreateTimeSeriesScheduleDto dto)
    {
        var existingExploration = await context.Explorations.SingleAsync(x => x.ProjectId == projectId && x.Id == explorationId);

        var existingWell = await context.Wells.SingleAsync(x => x.ProjectId == projectId && x.Id == wellId);

        var newExplorationWell = new ExplorationWell
        {
            Well = existingWell,
            Exploration = existingExploration,
            DrillingSchedule = new DrillingSchedule
            {
                StartYear = dto.StartYear,
                Values = dto.Values
            }
        };

        context.ExplorationWell.Add(newExplorationWell);
        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        if (newExplorationWell.DrillingSchedule == null)
        {
            throw new Exception(nameof(newExplorationWell.DrillingSchedule));
        }

        return MapToDto(newExplorationWell.DrillingSchedule);
    }

    public async Task<TimeSeriesScheduleDto> UpdateWellProjectWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateTimeSeriesScheduleDto dto)
    {
        var existingWellProject = await context.WellProjects
            .Include(e => e.WellProjectWells).ThenInclude(w => w.DrillingSchedule)
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.Id == wellProjectId)
            .SingleAsync(e => e.WellProjectWells.Any(w => w.DrillingScheduleId == drillingScheduleId));

        var existingDrillingSchedule = existingWellProject.WellProjectWells.FirstOrDefault(w => w.WellId == wellId)?.DrillingSchedule
            ?? throw new NotFoundInDbException($"Drilling schedule with {drillingScheduleId} not found.");

        existingDrillingSchedule.StartYear = dto.StartYear;
        existingDrillingSchedule.Values = dto.Values;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return MapToDto(existingDrillingSchedule);
    }

    public async Task<TimeSeriesScheduleDto> CreateWellProjectWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        CreateTimeSeriesScheduleDto dto)
    {
        var existingWellProject = await context.WellProjects.SingleAsync(x => x.ProjectId == projectId && x.Id == wellProjectId);

        var existingWell = await context.Wells.SingleAsync(x => x.ProjectId == projectId && x.Id == wellId);

        var newWellProjectWell = new WellProjectWell
        {
            Well = existingWell,
            WellProject = existingWellProject,
            DrillingSchedule = new DrillingSchedule
            {
                StartYear = dto.StartYear,
                Values = dto.Values
            }
        };

        context.WellProjectWell.Add(newWellProjectWell);
        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        if (newWellProjectWell.DrillingSchedule == null)
        {
            throw new Exception(nameof(newWellProjectWell.DrillingSchedule));
        }

        return MapToDto(newWellProjectWell.DrillingSchedule);
    }

    private static TimeSeriesScheduleDto MapToDto(DrillingSchedule drillingSchedule)
    {
        return new TimeSeriesScheduleDto
        {
            Id = drillingSchedule.Id,
            StartYear = drillingSchedule.StartYear,
            Values = drillingSchedule.Values
        };
    }
}
