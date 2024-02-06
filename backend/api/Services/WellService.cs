using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellService : IWellService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ICostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly ILogger<WellService> _logger;

    public WellService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory, ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper)
    {
        _context = context;
        _projectService = projectService;
        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;
        _logger = loggerFactory.CreateLogger<WellService>();
    }

    public async Task<ProjectDto> CreateWell(WellDto wellDto)
    {
        var _well = WellAdapter.Convert(wellDto);
        _context.Wells!.Add(_well);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(wellDto.ProjectId);
    }

    public async Task<ProjectDto> DeleteWell(Guid wellId)
    {
        var wellItem = await GetWell(wellId);
        _context.Wells!.Remove(wellItem);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(wellItem.ProjectId);
    }

    public async Task<WellDto> UpdateExistingWell(WellDto updatedWellDto)
    {
        var existing = await GetWell(updatedWellDto.Id);
        WellAdapter.ConvertExisting(existing, updatedWellDto);

        var well = _context.Wells!.Update(existing);
        await _context.SaveChangesAsync();
        return WellDtoAdapter.Convert(well.Entity);
    }

    public async Task<Well> GetWell(Guid wellId)
    {
        var well = await _context.Wells!
            .Include(e => e.WellProjectWells)
            .Include(e => e.ExplorationWells)
            .FirstOrDefaultAsync(w => w.Id == wellId);
        if (well == null)
        {
            throw new ArgumentException(string.Format("Well {0} not found.", wellId));
        }
        return well;
    }

    public async Task<IEnumerable<Well>> GetAll()
    {
        if (_context.Wells != null)
        {
            return await _context.Wells.ToListAsync();
        }
        else
        {
            _logger.LogInformation("No Wells existing");
            return new List<Well>();
        }
    }

    public async Task<IEnumerable<Well>> GetWells(Guid projectId)
    {
        if (_context.Wells != null)
        {
            return await _context.Wells
                .Where(d => d.ProjectId.Equals(projectId))
                .ToListAsync();
        }
        else
        {
            return new List<Well>();
        }
    }
}
