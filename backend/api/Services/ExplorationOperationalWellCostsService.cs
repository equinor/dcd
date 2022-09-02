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

        public ExplorationOperationalWellCostsDto? UpdateOperationalWellCosts(ExplorationOperationalWellCostsDto updatedSurfDto)
        {
            var existing = GetOperationalWellCosts(updatedSurfDto.Id);
            if (existing == null)
            {
                return null;
            }
            var updated = ExplorationOperationalWellCostsAdapter.Convert(updatedSurfDto);

            _context.ExplorationOperationalWellCosts!.Update(updated);
            _context.SaveChanges();
            var updatedDto = ExplorationOperationalWellCostsDtoAdapter.Convert(updated);
            return updatedDto;
            // return _projectService.GetProjectDto(existing.ProjectId);
        }
        public ExplorationOperationalWellCosts GetOperationalWellCosts(Guid id)
        {
            var operationalWellCosts = _context.ExplorationOperationalWellCosts!
                .FirstOrDefault(o => o.Id == id);
            if (operationalWellCosts == null)
            {
                throw new ArgumentException(string.Format("OperationalWellCosts {0} not found.", id));
            }
            return operationalWellCosts;
        }
    }
}
