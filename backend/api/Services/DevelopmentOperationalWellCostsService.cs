using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

namespace api.Services
{
    public class DevelopmentOperationalWellCostsService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ILogger<SurfService> _logger;
        public DevelopmentOperationalWellCostsService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<SurfService>();
        }

        public DevelopmentOperationalWellCostsDto? UpdateOperationalWellCosts(DevelopmentOperationalWellCostsDto dto)
        {
            var existing = GetOperationalWellCosts(dto.ProjectId);
            if (existing == null)
            {
                return null;
            }
            DevelopmentOperationalWellCostsAdapter.ConvertExisting(existing, dto);

            _context.DevelopmentOperationalWellCosts!.Update(existing);
            _context.SaveChanges();
            var updatedDto = DevelopmentOperationalWellCostsDtoAdapter.Convert(existing);
            return updatedDto;
        }
        public DevelopmentOperationalWellCostsDto CreateOperationalWellCosts(DevelopmentOperationalWellCostsDto dto)
        {
            var explorationOperationalWellCosts = DevelopmentOperationalWellCostsAdapter.Convert(dto);
            var project = _projectService.GetProjectWithAssets(dto.ProjectId);
            explorationOperationalWellCosts.Project = project;
            _context.DevelopmentOperationalWellCosts!.Add(explorationOperationalWellCosts);
            _context.SaveChanges();
            return DevelopmentOperationalWellCostsDtoAdapter.Convert(explorationOperationalWellCosts);
        }
        public DevelopmentOperationalWellCosts? GetOperationalWellCosts(Guid id)
        {
            var operationalWellCosts = _context.DevelopmentOperationalWellCosts!
                .FirstOrDefault(o => o.ProjectId == id);
            return operationalWellCosts;
        }
    }
}
