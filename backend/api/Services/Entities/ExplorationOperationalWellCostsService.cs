using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationOperationalWellCostsService : IExplorationOperationalWellCostsService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<SurfService> _logger;
    public ExplorationOperationalWellCostsService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SurfService>();
    }

    public async Task<ExplorationOperationalWellCosts?> GetOperationalWellCosts(Guid id)
    {
        var operationalWellCosts = await _context.ExplorationOperationalWellCosts!
            .Include(eowc => eowc.Project)
            .FirstOrDefaultAsync(o => o.Id == id);
        return operationalWellCosts;
    }
}
