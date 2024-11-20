using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TechnicalInputService(
    DcdDbContext context,
    IProjectService projectService,
    IExplorationOperationalWellCostsService explorationOperationalWellCostsService,
    IDevelopmentOperationalWellCostsService developmentOperationalWellCostsService,
    ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
    ILogger<TechnicalInputService> logger,
    IMapper mapper)
    : ITechnicalInputService
{
    public async Task<TechnicalInputDto> UpdateTehnicalInput(Guid projectId, UpdateTechnicalInputDto technicalInputDto)
    {
        var project = await projectService.GetProjectWithCasesAndAssets(projectId);

        await UpdateProject(project, technicalInputDto.ProjectDto);

        await UpdateExplorationOperationalWellCosts(project, technicalInputDto.ExplorationOperationalWellCostsDto);
        await UpdateDevelopmentOperationalWellCosts(project, technicalInputDto.DevelopmentOperationalWellCostsDto);

        var returnDto = new TechnicalInputDto();

        if (technicalInputDto.DeleteWellDtos?.Length > 0)
        {
            await DeleteWells(technicalInputDto.DeleteWellDtos);
        }

        if (technicalInputDto.UpdateWellDtos?.Length > 0 || technicalInputDto.CreateWellDtos?.Length > 0)
        {
            var wellResult = await CreateAndUpdateWells(projectId, technicalInputDto.CreateWellDtos, technicalInputDto.UpdateWellDtos);
            if (wellResult != null)
            {
                returnDto.ExplorationDto = wellResult.Value.explorationDto;
                returnDto.WellProjectDto = wellResult.Value.wellProjectDto;
            }
        }

        await context.SaveChangesAsync();

        var returnProject = await projectService.GetProjectWithCasesAndAssets(projectId);
        var returnProjectDto = mapper.Map<ProjectWithAssetsDto>(returnProject, opts => opts.Items["ConversionUnit"] = returnProject.PhysicalUnit.ToString());

        if (returnProjectDto == null)
        {
            logger.LogError("Failed to map project to dto");
            throw new Exception("Failed to map project to dto");
        }

        returnDto.ProjectDto = returnProjectDto;

        return returnDto;
    }

    private async Task DeleteWells(DeleteWellDto[] deleteWellDtos)
    {
        var affectedAssets = new Dictionary<string, List<Guid>>() {
            { nameof(Exploration), new List<Guid>() },
            { nameof(WellProject), new List<Guid>() }
        };

        foreach (var wellDto in deleteWellDtos)
        {
            var well = await context.Wells.FindAsync(wellDto.Id);
            if (well != null)
            {
                var explorationWells = context.ExplorationWell.Where(ew => ew.WellId == well.Id);
                foreach (var explorationWell in explorationWells)
                {
                    context.ExplorationWell.Remove(explorationWell);
                    affectedAssets[nameof(Exploration)].Add(explorationWell.ExplorationId);
                }
                var wellProjectWells = context.WellProjectWell.Where(ew => ew.WellId == well.Id);
                foreach (var wellProjectWell in wellProjectWells)
                {
                    context.WellProjectWell.Remove(wellProjectWell);
                    affectedAssets[nameof(WellProject)].Add(wellProjectWell.WellProjectId);
                }
                context.Wells.Remove(well);
            }
        }
        await context.SaveChangesAsync();
        foreach (var explorationId in affectedAssets[nameof(Exploration)])
        {
            await costProfileFromDrillingScheduleHelper.UpdateExplorationCostProfiles(explorationId);
        }
        foreach (var wellProjectId in affectedAssets[nameof(WellProject)])
        {
            await costProfileFromDrillingScheduleHelper.UpdateWellProjectCostProfiles(wellProjectId);
        }
    }

    private async Task<(ExplorationWithProfilesDto explorationDto, WellProjectWithProfilesDto wellProjectDto)?> CreateAndUpdateWells(
            Guid projectId,
            CreateWellDto[]? createWellDtos,
            UpdateWellDto[]? updateWellDtos
            )
    {
        var updatedWells = new List<Guid>();

        if (createWellDtos != null)
        {
            foreach (var wellDto in createWellDtos)
            {
                var well = mapper.Map<Well>(wellDto);
                if (well == null)
                {
                    throw new ArgumentNullException(nameof(well));
                }
                well.ProjectId = projectId;
                context.Wells.Add(well);
            }
        }

        if (updateWellDtos != null)
        {
            foreach (var wellDto in updateWellDtos)
            {
                var existing = await GetWell(wellDto.Id);
                if (wellDto.WellCost != existing.WellCost || wellDto.WellCategory != existing.WellCategory)
                {
                    updatedWells.Add(wellDto.Id);
                }
                mapper.Map(wellDto, existing);
                context.Wells.Update(existing);
            }
        }

        if (createWellDtos?.Any() == true || updateWellDtos?.Any() == true)
        {
            await context.SaveChangesAsync();
        }
        if (updatedWells.Count != 0)
        {
            await costProfileFromDrillingScheduleHelper.UpdateCostProfilesForWells(updatedWells);
        }
        return null;
    }

    private async Task<Well> GetWell(Guid wellId)
    {
        var well = await context.Wells
            .Include(e => e.WellProjectWells)
            .Include(e => e.ExplorationWells)
            .FirstOrDefaultAsync(w => w.Id == wellId);
        if (well == null)
        {
            throw new ArgumentException($"Well {wellId} not found.");
        }
        return well;
    }
    private async Task<ProjectWithAssetsDto> UpdateProject(Project project, UpdateProjectDto updatedDto)
    {
        mapper.Map(updatedDto, project);
        project.ModifyTime = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync();

        var projectDto = mapper.Map<ProjectWithAssetsDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());
        if (projectDto == null)
        {
            logger.LogError("Failed to map project to dto");
            throw new Exception("Failed to map project to dto");
        }

        return projectDto;
    }

    private async Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts(Project project, UpdateExplorationOperationalWellCostsDto updatedDto)
    {
        if (project.ExplorationOperationalWellCosts == null)
        {
            logger.LogError("Exploration operational well costs not found");
            throw new Exception("Exploration operational well costs not found");
        }
        var item = await explorationOperationalWellCostsService.GetOperationalWellCosts(project.ExplorationOperationalWellCosts.Id) ?? new ExplorationOperationalWellCosts();
        mapper.Map(updatedDto, item);
        var updatedItem = context.ExplorationOperationalWellCosts.Update(item);
        await context.SaveChangesAsync();
        var explorationOperationalWellCostsDto = mapper.Map<ExplorationOperationalWellCostsDto>(updatedItem.Entity);
        if (explorationOperationalWellCostsDto == null)
        {
            logger.LogError("Failed to map exploration operational well costs to dto");
            throw new Exception("Failed to map exploration operational well costs to dto");
        }
        return explorationOperationalWellCostsDto;
    }

    private async Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts(Project project, UpdateDevelopmentOperationalWellCostsDto updatedDto)
    {
        if (project.DevelopmentOperationalWellCosts == null)
        {
            logger.LogError("Development operational well costs not found");
            throw new Exception("Development operational well costs not found");
        }
        var item = await developmentOperationalWellCostsService.GetOperationalWellCosts(project.DevelopmentOperationalWellCosts.Id) ?? new DevelopmentOperationalWellCosts();
        mapper.Map(updatedDto, item);
        var updatedItem = context.DevelopmentOperationalWellCosts.Update(item);
        await context.SaveChangesAsync();
        var developmentOperationalWellCostsDto = mapper.Map<DevelopmentOperationalWellCostsDto>(updatedItem.Entity);
        if (developmentOperationalWellCostsDto == null)
        {
            logger.LogError("Failed to map development operational well costs to dto");
            throw new Exception("Failed to map development operational well costs to dto");
        }
        return developmentOperationalWellCostsDto;
    }
}
