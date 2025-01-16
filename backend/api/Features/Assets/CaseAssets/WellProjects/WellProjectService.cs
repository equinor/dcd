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

namespace api.Features.Assets.CaseAssets.WellProjects;

public class WellProjectService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<WellProjectDto> UpdateWellProject(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        UpdateWellProjectDto updatedWellProjectDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<WellProject>(projectId, wellProjectId);

        var existingWellProject = await context.WellProjects.SingleAsync(x => x.Id == wellProjectId);

        mapperService.MapToEntity(updatedWellProjectDto, existingWellProject, wellProjectId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<WellProject, WellProjectDto>(existingWellProject, wellProjectId);
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
            .SingleAsync(e => e.WellProjectWells.Any(w => w.DrillingScheduleId == drillingScheduleId));

        await projectIntegrityService.EntityIsConnectedToProject<WellProject>(projectId, existingWellProject.Id);

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
        await projectIntegrityService.EntityIsConnectedToProject<WellProject>(projectId, wellProjectId);

        var existingWellProject = await context.WellProjects.SingleAsync(x => x.Id == wellProjectId);

        var existingWell = await context.Wells.SingleAsync(x => x.Id == wellId);

        DrillingSchedule drillingSchedule = new();
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

        if (newWellProjectWell.DrillingSchedule == null)
        {
            throw new Exception(nameof(newWellProjectWell.DrillingSchedule));
        }

        return mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(newWellProjectWell.DrillingSchedule, wellProjectId);
    }
}
