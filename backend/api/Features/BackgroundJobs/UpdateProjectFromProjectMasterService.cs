using System.Diagnostics;

using api.Context;
using api.Dtos;
using api.Features.FusionIntegration.ProjectMaster;
using api.Helpers;
using api.Models;
using api.Services;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace api.Features.BackgroundJobs;

public class UpdateProjectFromProjectMasterService(IMapper mapper,
    DcdDbContext context,
    IFusionService fusionService,
    IProjectService projectService,
ILogger<UpdateProjectFromProjectMasterService> logger)
{
    public async Task UpdateProjectFromProjectMaster()
    {
        var projectDtos = await GetAllDtos();
        var numberOfDeviations = 0;
        var totalNumberOfProjects = projectDtos.Count;

        foreach (var project in projectDtos)
        {
            var projectMaster = await GetProjectDtoFromProjectMaster(project.Id);

            if (projectMaster == null)
            {
                logger.LogWarning("ProjectMaster not found for project {projectName} ({projectId})", project.Name, project.Id);
                continue;
            }

            if (!project.Equals(projectMaster))
            {
                logger.LogWarning("Project {projectName} ({projectId}) differs from ProjectMaster", project.Name, project.Id);
                numberOfDeviations++;
                await UpdateProjectFromProjectMaster(projectMaster);
            }
            else
            {
                logger.LogInformation("Project {projectName} ({projectId}) is identical to ProjectMaster",
                    project.Name, project.Id);
            }
        }

        logger.LogInformation("Number of projects which differs from ProjectMaster: {count} / {total}",
            numberOfDeviations, totalNumberOfProjects);
    }

    private async Task UpdateProjectFromProjectMaster(ProjectWithAssetsDto projectDto)
    {
        var existingProject = await projectService.GetProjectWithCasesAndAssets(projectDto.Id);

        mapper.Map(projectDto, existingProject);

        context.Projects.Update(existingProject);
        await context.SaveChangesAsync();
        await projectService.GetProjectDto(existingProject.Id);
    }

    private async Task<List<ProjectWithAssetsDto>> GetAllDtos()
    {
        var projects = await GetAll();
        var projectDtos = new List<ProjectWithAssetsDto>();

        foreach (var project in projects)
        {
            var projectDto = mapper.Map<ProjectWithAssetsDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

            if (projectDto != null)
            {
                projectDtos.Add(projectDto);
            }
        }

        Activity.Current?.AddBaggage(nameof(projectDtos), JsonConvert.SerializeObject(projectDtos, Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));

        return projectDtos;
    }

    private async Task<IEnumerable<Project>> GetAll()
    {
        Activity.Current?.AddBaggage(nameof(context.Projects), JsonConvert.SerializeObject(context.Projects,
            Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));

        var projects = context.Projects.Include(c => c.Cases);

        foreach (var project in projects)
        {
            await projectService.AddAssetsToProject(project);
        }

        return projects;
    }

    private async Task<ProjectWithAssetsDto?> GetProjectDtoFromProjectMaster(Guid projectGuid)
    {

        var projectMaster = await fusionService.GetProjectMasterFromFusionContextId(projectGuid);

        if (projectMaster == null)
        {
            return null;
        }

        ProjectCategory category;
        ProjectPhase phase;

        try
        {
            category = CommonLibraryHelper.ConvertCategory(projectMaster.ProjectCategory ?? "");
            phase = CommonLibraryHelper.ConvertPhase(projectMaster.Phase ?? "");
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Invalid category or phase for project with ID {ProjectId}", projectGuid);
            return null;
        }

        return new ProjectWithAssetsDto
        {
            Name = projectMaster.Description ?? "",
            CommonLibraryName = projectMaster.Description ?? "",
            FusionProjectId = projectMaster.Identity,
            Country = projectMaster.Country ?? "",
            Currency = Currency.NOK,
            PhysicalUnit = PhysUnit.SI,
            Id = projectMaster.Identity,
            ProjectCategory = category,
            ProjectPhase = phase,
        };
    }
}
