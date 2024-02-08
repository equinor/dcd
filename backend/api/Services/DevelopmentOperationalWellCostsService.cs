using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class DevelopmentOperationalWellCostsService : IDevelopmentOperationalWellCostsService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<SurfService> _logger;
    public DevelopmentOperationalWellCostsService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SurfService>();
    }

    public async Task<DevelopmentOperationalWellCosts?> GetOperationalWellCosts(Guid id)
    {
        var operationalWellCosts = await _context.DevelopmentOperationalWellCosts!
            .Include(dowc => dowc.Project)
            .FirstOrDefaultAsync(o => o.Id == id);
        return operationalWellCosts;
    }
}

