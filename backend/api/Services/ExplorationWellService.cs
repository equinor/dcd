using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationWellService : IExplorationWellService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ICostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly IExplorationService _explorationService;
    private readonly ILogger<ExplorationWellService> _logger;

    public ExplorationWellService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory,
        ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper, IExplorationService explorationService)
    {
        _context = context;
        _projectService = projectService;
        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;
        _explorationService = explorationService;
        _logger = loggerFactory.CreateLogger<ExplorationWellService>();
    }

    public async Task<ProjectDto> CreateExplorationWell(ExplorationWellDto explorationWellDto)
    {
        var explorationWell = ExplorationWellAdapter.Convert(explorationWellDto);
        _context.ExplorationWell!.Add(explorationWell);
        await _context.SaveChangesAsync();
        var projectId = _context.Explorations!.FirstOrDefault(c => c.Id == explorationWellDto.ExplorationId)?.ProjectId;
        if (projectId != null)
        {
            return await _projectService.GetProjectDtoAsync((Guid)projectId);
        }
        throw new NotFoundInDBException();
    }

    public async Task<ProjectDto> UpdateExplorationWell(ExplorationWellDto updatedExplorationWellDto)
    {
        var existingExplorationWell = await GetExplorationWell(updatedExplorationWellDto.WellId, updatedExplorationWellDto.ExplorationId);
        ExplorationWellAdapter.ConvertExisting(existingExplorationWell, updatedExplorationWellDto);
        if (updatedExplorationWellDto.DrillingSchedule == null && existingExplorationWell.DrillingSchedule != null)
        {
            _context.DrillingSchedule!.Remove(existingExplorationWell.DrillingSchedule);
        }

        _context.ExplorationWell!.Update(existingExplorationWell);
        await _context.SaveChangesAsync();
        var projectId = _context.Explorations!.FirstOrDefault(c => c.Id == updatedExplorationWellDto.ExplorationId)?.ProjectId;
        if (projectId != null)
        {
            return await _projectService.GetProjectDtoAsync((Guid)projectId);
        }
        throw new NotFoundInDBException();
    }

    public async Task<ExplorationWellDto[]?> UpdateMultpleExplorationWells(ExplorationWellDto[] updatedExplorationWellDtos, Guid caseId)
    {
        var explorationId = updatedExplorationWellDtos.FirstOrDefault()?.ExplorationId;
        ProjectDto? projectDto = null;
        foreach (var explorationWellDto in updatedExplorationWellDtos)
        {
            projectDto = await UpdateExplorationWell(explorationWellDto);
        }

        var explorationDto = await _costProfileFromDrillingScheduleHelper.UpdateExplorationCostProfilesForCase(caseId);

        await _explorationService.NewUpdateExploration(explorationDto);

        if (projectDto != null && explorationId != null)
        {
            return projectDto.Explorations?.FirstOrDefault(e => e.Id == explorationId)?.ExplorationWells?.ToArray();
        }
        return null;
    }

    public async Task<ExplorationWellDto[]?> CreateMultipleExplorationWells(ExplorationWellDto[] explorationWellDtos)
    {
        var explorationId = explorationWellDtos.FirstOrDefault()?.ExplorationId;
        ProjectDto? projectDto = null;
        foreach (var explorationWellDto in explorationWellDtos)
        {
            projectDto = await CreateExplorationWell(explorationWellDto);
        }
        if (projectDto != null && explorationId != null)
        {
            return projectDto.Explorations?.FirstOrDefault(e => e.Id == explorationId)?.ExplorationWells?.ToArray();
        }
        return null;
    }

    public async Task<ExplorationWell> GetExplorationWell(Guid wellId, Guid caseId)
    {
        var explorationWell = await _context.ExplorationWell!
            .Include(wpw => wpw.DrillingSchedule)
            .FirstOrDefaultAsync(w => w.WellId == wellId && w.ExplorationId == caseId);
        if (explorationWell == null)
        {
            throw new ArgumentException(string.Format("ExplorationWell {0} not found.", wellId));
        }
        return explorationWell;
    }

    public async Task<ExplorationWellDto[]?> CopyExplorationWell(Guid sourceExplorationId, Guid targetExplorationId)
    {
        var sourceExplorationWells = (await GetAll()).Where(ew => ew.ExplorationId == sourceExplorationId).ToList();
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
            var result = await CreateMultipleExplorationWells(newExplorationWellDtos.ToArray());
            return result;
        }
        return null;
    }

    public async Task<ExplorationWellDto> GetExplorationWellDto(Guid wellId, Guid caseId)
    {
        var explorationWell = await GetExplorationWell(wellId, caseId);
        var explorationWellDto = ExplorationWellDtoAdapter.Convert(explorationWell);

        return explorationWellDto;
    }

    public async Task<IEnumerable<ExplorationWell>> GetAll()
    {
        if (_context.ExplorationWell != null)
        {
            return await _context.ExplorationWell.Include(ew => ew.DrillingSchedule).ToListAsync();
        }
        else
        {
            _logger.LogInformation("No ExplorationWells existing");
            return new List<ExplorationWell>();
        }
    }

    public async Task<IEnumerable<ExplorationWellDto>> GetAllDtos()
    {
        var explorationWells = await GetAll();
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
