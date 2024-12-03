using System.Linq.Expressions;

using api.Exceptions;
using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Repositories;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.Models;
using api.Services;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Explorations.Services;

public class ExplorationService(
    ILogger<ExplorationService> logger,
    ICaseRepository caseRepository,
    IExplorationRepository repository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService)
    : IExplorationService
{
    public async Task<Exploration> GetExplorationWithIncludes(Guid explorationId, params Expression<Func<Exploration, object>>[] includes)
    {
        return await repository.GetExplorationWithIncludes(explorationId, includes)
            ?? throw new NotFoundInDBException($"Exploration with id {explorationId} not found.");
    }

    public async Task<ExplorationDto> UpdateExploration(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        UpdateExplorationDto updatedExplorationDto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Exploration>(projectId, explorationId);

        var existingExploration = await repository.GetExploration(explorationId)
            ?? throw new NotFoundInDBException($"Exploration with id {explorationId} not found.");

        mapperService.MapToEntity(updatedExplorationDto, existingExploration, explorationId);

        try
        {
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update exploration with id {explorationId} for case id {caseId}.", explorationId, caseId);
            throw;
        }

        var dto = mapperService.MapToDto<Exploration, ExplorationDto>(existingExploration, explorationId);
        return dto;
    }

    public async Task<DrillingScheduleDto> UpdateExplorationWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateDrillingScheduleDto updatedExplorationWellDto
    )
    {
        var existingExploration = await repository.GetExplorationWithDrillingSchedule(drillingScheduleId)
            ?? throw new NotFoundInDBException($"No exploration connected to {drillingScheduleId} found.");

        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Exploration>(projectId, existingExploration.Id);

        var existingDrillingSchedule = existingExploration.ExplorationWells?.FirstOrDefault(w => w.WellId == wellId)?.DrillingSchedule
            ?? throw new NotFoundInDBException($"Drilling schedule with id {drillingScheduleId} not found.");

        mapperService.MapToEntity(updatedExplorationWellDto, existingDrillingSchedule, drillingScheduleId);

        try
        {
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

    public async Task<DrillingScheduleDto> CreateExplorationWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        CreateDrillingScheduleDto updatedExplorationWellDto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Exploration>(projectId, explorationId);

        var existingExploration = await repository.GetExploration(explorationId)
            ?? throw new NotFoundInDBException($"Well project with {explorationId} not found.");

        var existingWell = await repository.GetWell(wellId)
            ?? throw new NotFoundInDBException($"Well with {wellId} not found.");

        DrillingSchedule drillingSchedule = new();
        var newDrillingSchedule = mapperService.MapToEntity(updatedExplorationWellDto, drillingSchedule, explorationId);

        ExplorationWell newExplorationWell = new()
        {
            Well = existingWell,
            Exploration = existingExploration,
            DrillingSchedule = newDrillingSchedule
        };

        ExplorationWell createdExplorationWell;
        try
        {
            createdExplorationWell = repository.CreateExplorationWellDrillingSchedule(newExplorationWell);
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update drilling schedule with id {drillingScheduleId}", explorationId);
            throw;
        }

        if (createdExplorationWell.DrillingSchedule == null)
        {
            // TODO: use a more specific exception
            throw new Exception(nameof(createdExplorationWell.DrillingSchedule));
        }

        var dto = mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(createdExplorationWell.DrillingSchedule, explorationId);
        return dto;
    }
}
