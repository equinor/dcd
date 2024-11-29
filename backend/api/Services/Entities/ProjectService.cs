using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Features.FusionIntegration.ProjectMaster;
using api.Features.Revisions.Get;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ProjectService(
    DcdDbContext context,
    ILogger<ProjectService> logger,
    IMapper mapper,
    IProjectRepository projectRepository,
    IMapperService mapperService,
    IFusionService fusionService,
    IProjectWithAssetsRepository projectWithAssetsRepository)
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

    public async Task<Guid> CreateProject(Guid contextId)
    {
        var projectMaster = await fusionService.GetProjectMasterFromFusionContextId(contextId);

        if (projectMaster == null)
        {
            throw new KeyNotFoundException($"Project with context ID {contextId} not found in the external API.");
        }

        // Check if a project with the same external ID already exists
        var existingProject = await projectRepository.GetProjectByExternalId(projectMaster.Identity);

        if (existingProject != null)
        {
            throw new ProjectAlreadyExistsException($"Project with externalId {projectMaster.Identity} already exists");
        }

        var project = new Project();

        mapperService.MapToEntity(projectMaster, project, Guid.Empty);

        project.CreateDate = DateTimeOffset.UtcNow;
        project.Cases = new List<Case>();
        project.DrainageStrategies = new List<DrainageStrategy>();
        project.Substructures = new List<Substructure>();
        project.Surfs = new List<Surf>();
        project.Topsides = new List<Topside>();
        project.Transports = new List<Transport>();
        project.WellProjects = new List<WellProject>();
        project.Explorations = new List<Exploration>();

        project.ExplorationOperationalWellCosts = new ExplorationOperationalWellCosts();
        project.DevelopmentOperationalWellCosts = new DevelopmentOperationalWellCosts();

        project.CO2EmissionFromFuelGas = 2.34;
        project.FlaredGasPerProducedVolume = 1.13;
        project.CO2EmissionsFromFlaredGas = 3.74;
        project.CO2Vented = 1.96;
        project.DailyEmissionFromDrillingRig = 100;
        project.AverageDevelopmentDrillingDays = 50;

        context.Projects.Add(project);

        await context.SaveChangesAsync();

        return project.Id;
    }

    public async Task<Project> GetProject(Guid projectId)
    {
        return await projectRepository.GetProject(projectId)
               ?? throw new NotFoundInDBException($"Project {projectId} not found");
    }

    public async Task<ProjectWithAssetsDto> GetProjectDto(Guid projectId)
    {
        var project = await projectWithAssetsRepository.GetProjectWithCasesAndAssets(projectId);

        var projectDto = mapper.Map<Project, ProjectWithAssetsDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

        var revisionDetails = context.RevisionDetails.Where(r => r.OriginalProjectId == project.Id).ToList();
        projectDto.RevisionsDetailsList = mapper.Map<List<RevisionDetailsDto>>(revisionDetails);

        projectDto.ModifyTime = project.Cases.Select(c => c.ModifyTime).Append(project.ModifyTime).Max();

        return projectDto;
    }
}
