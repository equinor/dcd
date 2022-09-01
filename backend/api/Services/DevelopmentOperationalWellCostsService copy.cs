using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

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

        public DevelopmentOperationalWellCostsDto UpdateOperationalWellCosts(DevelopmentOperationalWellCostsDto updatedSurfDto)
        {
            var existing = GetOperationalWellCosts(updatedSurfDto.Id);
            if (existing == null) {
                return null;
            }
            var updated = ProjectAdapter.Convert(updatedSurfDto);

            _context.DevelopmentOperationalWellCosts!.Update(updated);
            _context.SaveChanges();
            var updatedDto = ProjectDtoAdapter.Convert(updated);
            return updatedDto;
            // return _projectService.GetProjectDto(existing.ProjectId);
        }
        public DevelopmentOperationalWellCosts GetOperationalWellCosts(Guid id)
        {
            var operationalWellCosts = _context.DevelopmentOperationalWellCosts!
                .FirstOrDefault(o => o.Id == id);
            if (operationalWellCosts == null)
            {
                throw new ArgumentException(string.Format("OperationalWellCosts {0} not found.", id));
            }
            return operationalWellCosts;
        }
    }
}
