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
    private readonly ExplorationService _explorationService;
    private readonly ILogger<CaseService> _logger;

    public ExplorationWellService(DcdDbContext context, ProjectService projectService, ExplorationService explorationService, ILoggerFactory loggerFactory)
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

        var exploration = _context.Explorations!.Include(wp => wp.CostProfile)
        .Include(wp => wp.CountryOfficeCost)
        .Include(wp => wp.SeismicAcquisitionAndProcessing)
        .Include(wp => wp.ExplorationWells!)
        .ThenInclude(wpw => wpw.DrillingSchedule).FirstOrDefault(wp => wp.Id == existing.ExplorationId);

        _explorationService.CalculateCostProfile(exploration, existing, null);

        _context.ExplorationWell!.Update(existing);
        _context.SaveChanges();
        var projectId = _context.Explorations!.FirstOrDefault(c => c.Id == updatedExplorationWellDto.ExplorationId)?.ProjectId;
        if (projectId != null)
        {
            return _projectService.GetProjectDto((Guid)projectId);
        }
        throw new NotFoundInDBException();
    }

    public ExplorationWellDto[]? UpdateMultpleExplorationWells(ExplorationWellDto[] updatedExplorationWellDtos)
    {
        var explorationId = updatedExplorationWellDtos.FirstOrDefault()?.ExplorationId;
        ProjectDto? projectDto = null;
        foreach (var explorationWellDto in updatedExplorationWellDtos)
        {
            projectDto = UpdateExplorationWell(explorationWellDto);
        }
        if (projectDto != null && explorationId != null)
        {
            return projectDto.Explorations?.FirstOrDefault(e => e.Id == explorationId)?.ExplorationWells?.ToArray();
        }
        return null;
    }

    public ExplorationWellDto[]? CreateMultipleExplorationWells(ExplorationWellDto[] explorationWellDtos)
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

    public ExplorationWellDto[]? CopyExplorationWell(Guid sourceExplorationId, Guid targetExplorationId)
    {
        var sourceExplorationWells = GetAll().Where(ew => ew.ExplorationId == sourceExplorationId).ToList();
        if (sourceExplorationWells?.Count > 0)
        {
            var newExplorationWellDtos = new List<ExplorationWellDto>();
            foreach (var explorationWell in sourceExplorationWells)
            {
                var newExplorationDto = ExplorationWellDtoAdapter.Convert(explorationWell);
                if (newExplorationDto.DrillingSchedule != null)
                {
                    newExplorationDto.DrillingSchedule.Id = Guid.Empty;
                }
                newExplorationDto.ExplorationId = targetExplorationId;
                newExplorationWellDtos.Add(newExplorationDto);
            }
            var result = CreateMultipleExplorationWells(newExplorationWellDtos.ToArray());
            return result;
        }
        return null;
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
            return _context.ExplorationWell.Include(ew => ew.DrillingSchedule);
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
