using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
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

        public async Task<ExplorationOperationalWellCostsDto?> UpdateOperationalWellCostsAsync(ExplorationOperationalWellCostsDto dto)
        {
            var existing = await GetOperationalWellCostsByProjectIdAsync(dto.ProjectId);
            if (existing == null)
            {
                return null;
            }
            ExplorationOperationalWellCostsAdapter.ConvertExisting(existing, dto);

            _context.ExplorationOperationalWellCosts!.Update(existing);
            await _context.SaveChangesAsync();
            var updatedDto = ExplorationOperationalWellCostsDtoAdapter.Convert(existing);
            return updatedDto;
        }

        public async Task<ExplorationOperationalWellCostsDto> CreateOperationalWellCostsAsync(ExplorationOperationalWellCostsDto dto)
        {
            var explorationOperationalWellCosts = ExplorationOperationalWellCostsAdapter.Convert(dto);
            var project = await _projectService.GetProjectAsync(dto.ProjectId);
            explorationOperationalWellCosts.Project = project;
            _context.ExplorationOperationalWellCosts!.Add(explorationOperationalWellCosts);
            await _context.SaveChangesAsync();
            return ExplorationOperationalWellCostsDtoAdapter.Convert(explorationOperationalWellCosts);
        }

        public async Task<ExplorationOperationalWellCosts?> GetOperationalWellCostsByProjectIdAsync(Guid id)
        {
            var operationalWellCosts = await _context.ExplorationOperationalWellCosts!
                .FirstOrDefaultAsync(o => o.ProjectId == id);
            return operationalWellCosts;
        }

        public async Task<ExplorationOperationalWellCosts?> GetOperationalWellCostsAsync(Guid id)
        {
            var operationalWellCosts = await _context.ExplorationOperationalWellCosts!
                .Include(eowc => eowc.Project)
                .FirstOrDefaultAsync(o => o.Id == id);
            return operationalWellCosts;
        }
    }
}
