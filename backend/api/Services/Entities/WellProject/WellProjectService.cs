using System.Linq.Expressions;

using api.Dtos;
using api.Exceptions;
using api.Features.ProjectAccess;
using api.Models;
using api.Repositories;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellProjectService(
    ILogger<WellProjectService> logger,
    IWellProjectRepository repository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService)
    : IWellProjectService
{
    public async Task<WellProject> GetWellProjectWithIncludes(Guid wellProjectId, params Expression<Func<WellProject, object>>[] includes)
    {
        return await repository.GetWellProjectWithIncludes(wellProjectId, includes)
            ?? throw new NotFoundInDBException($"WellProject with id {wellProjectId} not found.");
    }

    public async Task<WellProjectDto> UpdateWellProject(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        UpdateWellProjectDto updatedWellProjectDto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<WellProject>(projectId, wellProjectId);

        var existingWellProject = await repository.GetWellProject(wellProjectId)
            ?? throw new NotFoundInDBException($"Well project with id {wellProjectId} not found.");

        mapperService.MapToEntity(updatedWellProjectDto, existingWellProject, wellProjectId);

        try
        {
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update well project with id {wellProjectId} for case id {caseId}.", wellProjectId, caseId);
            throw;
        }

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
            ?? throw new NotFoundInDBException($"No wellproject connected to {drillingScheduleId} found.");

        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<WellProject>(projectId, existingWellProject.Id);

        var existingDrillingSchedule = existingWellProject.WellProjectWells?.FirstOrDefault(w => w.WellId == wellId)?.DrillingSchedule
            ?? throw new NotFoundInDBException($"Drilling schedule with {drillingScheduleId} not found.");

        mapperService.MapToEntity(updatedWellProjectWellDto, existingDrillingSchedule, drillingScheduleId);

        // DrillingSchedule updatedDrillingSchedule;
        try
        {
            // updatedDrillingSchedule = _repository.UpdateWellProjectWellDrillingSchedule(existingDrillingSchedule);
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update drilling schedule with id {drillingScheduleId}", drillingScheduleId);
            throw;
        }

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
        await projectAccessService.ProjectExists<WellProject>(projectId, wellProjectId);

        var existingWellProject = await repository.GetWellProject(wellProjectId)
            ?? throw new NotFoundInDBException($"Well project with {wellProjectId} not found.");

        var existingWell = await repository.GetWell(wellId)
            ?? throw new NotFoundInDBException($"Well with {wellId} not found.");

        DrillingSchedule drillingSchedule = new();
        var newDrillingSchedule = mapperService.MapToEntity(updatedWellProjectWellDto, drillingSchedule, wellProjectId);

        WellProjectWell newWellProjectWell = new()
        {
            Well = existingWell,
            WellProject = existingWellProject,
            DrillingSchedule = newDrillingSchedule
        };

        WellProjectWell createdWellProjectWell;
        try
        {
            createdWellProjectWell = repository.CreateWellProjectWellDrillingSchedule(newWellProjectWell);
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update drilling schedule with id {drillingScheduleId}", wellProjectId);
            throw;
        }

        if (createdWellProjectWell.DrillingSchedule == null)
        {
            // TODO: use a more specific exception
            throw new Exception(nameof(createdWellProjectWell.DrillingSchedule));
        }

        var dto = mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(createdWellProjectWell.DrillingSchedule, wellProjectId);
        return dto;
    }
}
