using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationWellService
{
    private readonly DcdDbContext _context;
    private readonly ExplorationService _explorationService;
    private readonly ILogger<CaseService> _logger;
    private readonly ProjectService _projectService;

    public ExplorationWellService(DcdDbContext context, ProjectService projectService,
        ExplorationService explorationService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<CaseService>();
        _explorationService = explorationService;
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
        var existing = GetExplorationWell(updatedExplorationWellDto.WellId, updatedExplorationWellDto.ExplorationId);
        ExplorationWellAdapter.ConvertExisting(existing, updatedExplorationWellDto);
        if (updatedExplorationWellDto.DrillingSchedule == null && existing.DrillingSchedule != null)
        {
            _context.DrillingSchedule!.Remove(existing.DrillingSchedule);
        }

        var exploration = _context.Explorations!.Include(wp => wp.CostProfile).Include(wp => wp.ExplorationWells)
            .ThenInclude(wpw => wpw.DrillingSchedule).FirstOrDefault(wp => wp.Id == existing.ExplorationId);
        _explorationService.CalculateCostProfile(exploration, existing, null);

        _context.ExplorationWell!.Update(existing);
        _context.SaveChanges();
        var projectId = _context.Explorations!.FirstOrDefault(c => c.Id == updatedExplorationWellDto.ExplorationId)
            ?.ProjectId;
        if (projectId != null)
        {
            return _projectService.GetProjectDto((Guid)projectId);
        }

        throw new NotFoundInDBException();
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
            return _context.ExplorationWell;
        }

        _logger.LogInformation("No ExplorationWells existing");
        return new List<ExplorationWell>();
    }

    public IEnumerable<ExplorationWellDto> GetAllDtos()
    {
        var explorationWells = GetAll();
        if (explorationWells.Any())
        {
            var explorationWellDtos = new List<ExplorationWellDto>();
            foreach (var explorationWell in explorationWells)
            {
                var explorationWellDto = ExplorationWellDtoAdapter.Convert(explorationWell);
                explorationWellDtos.Add(explorationWellDto);
            }

            return explorationWellDtos;
        }

        return new List<ExplorationWellDto>();
    }
}
