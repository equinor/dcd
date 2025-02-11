using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.DrillingSchedules;

public class DrillingScheduleService(DcdDbContext context, RecalculationService recalculationService)
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
            .Include(e => e.ExplorationWells)
            .Where(x => x.Case.ProjectId == projectId)
            .Where(x => x.Id == explorationId)
            .SingleAsync(e => e.ExplorationWells.Any(w => w.WellId == wellId && w.ExplorationId == explorationId));

        var existingExplorationWell = existingExploration.ExplorationWells.FirstOrDefault(w => w.WellId == wellId && w.ExplorationId == explorationId)
                                       ?? throw new NotFoundInDbException($"Drilling schedule with id {drillingScheduleId} not found.");

        existingExplorationWell.StartYear = dto.StartYear;
        existingExplorationWell.Values = dto.Values;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);

        return MapToDto(existingExplorationWell);
    }

    public async Task<TimeSeriesScheduleDto> CreateExplorationWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        CreateTimeSeriesScheduleDto dto)
    {
        var existingExploration = await context.Explorations.SingleAsync(x => x.Case.ProjectId == projectId && x.Id == explorationId);

        var existingWell = await context.Wells.SingleAsync(x => x.ProjectId == projectId && x.Id == wellId);

        var existingCampaign = await context.Campaigns.SingleAsync(x => x.CaseId == caseId && x.CampaignType == CampaignType.ExplorationCampaign);

        var newExplorationWell = new ExplorationWell
        {
            Well = existingWell,
            Exploration = existingExploration,
            CampaignId = existingCampaign.Id,
            StartYear = dto.StartYear,
            Values = dto.Values
        };

        context.ExplorationWell.Add(newExplorationWell);
        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);

        return MapToDto(newExplorationWell);
    }

    public async Task<TimeSeriesScheduleDto> UpdateDevelopmentWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateTimeSeriesScheduleDto dto)
    {
        var existingWellProject = await context.WellProjects
            .Include(e => e.DevelopmentWells)
            .Where(x => x.Case.ProjectId == projectId)
            .Where(x => x.Id == wellProjectId)
            .SingleAsync(e => e.DevelopmentWells.Any(w => w.WellId == wellId && w.WellProjectId == wellProjectId));

        var existingDevelopmentWell = existingWellProject.DevelopmentWells.FirstOrDefault(w => w.WellId == wellId)
                                      ?? throw new NotFoundInDbException($"Drilling schedule with {drillingScheduleId} not found.");

        existingDevelopmentWell.StartYear = dto.StartYear;
        existingDevelopmentWell.Values = dto.Values;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);

        return MapToDto(existingDevelopmentWell);
    }

    public async Task<TimeSeriesScheduleDto> CreateDevelopmentWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        CreateTimeSeriesScheduleDto dto)
    {
        var existingWellProject = await context.WellProjects.SingleAsync(x => x.Case.ProjectId == projectId && x.Id == wellProjectId);

        var existingWell = await context.Wells.SingleAsync(x => x.ProjectId == projectId && x.Id == wellId);

        var existingCampaign = await context.Campaigns.SingleAsync(x => x.CaseId == caseId && x.CampaignType == CampaignType.DevelopmentCampaign);

        var newDevelopmentWell = new DevelopmentWell
        {
            Well = existingWell,
            WellProject = existingWellProject,
            CampaignId = existingCampaign.Id,
            StartYear = dto.StartYear,
            Values = dto.Values
        };

        context.DevelopmentWells.Add(newDevelopmentWell);
        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);

        return MapToDto(newDevelopmentWell);
    }

    private static TimeSeriesScheduleDto MapToDto(ExplorationWell explorationWell)
    {
        return new TimeSeriesScheduleDto
        {
            Id = explorationWell.Id,
            StartYear = explorationWell.StartYear,
            Values = explorationWell.Values
        };
    }

    private static TimeSeriesScheduleDto MapToDto(DevelopmentWell developmentWell)
    {
        return new TimeSeriesScheduleDto
        {
            Id = developmentWell.Id,
            StartYear = developmentWell.StartYear,
            Values = developmentWell.Values
        };
    }
}
