using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationService : IExplorationService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;

    private readonly ILogger<ExplorationService> _logger;

    public ExplorationService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<ExplorationService>();
    }


    public async Task<ExplorationDto> CopyExploration(Guid explorationId, Guid sourceCaseId)
    {
        var source = await GetExploration(explorationId);
        var newExplorationDto = ExplorationDtoAdapter.Convert(source);
        newExplorationDto.Id = Guid.Empty;

        if (newExplorationDto.ExplorationWellCostProfile != null)
        {
            newExplorationDto.ExplorationWellCostProfile.Id = Guid.Empty;
        }
        if (newExplorationDto.AppraisalWellCostProfile != null)
        {
            newExplorationDto.AppraisalWellCostProfile.Id = Guid.Empty;
        }
        if (newExplorationDto.SidetrackCostProfile != null)
        {
            newExplorationDto.SidetrackCostProfile.Id = Guid.Empty;
        }
        if (newExplorationDto.SeismicAcquisitionAndProcessing != null)
        {
            newExplorationDto.SeismicAcquisitionAndProcessing.Id = Guid.Empty;
        }
        if (newExplorationDto.CountryOfficeCost != null)
        {
            newExplorationDto.CountryOfficeCost.Id = Guid.Empty;
        }
        if (newExplorationDto.GAndGAdminCost != null)
        {
            newExplorationDto.GAndGAdminCost.Id = Guid.Empty;
        }

        var wellProject = await NewCreateExploration(newExplorationDto, sourceCaseId);
        var dto = ExplorationDtoAdapter.Convert(wellProject);
        return dto;
    }

    public async Task<ProjectDto> CreateExploration(ExplorationDto explorationDto, Guid sourceCaseId)
    {
        var exploration = ExplorationAdapter.Convert(explorationDto);
        var project = await _projectService.GetProjectAsync(exploration.ProjectId);
        exploration.Project = project;
        _context.Explorations!.Add(exploration);
        await _context.SaveChangesAsync();
        await SetCaseLink(exploration, sourceCaseId, project);
        return await _projectService.GetProjectDtoAsync(exploration.ProjectId);
    }

    public async Task<Exploration> NewCreateExploration(ExplorationDto explorationDto, Guid sourceCaseId)
    {
        var exploration = ExplorationAdapter.Convert(explorationDto);
        var project = await _projectService.GetProjectAsync(exploration.ProjectId);
        exploration.Project = project;
        var createdExploration = _context.Explorations!.Add(exploration);
        await _context.SaveChangesAsync();
        await SetCaseLink(exploration, sourceCaseId, project);
        return createdExploration.Entity;
    }

    private async Task SetCaseLink(Exploration exploration, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.ExplorationLink = exploration.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<ProjectDto> DeleteExploration(Guid explorationId)
    {
        var exploration = await GetExploration(explorationId);
        _context.Explorations!.Remove(exploration);
        DeleteCaseLinks(explorationId);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDtoAsync(exploration.ProjectId);
    }

    private void DeleteCaseLinks(Guid explorationId)
    {
        foreach (Case c in _context.Cases!)
        {
            if (c.ExplorationLink == explorationId)
            {
                c.ExplorationLink = Guid.Empty;
            }
        }
    }

    public async Task<ProjectDto> UpdateExploration(ExplorationDto updatedExplorationDto)
    {
        var existing = await GetExploration(updatedExplorationDto.Id);
        ExplorationAdapter.ConvertExisting(existing, updatedExplorationDto);

        _context.Explorations!.Update(existing);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDtoAsync(existing.ProjectId);
    }

    public async Task<ExplorationDto> NewUpdateExploration(ExplorationDto updatedExplorationDto)
    {
        var existing = await GetExploration(updatedExplorationDto.Id);
        ExplorationAdapter.ConvertExisting(existing, updatedExplorationDto);

        var updatedExploration = _context.Explorations!.Update(existing);
        await _context.SaveChangesAsync();
        return ExplorationDtoAdapter.Convert(updatedExploration.Entity);
    }

    public async Task<ExplorationDto[]> UpdateMultiple(ExplorationDto[] updatedExplorationDtos)
    {
        var updatedExplorationDtoList = new List<ExplorationDto>();
        foreach (var explorationDto in updatedExplorationDtos)
        {
            var updatedExplorationDto = await UpdateSingleExploration(explorationDto);
            updatedExplorationDtoList.Add(updatedExplorationDto);
        }

        await _context.SaveChangesAsync();
        return updatedExplorationDtoList.ToArray();
    }

    public async Task<ExplorationDto> UpdateSingleExploration(ExplorationDto updatedExplorationDto)
    {
        var existing = await GetExploration(updatedExplorationDto.Id);
        ExplorationAdapter.ConvertExisting(existing, updatedExplorationDto);
        var exploration = _context.Explorations!.Update(existing);
        await _context.SaveChangesAsync();
        return ExplorationDtoAdapter.Convert(exploration.Entity);
    }

    public async Task<Exploration> GetExploration(Guid explorationId)
    {
        var exploration = await _context.Explorations!
            .Include(c => c.ExplorationWellCostProfile)
            .Include(c => c.AppraisalWellCostProfile)
            .Include(c => c.SidetrackCostProfile)
            .Include(c => c.GAndGAdminCost)
            .Include(c => c.SeismicAcquisitionAndProcessing)
            .Include(c => c.CountryOfficeCost)
            .Include(c => c.ExplorationWells!).ThenInclude(ew => ew.DrillingSchedule)
            .Include(ew => ew.ExplorationWells!).ThenInclude(ew => ew.Well)
            .FirstOrDefaultAsync(o => o.Id == explorationId);
        if (exploration == null)
        {
            throw new ArgumentException(string.Format("Exploration {0} not found.", explorationId));
        }
        return exploration;
    }
}
