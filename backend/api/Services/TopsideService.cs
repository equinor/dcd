using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{

    public class TopsideService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public TopsideService(DcdDbContext context, ProjectService
                projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public IEnumerable<Topside> GetTopsides(Guid projectId)
        {
            if (_context.Topsides != null)
            {
                return _context.Topsides
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                    .Where(d => d.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Topside>();
            }
        }

        public Topside CreateTopside(Topside topside)
        {
            AddTopsideToProject(topside);
            var result = _context.Topsides!.Add(topside);
            _context.SaveChanges();
            return result.Entity;
        }
        private void AddTopsideToProject(Topside topside)
        {
            var project = _projectService.GetProject(topside.Project.Id);
            _projectService.AddTopside(project, topside);
        }
    }
}
