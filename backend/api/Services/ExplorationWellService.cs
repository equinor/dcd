using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationWellService : IExplorationWellService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ICostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly IExplorationService _explorationService;
    private readonly ILogger<ExplorationWellService> _logger;
    private readonly IMapper _mapper;

    public ExplorationWellService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
        IExplorationService explorationService,
        IMapper mapper)
    {
        _context = context;
        _projectService = projectService;
        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;
        _explorationService = explorationService;
        _logger = loggerFactory.CreateLogger<ExplorationWellService>();
        _mapper = mapper;
    }

    public async Task<ProjectDto> CreateExplorationWell(CreateExplorationWellDto explorationWellDto)
    {
        var explorationWell = _mapper.Map<ExplorationWell>(explorationWellDto);
        if (explorationWell == null)
        {
            throw new ArgumentNullException(nameof(explorationWell));
        }
        _context.ExplorationWell!.Add(explorationWell);
        await _context.SaveChangesAsync();
        var projectId = _context.Explorations!.FirstOrDefault(c => c.Id == explorationWellDto.ExplorationId)?.ProjectId;
        if (projectId != null)
        {
            return await _projectService.GetProjectDto((Guid)projectId);
        }
        throw new NotFoundInDBException();
    }

    public async Task<ExplorationWellDto[]?> CreateMultipleExplorationWells(CreateExplorationWellDto[] explorationWellDtos)
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

        public async Task<List<ExplorationWell>> GetExplorationWellsForExploration(Guid explorationId)
    {
        var explorationWells = await _context.ExplorationWell!
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.ExplorationId == explorationId).ToListAsync();
        if (explorationWells == null)
        {
            throw new ArgumentException(string.Format("ExplorationWell for Exploration {0} not found.", explorationId));
        }
        return explorationWells;
    }

    public async Task<ExplorationWellDto[]?> CopyExplorationWell(Guid sourceExplorationId, Guid targetExplorationId)
    {
        var sourceExplorationWells = (await GetAll()).Where(ew => ew.ExplorationId == sourceExplorationId).ToList();
        if (sourceExplorationWells?.Count > 0)
        {
            var newExplorationWellDtos = new List<CreateExplorationWellDto>();
            foreach (var explorationWell in sourceExplorationWells)
            {
                var newExplorationDto = _mapper.Map<CreateExplorationWellDto>(explorationWell);
                if (newExplorationDto == null)
                {
                    throw new ArgumentNullException(nameof(newExplorationDto));
                }
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

}
