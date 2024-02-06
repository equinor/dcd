using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellProjectWellService : IWellProjectWellService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ICostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly IWellProjectService _wellProjectService;
    private readonly ILogger<WellProjectWellService> _logger;

    public WellProjectWellService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory,
        ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper, IWellProjectService wellProjectService)
    {
        _context = context;
        _projectService = projectService;
        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;
        _wellProjectService = wellProjectService;
        _logger = loggerFactory.CreateLogger<WellProjectWellService>();
    }

    public async Task<ProjectDto> CreateWellProjectWell(WellProjectWellDto wellProjectWellDto)
    {
        var wellProjectWell = WellProjectWellAdapter.Convert(wellProjectWellDto);
        _context.WellProjectWell!.Add(wellProjectWell);
        await _context.SaveChangesAsync();
        var projectId = (await _context.WellProjects!.FirstOrDefaultAsync(c => c.Id == wellProjectWellDto.WellProjectId))?.ProjectId;
        if (projectId != null)
        {
            return await _projectService.GetProjectDto((Guid)projectId);
        }
        throw new NotFoundInDBException();
    }

    public async Task<WellProjectWellDto[]?> CreateMultipleWellProjectWells(WellProjectWellDto[] wellProjectWellDtos)
    {
        var wellProjectId = wellProjectWellDtos.FirstOrDefault()?.WellProjectId;
        ProjectDto? projectDto = null;
        foreach (var wellProjectWellDto in wellProjectWellDtos)
        {
            projectDto = await CreateWellProjectWell(wellProjectWellDto);
        }
        if (projectDto != null && wellProjectId != null)
        {
            return projectDto.WellProjects?.FirstOrDefault(e => e.Id == wellProjectId)?.WellProjectWells?.ToArray();
        }
        return null;
    }

    public async Task<WellProjectWell> GetWellProjectWell(Guid wellId, Guid caseId)
    {
        var wellProjectWell = await _context.WellProjectWell!
            .Include(wpw => wpw.DrillingSchedule)
            .FirstOrDefaultAsync(w => w.WellId == wellId && w.WellProjectId == caseId);
        if (wellProjectWell == null)
        {
            throw new ArgumentException(string.Format("WellProjectWell {0} not found.", wellId));
        }
        return wellProjectWell;
    }

    public async Task<WellProjectWellDto[]?> CopyWellProjectWell(Guid sourceWellProjectId, Guid targetWellProjectId)
    {
        var sourceWellProjectWells = (await GetAll()).Where(wpw => wpw.WellProjectId == sourceWellProjectId).ToList();
        if (sourceWellProjectWells?.Count > 0)
        {
            var newWellProjectWellDtos = new List<WellProjectWellDto>();
            foreach (var wellProjectWell in sourceWellProjectWells)
            {
                var newWellProjectWellDto = WellProjectWellDtoAdapter.Convert(wellProjectWell);
                if (newWellProjectWellDto.DrillingSchedule != null)
                {
                    newWellProjectWellDto.DrillingSchedule.Id = Guid.Empty;
                }
                newWellProjectWellDto.WellProjectId = targetWellProjectId;
                newWellProjectWellDtos.Add(newWellProjectWellDto);
            }
            var result = await CreateMultipleWellProjectWells(newWellProjectWellDtos.ToArray());
            return result;
        }
        return null;
    }

    public async Task<IEnumerable<WellProjectWell>> GetAll()
    {
        if (_context.WellProjectWell != null)
        {
            return await _context.WellProjectWell.Include(wpw => wpw.DrillingSchedule).ToListAsync();
        }
        else
        {
            _logger.LogInformation("No WellProjectWells existing");
            return new List<WellProjectWell>();
        }
    }
}
