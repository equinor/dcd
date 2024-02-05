using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
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

        public async Task<DevelopmentOperationalWellCostsDto?> UpdateOperationalWellCostsAsync(DevelopmentOperationalWellCostsDto dto)
        {
            var existing = await GetOperationalWellCostsByProjectIdAsync(dto.ProjectId);
            if (existing == null)
            {
                return null;
            }
            DevelopmentOperationalWellCostsAdapter.ConvertExisting(existing, dto);

            _context.DevelopmentOperationalWellCosts!.Update(existing);
            await _context.SaveChangesAsync();
            var updatedDto = DevelopmentOperationalWellCostsDtoAdapter.Convert(existing);
            return updatedDto;
        }

        public async Task<DevelopmentOperationalWellCostsDto> CreateOperationalWellCostsAsync(DevelopmentOperationalWellCostsDto dto)
        {
            var developmentOperationalWellCosts = DevelopmentOperationalWellCostsAdapter.Convert(dto);
            var project = await _projectService.GetProjectAsync(dto.ProjectId);
            developmentOperationalWellCosts.Project = project;
            _context.DevelopmentOperationalWellCosts!.Add(developmentOperationalWellCosts);
            await _context.SaveChangesAsync();
            return DevelopmentOperationalWellCostsDtoAdapter.Convert(developmentOperationalWellCosts);
        }

        public async Task<DevelopmentOperationalWellCosts?> GetOperationalWellCostsByProjectIdAsync(Guid id)
        {
            var operationalWellCosts = await _context.DevelopmentOperationalWellCosts!
                .FirstOrDefaultAsync(o => o.ProjectId == id);
            return operationalWellCosts;
        }

        public async Task<DevelopmentOperationalWellCosts?> GetOperationalWellCostsAsync(Guid id)
        {
            var operationalWellCosts = await _context.DevelopmentOperationalWellCosts!
                .Include(dowc => dowc.Project)
                .FirstOrDefaultAsync(o => o.Id == id);
            return operationalWellCosts;
        }
    }
}
