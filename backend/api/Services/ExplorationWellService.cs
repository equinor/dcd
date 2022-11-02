using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationWellService
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CaseService> _logger;

    public ExplorationWellService(DcdDbContext context, ProjectService projectService, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _serviceProvider = serviceProvider;
        _logger = loggerFactory.CreateLogger<CaseService>();
    }

    public ProjectDto CreateExplorationWell(ExplorationWellDto explorationWellDto)
    {
        var explorationWell = ExplorationWellAdapter.Convert(explorationWellDto);
        _context.ExplorationWell!.Add(explorationWell);
        _context.SaveChanges();
        var projectId = _context.Explorations!.FirstOrDefault(c => c.Id == explorationWellDto.ExplorationId)?.ProjectId;
        if (projectId != null)
        {
            return _projectService.GetProjectDto((Guid)projectId);
        }
        throw new NotFoundInDBException();
    }

    public ProjectDto UpdateExplorationWell(ExplorationWellDto updatedExplorationWellDto)
    {
        var existingExplorationWell = GetExplorationWell(updatedExplorationWellDto.WellId, updatedExplorationWellDto.ExplorationId);
        ExplorationWellAdapter.ConvertExisting(existingExplorationWell, updatedExplorationWellDto);
        if (updatedExplorationWellDto.DrillingSchedule == null && existingExplorationWell.DrillingSchedule != null)
        {
            _context.DrillingSchedule!.Remove(existingExplorationWell.DrillingSchedule);
        }

        _context.ExplorationWell!.Update(existingExplorationWell);
        _context.SaveChanges();
        var projectId = _context.Explorations!.FirstOrDefault(c => c.Id == updatedExplorationWellDto.ExplorationId)?.ProjectId;
        if (projectId != null)
        {
            return _projectService.GetProjectDto((Guid)projectId);
        }
        throw new NotFoundInDBException();
    }

    public ExplorationWellDto[]? UpdateMultpleExplorationWells(ExplorationWellDto[] updatedExplorationWellDtos, Guid caseId)
    {
        var explorationId = updatedExplorationWellDtos.FirstOrDefault()?.ExplorationId;
        ProjectDto? projectDto = null;
        foreach (var explorationWellDto in updatedExplorationWellDtos)
        {
            projectDto = UpdateExplorationWell(explorationWellDto);
        }

        var costProfileHelper = _serviceProvider.GetRequiredService<CostProfileFromDrillingScheduleHelper>();
        costProfileHelper.UpdateExplorationCostProfilesForCase(caseId);

        if (projectDto != null && explorationId != null)
        {
            return projectDto.Explorations?.FirstOrDefault(e => e.Id == explorationId)?.ExplorationWells?.ToArray();
        }
        return null;
    }

    public ExplorationWellDto[]? CreateMultpleExplorationWells(ExplorationWellDto[] explorationWellDtos)
    {
        var explorationId = explorationWellDtos.FirstOrDefault()?.ExplorationId;
        ProjectDto? projectDto = null;
        foreach (var explorationWellDto in explorationWellDtos)
        {
            projectDto = CreateExplorationWell(explorationWellDto);
        }
        if (projectDto != null && explorationId != null)
        {
            return projectDto.Explorations?.FirstOrDefault(e => e.Id == explorationId)?.ExplorationWells?.ToArray();
        }
        return null;
    }

    public ExplorationWell GetExplorationWell(Guid wellId, Guid caseId)
    {
        var explorationWell = _context.ExplorationWell!
            .Include(wpw => wpw.DrillingSchedule)
            .FirstOrDefault(w => w.WellId == wellId && w.ExplorationId == caseId);
        if (explorationWell == null)
        {
            throw new ArgumentException(string.Format("ExplorationWell {0} not found.", wellId));
        }
        return explorationWell;
    }

    public ExplorationWellDto GetExplorationWellDto(Guid wellId, Guid caseId)
    {
        var explorationWell = GetExplorationWell(wellId, caseId);
        var explorationWellDto = ExplorationWellDtoAdapter.Convert(explorationWell);

        return explorationWellDto;
    }

    public IEnumerable<ExplorationWell> GetAll()
    {
        if (_context.ExplorationWell != null)
        {
            return _context.ExplorationWell.Include(wpw => wpw.DrillingSchedule);
        }
        else
        {
            _logger.LogInformation("No ExplorationWells existing");
            return new List<ExplorationWell>();
        }
    }

    public IEnumerable<ExplorationWellDto> GetAllDtos()
    {
        var explorationWells = GetAll();
        if (explorationWells.Any())
        {
            var explorationWellDtos = new List<ExplorationWellDto>();
            foreach (ExplorationWell explorationWell in explorationWells)
            {
                var explorationWellDto = ExplorationWellDtoAdapter.Convert(explorationWell);
                explorationWellDtos.Add(explorationWellDto);
            }

            return explorationWellDtos;
        }
        else
        {
            return new List<ExplorationWellDto>();
        }
    }
}
