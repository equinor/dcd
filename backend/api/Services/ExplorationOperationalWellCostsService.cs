using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

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
            var existing = GetOperationalWellCosts(dto.ProjectId);
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
            var project = _projectService.GetProjectWithBaggage(dto.ProjectId);
            explorationOperationalWellCosts.Project = project;
            _context.ExplorationOperationalWellCosts!.Add(explorationOperationalWellCosts);
            _context.SaveChanges();
            return ExplorationOperationalWellCostsDtoAdapter.Convert(explorationOperationalWellCosts);
        }
        public ExplorationOperationalWellCosts? GetOperationalWellCosts(Guid id)
        {
            var operationalWellCosts = _context.ExplorationOperationalWellCosts!
                .FirstOrDefault(o => o.ProjectId == id);
            return operationalWellCosts;
        }
    }
}
