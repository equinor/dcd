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

    public async Task<ProjectDto> DeleteWell(Guid wellId)
    {
        var wellItem = await GetWell(wellId);
        _context.Wells!.Remove(wellItem);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(wellItem.ProjectId);
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
}
