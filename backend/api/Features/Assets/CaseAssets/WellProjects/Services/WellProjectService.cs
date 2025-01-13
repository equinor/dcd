using api.Exceptions;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Repositories;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Features.CaseProfiles.Repositories;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

namespace api.Features.Assets.CaseAssets.WellProjects.Services;

public class WellProjectService(
    IWellProjectRepository repository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : IWellProjectService
{
    public async Task<WellProjectDto> UpdateWellProject(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        UpdateWellProjectDto updatedWellProjectDto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectIntegrityService.EntityIsConnectedToProject<WellProject>(projectId, wellProjectId);

        var existingWellProject = await repository.GetWellProject(wellProjectId)
            ?? throw new NotFoundInDbException($"Well project with id {wellProjectId} not found.");

        mapperService.MapToEntity(updatedWellProjectDto, existingWellProject, wellProjectId);

        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        var dto = mapperService.MapToDto<WellProject, WellProjectDto>(existingWellProject, wellProjectId);
        return dto;
    }

    public async Task<DrillingScheduleDto> UpdateWellProjectWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateDrillingScheduleDto updatedWellProjectWellDto
    )
    {
        var existingWellProject = await repository.GetWellProjectWithDrillingSchedule(drillingScheduleId)
            ?? throw new NotFoundInDbException($"No wellproject connected to {drillingScheduleId} found.");

        // Need to verify that the project from the URL is the same as the project of the resource
        await projectIntegrityService.EntityIsConnectedToProject<WellProject>(projectId, existingWellProject.Id);

        var existingDrillingSchedule = existingWellProject.WellProjectWells?.FirstOrDefault(w => w.WellId == wellId)?.DrillingSchedule
            ?? throw new NotFoundInDbException($"Drilling schedule with {drillingScheduleId} not found.");

        mapperService.MapToEntity(updatedWellProjectWellDto, existingDrillingSchedule, drillingScheduleId);

        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        var dto = mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(existingDrillingSchedule, drillingScheduleId);
        return dto;
    }

    public async Task<DrillingScheduleDto> CreateWellProjectWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        CreateDrillingScheduleDto updatedWellProjectWellDto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectIntegrityService.EntityIsConnectedToProject<WellProject>(projectId, wellProjectId);

        var existingWellProject = await repository.GetWellProject(wellProjectId)
            ?? throw new NotFoundInDbException($"Well project with {wellProjectId} not found.");

        var existingWell = await repository.GetWell(wellId)
            ?? throw new NotFoundInDbException($"Well with {wellId} not found.");

        DrillingSchedule drillingSchedule = new();
        var newDrillingSchedule = mapperService.MapToEntity(updatedWellProjectWellDto, drillingSchedule, wellProjectId);

        WellProjectWell newWellProjectWell = new()
        {
            Well = existingWell,
            WellProject = existingWellProject,
            DrillingSchedule = newDrillingSchedule
        };

        var createdWellProjectWell = repository.CreateWellProjectWellDrillingSchedule(newWellProjectWell);
        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        if (createdWellProjectWell.DrillingSchedule == null)
        {
            // TODO: use a more specific exception
            throw new Exception(nameof(createdWellProjectWell.DrillingSchedule));
        }

        var dto = mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(createdWellProjectWell.DrillingSchedule, wellProjectId);
        return dto;
    }
}
