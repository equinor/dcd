using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class SubstructureService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public SubstructureService(DcdDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public IEnumerable<Substructure> GetSubstructures(Guid projectId)
        {
            if (_context.Substructures != null)
            {
                return _context.Substructures
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Substructure>();
            }
        }

        public Substructure CreateSubstructure(Substructure substructrure)
        {
            var project = _projectService.GetProject(substructrure.ProjectId);
            _projectService.AddSubstructure(project, substructrure);
            var result = _context.Substructures!.Add(substructrure);
            _context.SaveChanges();
            return result.Entity;
        }
    }
}
