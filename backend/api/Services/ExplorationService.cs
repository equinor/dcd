using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{

    public class ExplorationService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public ExplorationService(DcdDbContext context, ProjectService
                projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public IEnumerable<Exploration> GetExplorations(Guid projectId)
        {
            if (_context.Explorations != null)
            {
                return _context.Explorations
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.GAndGAdminCost)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DrillingSchedule)
                            .ThenInclude(c => c.YearValues)
                    .Where(d => d.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Exploration>();
            }
        }

        public Exploration CreateExploration(Exploration exploration)
        {
            AddExplorationToProject(exploration);
            var result = _context.Explorations!.Add(exploration);
            _context.SaveChanges();
            return result.Entity;
        }
        private void AddExplorationToProject(Exploration exploration)
        {
            var project = _projectService.GetProject(exploration.Project.Id);
            _projectService.AddExploration(project, exploration);
        }
    }
}
