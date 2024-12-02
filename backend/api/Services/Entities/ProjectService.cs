using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ProjectService(
    ILogger<ProjectService> logger,
    IProjectRepository projectRepository,
    IMapperService mapperService)
    : IProjectService
{
    public async Task<ProjectWithCasesDto> UpdateProject(Guid projectId, UpdateProjectDto projectDto)
    {
        var existingProject = await projectRepository.GetProjectWithCases(projectId)
                              ?? throw new NotFoundInDBException($"Project {projectId} not found");

        existingProject.ModifyTime = DateTimeOffset.UtcNow;

        mapperService.MapToEntity(projectDto, existingProject, projectId);

        try
        {
            await projectRepository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Failed to update project {projectId}", projectId);
            throw;
        }

        var dto = mapperService.MapToDto<Project, ProjectWithCasesDto>(existingProject, projectId);
        return dto;
    }

    public async Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts(
        Guid projectId,
        Guid explorationOperationalWellCostsId,
        UpdateExplorationOperationalWellCostsDto dto
    )
    {
        var existingExplorationOperationalWellCosts = await projectRepository.GetExplorationOperationalWellCosts(explorationOperationalWellCostsId)
                                                      ?? throw new NotFoundInDBException($"ExplorationOperationalWellCosts {explorationOperationalWellCostsId} not found");

        mapperService.MapToEntity(dto, existingExplorationOperationalWellCosts, explorationOperationalWellCostsId);

        try
        {
            await projectRepository.UpdateModifyTime(projectId);
            await projectRepository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Failed to update exploration operational well costs {explorationOperationalWellCostsId}", explorationOperationalWellCostsId);
            throw;
        }

        var returnDto = mapperService.MapToDto<ExplorationOperationalWellCosts, ExplorationOperationalWellCostsDto>(existingExplorationOperationalWellCosts, explorationOperationalWellCostsId);
        return returnDto;
    }

    public async Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts(
        Guid projectId,
        Guid developmentOperationalWellCostsId,
        UpdateDevelopmentOperationalWellCostsDto dto
    )
    {
        var existingDevelopmentOperationalWellCosts = await projectRepository.GetDevelopmentOperationalWellCosts(developmentOperationalWellCostsId)
                                                      ?? throw new NotFoundInDBException($"DevelopmentOperationalWellCosts {developmentOperationalWellCostsId} not found");

        mapperService.MapToEntity(dto, existingDevelopmentOperationalWellCosts, developmentOperationalWellCostsId);

        try
        {
            await projectRepository.UpdateModifyTime(projectId);
            await projectRepository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Failed to update development operational well costs {developmentOperationalWellCostsId}", developmentOperationalWellCostsId);
            throw;
        }

        var returnDto = mapperService.MapToDto<DevelopmentOperationalWellCosts, DevelopmentOperationalWellCostsDto>(existingDevelopmentOperationalWellCosts, developmentOperationalWellCostsId);
        return returnDto;
    }

    public async Task<Project> GetProject(Guid projectId)
    {
        return await projectRepository.GetProject(projectId)
               ?? throw new NotFoundInDBException($"Project {projectId} not found");
    }
}
