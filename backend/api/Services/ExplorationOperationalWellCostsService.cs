using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ExplorationOperationalWellCostsService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ILogger<SurfService> _logger;
        public ExplorationOperationalWellCostsService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<SurfService>();
        }

        public ExplorationOperationalWellCostsDto? UpdateOperationalWellCosts(ExplorationOperationalWellCostsDto dto)
        {
            var existing = GetOperationalWellCostsByProjectId(dto.ProjectId);
            if (existing == null)
            {
                return null;
            }
            ExplorationOperationalWellCostsAdapter.ConvertExisting(existing, dto);

            _context.ExplorationOperationalWellCosts!.Update(existing);
            _context.SaveChanges();
            var updatedDto = ExplorationOperationalWellCostsDtoAdapter.Convert(existing);
            return updatedDto;
        }

        public ExplorationOperationalWellCostsDto CreateOperationalWellCosts(ExplorationOperationalWellCostsDto dto)
        {
            var explorationOperationalWellCosts = ExplorationOperationalWellCostsAdapter.Convert(dto);
            var project = _projectService.GetProject(dto.ProjectId);
            explorationOperationalWellCosts.Project = project;
            _context.ExplorationOperationalWellCosts!.Add(explorationOperationalWellCosts);
            _context.SaveChanges();
            return ExplorationOperationalWellCostsDtoAdapter.Convert(explorationOperationalWellCosts);
        }
        public ExplorationOperationalWellCosts? GetOperationalWellCostsByProjectId(Guid id)
        {
            var operationalWellCosts = _context.ExplorationOperationalWellCosts!
                .FirstOrDefault(o => o.ProjectId == id);
            return operationalWellCosts;
        }

        public ExplorationOperationalWellCosts? GetOperationalWellCosts(Guid id)
        {
            var operationalWellCosts = _context.ExplorationOperationalWellCosts!
                .Include(eowc => eowc.Project)
                .FirstOrDefault(o => o.Id == id);
            return operationalWellCosts;
        }
    }
}
