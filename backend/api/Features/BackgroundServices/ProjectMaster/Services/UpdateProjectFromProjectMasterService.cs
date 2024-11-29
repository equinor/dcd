using api.Context;
using api.Features.BackgroundServices.ProjectMaster.Dtos;
using api.Features.BackgroundServices.ProjectMaster.Services.EnumConverters;
using api.Features.FusionIntegration.ProjectMaster;
using api.Features.Revisions.Create;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.BackgroundServices.ProjectMaster.Services;

public class UpdateProjectFromProjectMasterService(IDbContextFactory<DcdDbContext> contextFactory, IFusionService fusionService)
{
    public async Task UpdateProjectFromProjectMaster()
    {
        var projectsInDatabase = await ProjectsInDatabase();

        foreach (var projectInDatabase in projectsInDatabase)
        {
            var projectMasterDto = await GetProjectMasterDataForProject(projectInDatabase.Key);

            if (projectMasterDto == null)
            {
                continue;
            }

            var project = await GetProject(projectInDatabase.Key);

            if (!ProjectHasChanged(project, projectMasterDto))
            {
                continue;
            }

            await CreateRevisionIfProjectPhaseHasChanged(projectMasterDto);
            await UpdateProject(projectMasterDto);
        }
    }

    private async Task<Dictionary<Guid, string>> ProjectsInDatabase()
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.Projects
            .Where(x => x.OriginalProjectId == null)
            .ToDictionaryAsync(x => x.Id, x => x.Name);
    }

    private async Task<Project> GetProject(Guid projectId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.Projects.SingleAsync(x => x.Id == projectId);
    }

    private static bool ProjectHasChanged(Project project, UpdatableFieldsFromProjectMasterDto projectMasterDtoDto)
    {
        return project.Name != projectMasterDtoDto.Name ||
               project.CommonLibraryName != projectMasterDtoDto.CommonLibraryName ||
               project.FusionProjectId != projectMasterDtoDto.FusionProjectId ||
               project.Country != projectMasterDtoDto.Country ||
               project.ProjectCategory != projectMasterDtoDto.ProjectCategory ||
               project.ProjectPhase != projectMasterDtoDto.ProjectPhase;
    }

    private async Task CreateRevisionIfProjectPhaseHasChanged(UpdatableFieldsFromProjectMasterDto projectDto)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var project = await context.Projects.SingleAsync(x => x.Id == projectDto.Id);

        if (projectDto.ProjectPhase == project.ProjectPhase)
        {
            return;
        }

        var createRevisionService = new CreateRevisionService(new CreateRevisionRepository(context), context);

        await createRevisionService.CreateRevision(project.Id, new CreateRevisionDto
        {
            Classification = project.Classification,
            InternalProjectPhase = project.InternalProjectPhase,
            Name = $"{projectDto.Name} (project phase changed to {project.ProjectPhase.ToString()})",
            Arena = false,
            Mdqc = false
        });

        await context.SaveChangesAsync();
    }

    private async Task UpdateProject(UpdatableFieldsFromProjectMasterDto projectMasterDto)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var project = await context.Projects.SingleAsync(x => x.Id == projectMasterDto.Id);

        project.Name = projectMasterDto.Name;
        project.CommonLibraryName = projectMasterDto.CommonLibraryName;
        project.FusionProjectId = projectMasterDto.FusionProjectId;
        project.Country = projectMasterDto.Country;
        project.ProjectCategory = projectMasterDto.ProjectCategory;
        project.ProjectPhase = projectMasterDto.ProjectPhase;

        await context.SaveChangesAsync();
    }

    private async Task<UpdatableFieldsFromProjectMasterDto?> GetProjectMasterDataForProject(Guid projectId)
    {
        var projectMaster = await fusionService.GetProjectMasterFromFusionContextId(projectId);

        if (projectMaster == null)
        {
            return null;
        }

        return new UpdatableFieldsFromProjectMasterDto
        {
            Id = projectMaster.Identity,
            FusionProjectId = projectMaster.Identity,
            Name = projectMaster.Description ?? "",
            CommonLibraryName = projectMaster.Description ?? "",
            Country = projectMaster.Country ?? "",
            ProjectCategory = ProjectCategoryEnumConverter.ConvertCategory(projectMaster.ProjectCategory),
            ProjectPhase = ProjectPhaseEnumConverter.ConvertPhase(projectMaster.Phase)
        };
    }
}
