using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class OperationalWellCostsService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ILogger<SurfService> _logger;
        public OperationalWellCostsService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<SurfService>();

        }

        public ProjectDto UpdateOperationalWellCosts(OperationalWellCostsDto updatedSurfDto)
        {
            var existing = GetSurf(updatedSurfDto.Id);
            ProjectAdapter.Convert(updatedSurfDto);

            _context.OperationalWellCosts!.Update(existing);
            _context.SaveChanges();
            return _projectService.GetProjectDto(existing.ProjectId);
        }
        public OperationalWellCosts GetSurf(Guid surfId)
        {
            var surf = _context.Surfs!
                .Include(c => c.CostProfile)
                .Include(c => c.CessationCostProfile)
                .FirstOrDefault(o => o.Id == surfId);
            if (surf == null)
            {
                throw new ArgumentException(string.Format("Surf {0} not found.", surfId));
            }
            return surf;
        }
    }
}
