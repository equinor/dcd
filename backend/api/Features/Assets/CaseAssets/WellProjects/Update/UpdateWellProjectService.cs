using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Assets.CaseAssets.Explorations.Update.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Profiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Features.Cases.Recalculation;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.WellProjects.Update;

public class UpdateWellProjectService(
    DcdDbContext context,
    IMapperService mapperService,
    IRecalculationService recalculationService)
{
    public async Task<WellProjectDto> UpdateWellProject(Guid projectId, Guid caseId, Guid wellProjectId, UpdateWellProjectDto updatedWellProjectDto)
    {
        var existingWellProject = await context.WellProjects.SingleAsync(x => x.ProjectId == projectId && x.Id == wellProjectId);

        existingWellProject.Name = updatedWellProjectDto.Name;
        existingWellProject.ArtificialLift = updatedWellProjectDto.ArtificialLift;
        existingWellProject.Currency = updatedWellProjectDto.Currency;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new WellProjectDto
        {
            Id = existingWellProject.Id,
            ProjectId = existingWellProject.ProjectId,
            Name = existingWellProject.Name,
            ArtificialLift = existingWellProject.ArtificialLift,
            Currency = existingWellProject.Currency
        };
    }

    public async Task<DrillingScheduleDto> UpdateWellProjectWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateDrillingScheduleDto updatedWellProjectWellDto)
    {
        var existingWellProject = await context.WellProjects
            .Include(e => e.WellProjectWells)
            .ThenInclude(w => w.DrillingSchedule)
            .Where(x => x.ProjectId == projectId)
            .SingleAsync(e => e.WellProjectWells.Any(w => w.DrillingScheduleId == drillingScheduleId));

        var existingDrillingSchedule = existingWellProject.WellProjectWells.FirstOrDefault(w => w.WellId == wellId)?.DrillingSchedule
            ?? throw new NotFoundInDbException($"Drilling schedule with {drillingScheduleId} not found.");

        mapperService.MapToEntity(updatedWellProjectWellDto, existingDrillingSchedule, drillingScheduleId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(existingDrillingSchedule, drillingScheduleId);
    }

    public async Task<DrillingScheduleDto> CreateWellProjectWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        CreateDrillingScheduleDto updatedWellProjectWellDto)
    {
        var existingWellProject = await context.WellProjects.SingleAsync(x => x.ProjectId == projectId && x.Id == wellProjectId);

        var existingWell = await context.Wells.SingleAsync(x => x.Id == wellId);

        var drillingSchedule = new DrillingSchedule();
        var newDrillingSchedule = mapperService.MapToEntity(updatedWellProjectWellDto, drillingSchedule, wellProjectId);

        var newWellProjectWell = new WellProjectWell
        {
            Well = existingWell,
            WellProject = existingWellProject,
            DrillingSchedule = newDrillingSchedule
        };

        context.WellProjectWell.Add(newWellProjectWell);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(newWellProjectWell.DrillingSchedule, wellProjectId);
    }
}
